using PrimeRuleEngine.Model;
using System;
using System.Collections.Generic;
using System.IO;

namespace PrimeRuleEngine.ViewModel.RuleProcessor
{
    public sealed class RuleValidator : IRule
    {
        private static RuleValidator instance = null;
        private static readonly object padlock = new object();

        RuleValidator()
        {
        }

        public static RuleValidator Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new RuleValidator();
                    }
                    return instance;
                }
            }
        }

        public bool IsValidRule(string pName, object pval, string condition, string keyParam, string keyParamVal)
        {
            return !string.IsNullOrEmpty(pName) &&
                        pval != null &&
                        !string.IsNullOrEmpty(condition) && Util.IsInvalidOperation(condition, pval) && 
                        !string.IsNullOrEmpty(keyParam) && !string.IsNullOrEmpty(keyParamVal);
        }

        public RuleElement CreateRuleObject(string pName, object pval, string condition, string key, string keyVal)
        {
            var validRule = IsValidRule(pName, pval, condition, key, keyVal);
            if (!validRule)
            {
                return null;
            }
            else
            {
                return new RuleElement
                {
                    FieldName = pName,
                    FieldValue = pval,
                    Condition = condition,
                    KeyAttribute = key,
                    KeyValue = keyVal
                };
            }
        }

        public bool EvaluateRule(RuleElement ruleObj, IDictionary<string, string> collection)
        {
            bool result = false;
            dynamic currentData;
            dynamic ruleData;
            var ruleValType = Util.GetDataType(ruleObj.FieldValue);
            bool currentVal = Util.ConvertData(collection[ruleObj.FieldName], ruleValType, out currentData);
            bool ruleVal = Util.ConvertData(ruleObj.FieldValue, ruleValType, out ruleData);

            switch (ruleObj.Condition)
            {
                case ">":
                case "greater than":
                    if (collection[ruleObj.KeyAttribute] == ruleObj.KeyValue && currentVal && ruleVal)
                    {
                        result = currentData > ruleData;
                    }
                    else
                        result = true;

                    break;
                case "<":
                case "less than":
                    if (collection[ruleObj.KeyAttribute] == ruleObj.KeyValue && currentVal && ruleVal)
                    {
                        result = currentData < ruleData;
                    }
                    else
                        result = true;
                    break;
                case ">=":
                case "greater than or equal to":
                    if (collection[ruleObj.KeyAttribute] == ruleObj.KeyValue && currentVal && ruleVal)
                    {
                        result = currentData >= ruleData;
                    }
                    else
                        result = true;
                    break;
                case "<=":
                case "less than or equal to":
                    if (collection[ruleObj.KeyAttribute] == ruleObj.KeyValue && currentVal && ruleVal)
                    {
                        result = currentData <= ruleData;
                    }
                    else
                        result = true;
                    break;
                case "==":
                case "equal to":
                    if (collection[ruleObj.KeyAttribute] == ruleObj.KeyValue && currentVal && ruleVal)
                    {
                        result = currentData == ruleData;
                    }
                    else
                        result = true;
                    break;
                case "!=":
                case "not equal to":
                    if (collection[ruleObj.KeyAttribute] == ruleObj.KeyValue && currentVal && ruleVal)
                    {
                        result = currentData != ruleData;
                    }
                    else
                        result = true;
                    break;
                case "startswith":
                    if (collection[ruleObj.KeyAttribute] == ruleObj.KeyValue && currentVal && ruleVal)
                    {
                        result = (currentData as string).StartsWith(ruleData as string);
                    }
                    else
                        result = true;
                    break;
                case "endswith":
                    if (collection[ruleObj.KeyAttribute] == ruleObj.KeyValue && currentVal && ruleVal)
                    {
                        result = (currentData as string).EndsWith(ruleData as string);
                    }
                    else
                        result = true;
                    break;
                case "contains":
                    if (collection[ruleObj.KeyAttribute] == ruleObj.KeyValue && currentVal && ruleVal)
                    {
                        result = (currentData as string).Contains(ruleData as string);
                    }
                    else
                        result = true;
                    break;
            }
            return result;
        }

        public void StoreRule(RuleElement rule, string path)
        {
            try
            {
                var filePath = Path.GetDirectoryName(path);
                RuleManager.SaveRule(rule, filePath);
            }
            catch(Exception ex) { }
        }
    }
}
