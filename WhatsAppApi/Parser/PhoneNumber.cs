using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace WhatsAppApi.Parser
{
    public class PhoneNumber
    {
        public string Country;
        public string CC;
        public string Number;
        public string FullNumber
        {
            get
            {
                return CC + Number;
            }
        }
        public string ISO3166;
        public string ISO639;
        protected string _mcc;
        protected string _mnc;

        public string MCC
        {
            get
            {
                return _mcc.PadLeft(3, '0');
            }
        }

        public string MNC
        {
            get
            {
                return _mnc.PadLeft(3, '0');
            }
        }

        public PhoneNumber(string number)
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("WhatsAppApi.Parser.countries.csv"))
            {
                using (var reader = new StreamReader(stream))
                {
                    string csv = reader.ReadToEnd();
                    string[] lines = csv.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string line in lines)
                    {
                        string[] values = line.Trim('\r').Split(',');
                        //try to match
                        if (number.StartsWith(values[1]))
                        {
                            //matched
                            Country = values[0].Trim('"');
                            //hook: Fix CC for North America
                            if (values[1].StartsWith("1"))
                            {
                                values[1] = "1";
                            }
                            CC = values[1];
                            Number = number.Substring(CC.Length);
                            ISO3166 = values[4].Trim('"');
                            ISO639 = values[5].Trim('"');
                            _mcc = values[2].Trim('"');
                            _mnc = values[3].Trim('"');
                            if (_mcc.Contains('|'))
                            {
                                //take first one
                                string[] parts = _mcc.Split('|');
                                _mcc = parts[0];
                            }
                            return;
                        }
                    }
                    //could not match!
                    throw new Exception(String.Format("Could not dissect phone number {0}", number));
                }
            }
        }
    }
}
