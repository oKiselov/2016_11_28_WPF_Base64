using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kiselov_HW_WPF_Base64
{
    class Controller
    {
        public static string ConvertFromTextToBase64(string strForEncoding)
        {
            return Base64Convertor.Encode(Encoding.ASCII.GetBytes(strForEncoding));
        }

        public static string ConvertFromBase64ToText(string strForDecoding)
        {
            return Encoding.ASCII.GetString(Base64Convertor.Decode(strForDecoding));
        }
    }
}
