using System;
using System.IO;

namespace PrimeRuleEngine.Model
{
    public static class RuleManager
    {
        public static void SaveRule(RuleElement ruleObj, string filePath)
        {
            string fileName = "RuleBase.txt";

            string strVal = "ParamName####" + ruleObj.FieldName + ",ParamValue####" + ruleObj.FieldValue +
                        ",Condition####" + ruleObj.Condition + ",KeyParam####" + ruleObj.KeyAttribute +
                        ",KeyParamValue###" + ruleObj.KeyValue;
            try
            {
                filePath = Path.Combine(filePath, fileName);

                if (File.Exists(filePath))
                {
                    using (StreamWriter sw = File.AppendText(filePath))
                    {
                        sw.WriteLine(strVal);
                    }
                }
                else
                {
                    using (StreamWriter sw = new StreamWriter(filePath))
                    {
                        sw.WriteLine(strVal);
                    }
                }
            }
            catch (Exception ex)
            {
                using (StreamWriter sw = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName)))
                {
                    sw.WriteLine(strVal);
                }
            }
        }
    }
}
