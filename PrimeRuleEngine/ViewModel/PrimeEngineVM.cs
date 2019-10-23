using PrimeRuleEngine.Model;
using PrimeRuleEngine.ViewModel.RuleProcessor;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

namespace PrimeRuleEngine.ViewModel
{
    public class PrimeEngineVM : INotifyPropertyChanged
    {
        #region Properties

        private string fPath;
        public string InputFilePath
        {
            get { return fPath; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    fPath = value;
                    IsApplyRuleVisible = true;
                }
                RaiseProperChanged();
            }
        }

        private string pName;
        public string ParamName
        {
            get { return pName; }
            set
            {
                pName = value;
                RaiseProperChanged();
            }
        }

        private object pVal;
        public object ParamValue
        {
            get { return pVal; }
            set
            {
                pVal = value;
                RaiseProperChanged();
            }
        }

        private string oper;
        public string Operator
        {
            get { return oper; }
            set
            {
                oper = value;
                RaiseProperChanged();
            }
        }

        private string keypName;
        public string KeyParamName
        {
            get { return keypName; }
            set
            {
                keypName = value;
                RaiseProperChanged();
            }
        }

        private string keypVal;
        public string KeyParamValue
        {
            get { return keypVal; }
            set
            {
                keypVal = value;
                RaiseProperChanged();
            }
        }

        private string signal;
        public string Signal
        {
            get { return signal; }
            set
            {
                signal = value;
                RaiseProperChanged();
            }
        }

        private string val;
        public string Value
        {
            get { return val; }
            set
            {
                val = value;
                RaiseProperChanged();
            }
        }

        private string val_type;
        public string Value_type
        {
            get { return val_type; }
            set
            {
                val_type = value;
                RaiseProperChanged();
            }
        }

        private ObservableCollection<DataUnit> failedContents;
        public ObservableCollection<DataUnit> FailedContents
        {
            get { return failedContents; }
            set
            {
                failedContents = value;
                RaiseProperChanged();
            }
        }

        private string eMsg;
        public string ErrorMessage
        {
            get { return eMsg; }
            set
            {
                eMsg = value;
                RaiseProperChanged();
            }
        }

        private bool isApplyRuleVisible;
        public bool IsApplyRuleVisible
        {
            get { return isApplyRuleVisible; }
            set
            {
                isApplyRuleVisible = value;
                RaiseProperChanged();
            }
        }

        

        #endregion

        #region Commands

        public RelayCommand ValidateAndSaveRuleCommand { get; set; }
        public RelayCommand ApplyRuleCommand { get; set; }

        #endregion

        #region Constructor

        public PrimeEngineVM()
        {
            ValidateAndSaveRuleCommand = new RelayCommand(o => ValidateAndSaveRuleCommandExecute(), o => true);
            ApplyRuleCommand = new RelayCommand(o => ApplyRuleCommandExecute(), o => true);
        }

        #endregion

        #region methods

        private void ValidateAndSaveRuleCommandExecute()
        {
            try
            {
                bool res = RuleValidator.Instance.IsValidRule(ParamName, ParamValue, Operator, KeyParamName, KeyParamValue);
                if (res)
                {
                    var rule = RuleValidator.Instance.CreateRuleObject(ParamName, ParamValue, Operator, KeyParamName, KeyParamValue);
                    RuleValidator.Instance.StoreRule(rule, InputFilePath);
                    ErrorMessage = "Rule verified and stored";
                    IsApplyRuleVisible = !string.IsNullOrEmpty(InputFilePath);
                }
                else {
                    ErrorMessage = "Invalid rule, please provide proper inputs for all rule fields.";
                }
            }
            catch(Exception ex) { ErrorMessage = "Error occured, please provide valid inputs."; }
        }

        private void ApplyRuleCommandExecute()
        {
            try
            {
                bool res = RuleValidator.Instance.IsValidRule(ParamName, ParamValue, Operator, KeyParamName, KeyParamValue);                
                if(res)
                {
                    using (StreamReader r = new StreamReader(InputFilePath))
                    {
                        string json = r.ReadToEnd();
                        var deserializedResult = new JavaScriptSerializer().Deserialize<IList<IDictionary<string, string>>>(json);
                        FailedContents = new ObservableCollection<DataUnit>();
                        var filteredList = deserializedResult.Where(x => ValidateBasicRules(x)).ToList();

                        if (FailedContents.Count() > 0)
                            ErrorMessage = "Invalid Data in the file, File contents should follow the rule engine format.";
                        else
                            ApplyRules(filteredList);
                    }
                }
                else
                    ErrorMessage = "Invalid rule, please provide proper inputs for all rule fields.";
            }
            catch (Exception ex) { }
        }

        private bool ValidateBasicRules(IDictionary<string, string> content)
        {
            bool res = false;
            var failedList = new List<DataUnit>();
            var typeList = new List<string> { "Integer", "String", "Datetime" };
            try
            {
                Regex r = new Regex("^[a-zA-Z0-9]*$");
                if (!r.IsMatch(content["signal"]) || !(content["value"] is string) || !typeList.Contains(content["value_type"]))
                    res = false;
                else
                {
                    switch (content["value_type"])
                    {
                        case "Integer":
                            res = Regex.IsMatch(content["value"], @"\d");
                            break;
                        case "String":
                            res = content["value"] is string;
                            break;
                        case "Datetime":
                            DateTime dt;
                            string[] formats = {"M/d/yyyy h:mm:ss tt", "M/d/yyyy h:mm tt",
                                           "MM/dd/yyyy hh:mm:ss", "M/d/yyyy h:mm:ss",
                                           "M/d/yyyy hh:mm tt", "M/d/yyyy hh tt",
                                           "M/d/yyyy h:mm", "M/d/yyyy h:mm",
                                           "MM/dd/yyyy hh:mm", "M/dd/yyyy hh:mm","yyyy-MM-dd HH:mm:ss"};
                            res = DateTime.TryParseExact(content["value"], formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);
                            break;
                    }

                }
                if (!res)
                {
                    FailedContents.Add(
                        new DataUnit
                        {
                            signal = content["signal"],
                            value = content["value"],
                            value_type = content["value_type"]
                        });
                }
            }
            catch (Exception ex) { }
            return res;
        }

        private void ApplyRules(IList<IDictionary<string, string>> filteredList)
        {
            try
            {
                ErrorMessage = string.Empty;
                var rule = RuleValidator.Instance.CreateRuleObject(ParamName, ParamValue, Operator, KeyParamName, KeyParamValue);
                foreach (var item in filteredList)
                {
                    var status = RuleValidator.Instance.EvaluateRule(rule, item);
                    if (!status)
                        FailedContents.Add(new DataUnit
                        {
                            signal = item["signal"],
                            value = item["value"],
                            value_type = item["value_type"]
                        });
                }
                if (!string.IsNullOrEmpty(InputFilePath))
                {
                    RuleValidator.Instance.StoreRule(rule, InputFilePath);
                    ErrorMessage = "Rule applied to input stream and stored.";
                }
                else
                {
                    ErrorMessage = "Error in file store.";
                }
            } catch(Exception ex) { }
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaiseProperChanged([CallerMemberName] string caller = "")
        {

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(caller));
            }
        }

    }
}
