using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kiselov_HW_WPF_Base64
{
    class Base64Convertor
    {
        private static string strCompareble = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";

        /// <summary>
        /// Method for text convertion from Text to Base64
        /// </summary>
        /// <param name="bArrayForEncoding"></param>
        /// <returns></returns>
        public static string Encode(byte [] bArrayForEncoding)
        {
            // create an empty char array. Length of array equals the length of byte array like 4/3
            StringBuilder strEncoded = new StringBuilder(bArrayForEncoding.Length*4/3);
            // max index in strCompareble string = 63 
            int indexNum=0;

            // loop's step = 3 bytes, which converts into 4 bytes of Base64
            for (int i = 0; i < bArrayForEncoding.Length; i += 3)
            {
                /*
                First byte in Text:
                01010101 AND
                11111100
                -------- 
                010101-- 
                First byte in Base64 : than >>2 =>00010101
                */
                indexNum = (bArrayForEncoding[i] & 0xFC) >> 2;          // 0xFC=252      11111100
                strEncoded.Append(strCompareble[indexNum]);

                /* FOR NEXT BYTE IN Base64Block:
                Second byte in Base64: 
                First byte in TextBlock covers with 0x03 (00000011) and <<4 =>
                01010101 AND
                00000011
                --------
                00000001
                than << 4 => 00010000
                */
                indexNum = (bArrayForEncoding[i] & 0x03) << 4;          // 0x03=3        00000011 

                // NEXT BYTE IN TEXT BLOCK 
                if (i + 1 < bArrayForEncoding.Length)
                {
                    /*
                    From previous stage we have indexNum = (bArrayForEncoding[i] & 0x03) << 4 
                    and now we are comparing it with (bArrayForEncoding[i + 1] & 0xF0) >> 4 
                    Using operator OR (|) 
                    01010101 OR
                    00010000
                    -------- 
                    01010101 
                    */
                    indexNum |= (bArrayForEncoding[i + 1] & 240) >> 4; // 0xF0=240    11110000
                    strEncoded.Append(strCompareble[indexNum]);

                    /*
                    FOR NEXT BYTE IN BASE64BLOCK

                    */
                    indexNum = (bArrayForEncoding[i + 1] & 15) << 2;  // 0x0F=15      00001111

                    /*
                    FOR NEXT BYTE IN TEXTBLOCK
                    */
                    if (i + 2 < bArrayForEncoding.Length)
                    {
                        indexNum |= (bArrayForEncoding[i + 2] & 192) >> 6; //0xC0=192      11000000 
                        strEncoded.Append(strCompareble[indexNum]);

                        indexNum = bArrayForEncoding[i + 2] & 63;         //0x3F=63       00111111
                        strEncoded.Append(strCompareble[indexNum]);
                    }
                    // If the length of this TextBlock is less than 3 for 1 symbol 
                    else
                    {
                        strEncoded.Append(strCompareble[indexNum]);
                        strEncoded.Append('=');
                    }
                }
                // If the length of this TextBlock is less than 3 for 2 symbols 
                else
                {
                    strEncoded.Append(strCompareble[indexNum]);
                    strEncoded.Append("==");
                }
            }
            return strEncoded.ToString();
        }

        /// <summary>
        /// Method for text convertion from Base64 to Text 
        /// </summary>
        /// <param name="strEncoded"></param>
        /// <returns></returns>
        public static byte[] Decode(string strEncoded)
        {
            // Inspection for multiplicity of current encoded string
            if (strEncoded.Length % 4 != 0)
            {
                throw new Exception("Inputted string is not valid for FromBase64ToText Conversion");
            }

            // for cutting out the encoded string for amount of symbols "=" in its tail 
            byte[] byteArrDecoded =
                new byte[((strEncoded.Length * 3) / 4) - (strEncoded.IndexOf('=') > 0 
                ? (strEncoded.Length - strEncoded.IndexOf('=')) 
                : 0)];
            // for access to exach index in the string 
            StringBuilder strbEncoded = new StringBuilder(strEncoded);
            // counter 
            int j = 0;
            // array of symbols (in BYTES) in 1 Base64 block which equals to 3 symbols in TextBlock 
            int[] b = new int[4];
            // loop's step = 4 (amount of elements in Base64 Block )
            for (int i = 0; i < strbEncoded.Length; i += 4)
            {
                // Find index in Compareble string for each symbol in encoded string 
                b[0] = strCompareble.IndexOf(strbEncoded[i]);
                b[1] = strCompareble.IndexOf(strbEncoded[i + 1]);
                b[2] = strCompareble.IndexOf(strbEncoded[i + 2]);
                b[3] = strCompareble.IndexOf(strbEncoded[i + 3]);

                /* First byte in TextBlock consist of: 
                b[0] = 01010101 => <<2
                b[1] = 00100101 => >>4 
                TextBlock:      010101   01 || 0110   1100 || 0111 0011
                Base64Block:    110011 || 01   0100 || 1101   11 || 111111 
                */
                // Comparing with index of symbol "=" - 64
                byteArrDecoded[j++] = (byte)((b[0] << 2) | (b[1] >> 4));
                if (b[2] < 64)
                {
                    byteArrDecoded[j++] = (byte)((b[1] << 4) | (b[2] >> 2));
                    if (b[3] < 64)
                    {
                        byteArrDecoded[j++] = (byte)((b[2] << 6) | b[3]);
                    }
                }
            }
            return byteArrDecoded;
        }
    }
}
