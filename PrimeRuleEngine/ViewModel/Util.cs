using System;
using System.Globalization;

namespace PrimeRuleEngine.ViewModel
{
    public static class Util
    {
        public static Type GetDataType(object pval)
        {
            bool status = false;
            DateTime dt;

            status = IsValidDateTime(pval.ToString(), out dt);

            if (!status)
            {
                double currentData;
                status = double.TryParse(pval.ToString(), out currentData);

                if (!status)
                    return typeof(string);
                else
                    return typeof(double);
            }
            else
                return typeof(DateTime);
        }

        public static bool IsValidDateTime(string val, out DateTime dt)
        {
            string[] formats = {"M/d/yyyy h:mm:ss tt", "M/d/yyyy h:mm tt",
                                           "MM/dd/yyyy hh:mm:ss", "M/d/yyyy h:mm:ss",
                                           "M/d/yyyy hh:mm tt", "M/d/yyyy hh tt",
                                           "M/d/yyyy h:mm", "M/d/yyyy h:mm",
                                           "MM/dd/yyyy hh:mm", "M/dd/yyyy hh:mm","yyyy-MM-dd HH:mm:ss"};
            return DateTime.TryParseExact(val.ToString(), formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);
        }

        public static bool IsInvalidOperation(string condition, object pval)
        {
            bool status = false;
            DateTime dt;

            try {
                status = IsValidDateTime(pval.ToString(), out dt);

                if (!status)
                {
                    double currentData;
                    status = double.TryParse(pval.ToString(), out currentData);

                    if (!status)
                    {
                        status = pval is string;
                        if (status)
                        {
                            if (condition.StartsWith(">") || condition.StartsWith("<") || condition.StartsWith("greater") || condition.StartsWith("lower"))
                                return false;
                        }
                    }
                }
            }
            catch(Exception ex) { }
            return true;
        }

        public static bool ConvertData(object val, Type valType, out dynamic result)
        {
            bool status = false;
            DateTime dt;
            try
            {
                status = IsValidDateTime(val.ToString(), out dt);

                if (status)
                {
                    status = valType == typeof(DateTime);
                    result = dt;
                }
                else
                {
                    double currentData;
                    status = double.TryParse(val.ToString(), out currentData);

                    if (status)
                    {
                        status = valType == typeof(double) || valType == typeof(int);
                        result = currentData;
                    }
                    else
                    {
                        status = valType == typeof(string);
                        result = val.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                result = val.ToString();
            }
            return status;
        }

    }
}
