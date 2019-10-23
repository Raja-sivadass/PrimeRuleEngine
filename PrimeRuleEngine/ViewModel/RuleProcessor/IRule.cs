using PrimeRuleEngine.Model;
using System.Collections.Generic;

namespace PrimeRuleEngine.ViewModel.RuleProcessor
{
    interface IRule
    {
        bool IsValidRule(string pName, object pval, string condition, string keyParam, string keyParamVal);
        bool EvaluateRule(RuleElement ruleObj, IDictionary<string, string> collection);
    }
}
