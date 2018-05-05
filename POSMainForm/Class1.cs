using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace POSMainForm
{
    class Class1
    {
        public static string IDGenerate(int length)
        {
            var barcode = "";
            try
            {
                var temp = Guid.NewGuid().ToString().Replace("-", string.Empty);
                barcode = Regex.Replace(temp, "[A-Za-z]", string.Empty).Substring(0, length);
            }
            catch { }
            return barcode;
        }
    }
}
