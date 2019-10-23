namespace PrimeRuleEngine.Model
{
    public class RuleElement
    {
        public string FieldName { get; set; }
        public object FieldValue { get; set; }
        public string Condition { get; set; }
        public string KeyAttribute { get; set; }
        public string KeyValue { get; set; }
    }

    public class DataUnit
    {
        public string signal { get; set; }
        public string value { get; set; }
        public string value_type { get; set; }
    }
}
