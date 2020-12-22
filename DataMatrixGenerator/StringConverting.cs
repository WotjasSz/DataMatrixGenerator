namespace DataMatrixGenerator
{
    public class StringConverting
    {
        public static readonly string[] LowNames =
            {
            "NUL", "SOH", "STX", "ETX", "EOT", "ENQ", "ACK", "BEL",
            "BS", "HT", "LF", "VT", "FF", "CR", "SO", "SI","DLE",
            "DC1", "DC2", "DC3", "DC4", "NAK", "SYN", "ETB","CAN",
            "EM", "SUB", "ESC", "FS", "GS", "RS", "US"
        };

        public static string DisplayString(string text)
        {
            string string1 = "", string2 = "";
            string outputString = "";
            foreach (char c in text)
            {
                if (c < 32)
                {
                    //outputString = outputString + string.Format("<{0}> - U+{1:x4}\n", LowNames[c], (int)c);
                    string1 = string1 + string.Format("<{0}>", LowNames[c]);
                    string2 = string2 + string.Format("{0,6:x4}", (int)c);
                }
                else if (c > 127)
                {
                    //outputString = outputString + string.Format("<NON> - U+{0:x4}\n", (int)c);//(Possibly non-printable)
                    string1 = string1 + string.Format("{0}", "<NON>");
                    string2 = string2 + string.Format("{0,6:x4}", (int)c);
                }
                else
                {
                    //outputString = outputString + string.Format("{0} - U+{1:x4}\n", c, (int)c);
                    string1 = string1 + string.Format("{0}", c);
                    string2 = string2 + string.Format("{0,6:x4}", (int)c);
                }
            }
            outputString = string.Format("{0} - {1}\n", string1, string2);
            return outputString;
        }
    }
}
