using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;

namespace Basara
{
    static class TableAndEncodeDecode
    {
        public static byte[] HexStringToByteArray(string hex)
        {
            if (hex.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                hex = hex.Substring(2);

            if (hex.Length % 2 != 0)
                hex = "0" + hex;

            int length = hex.Length;
            byte[] bytes = new byte[length / 2];

            for (int i = 0; i < length; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);

            Array.Reverse(bytes);
            return bytes;
        }
        public static Dictionary<string,string> AddString(string textFile,Dictionary<string,string> table)
        {
            Dictionary<string, string> copiedTable = new Dictionary<string, string>(table);
            HashSet<string> uniqueKey = new HashSet<string>();
            HashSet<string> uniqueValue = new HashSet<string>();

            Regex regex = new Regex(@"<0x[0-9a-fA-F]+>");
            MatchCollection matches = regex.Matches(textFile);
            foreach (Match match in matches)
            {
                uniqueKey.Add(match.Value);
                uniqueValue.Add(match.Value.Substring(3,match.Value.Length-4));
            }
            List<string> uniqueKeyList = uniqueKey.ToList();
            List<string> uniqueValueList = uniqueValue.ToList();
            for (int i = 0; i < uniqueKeyList.Count;i++)
                copiedTable.Add(uniqueKeyList[i], uniqueValueList[i]); 
            
            return copiedTable;
        }
        public static byte[] EncodeBytes(string textfile, Dictionary<string,string> table)
        {
            table = AddString(textfile,table);
            HashSet<int> uniqueLengths = new HashSet<int>(table.Select(i => i.Key.Length).OrderByDescending(length => length));
            List<byte[]> encode = new List<byte[]>();
            for(int i = 0; i < textfile.Length;)
            {
                bool found = false;
                foreach(int j in uniqueLengths)
                {
                    string byte_pairhex = new string(textfile.Skip(i).Take(j).ToArray());
                    if(table.ContainsKey(byte_pairhex) && !string.IsNullOrEmpty(byte_pairhex))
                    {
                        encode.Add(HexStringToByteArray(table[byte_pairhex]));
                        found = true;
                        i+=j;
                    }
                }
                if(!found)
                {
                    string byte_pairhex = new string(textfile.Skip(i).Take(1).ToArray());
                    Console.WriteLine($"{byte_pairhex} is not in the table, so it will be skipped.");
                    i += 1;
                }
            }
            
            return encode.SelectMany(row => row).ToArray();
        }
        public static string DecodeBytes(byte[] data, Dictionary<string,string> table)
        {
            string decode = "";
            HashSet<int> uniqueLengths = new HashSet<int>(table.Select(i => (i.Key.Length / 2) - 1).OrderByDescending(length => length));
            for(int i = 0; i < data.Length;)
            {
                bool found = false;
                foreach(int j in uniqueLengths)
                {
                    byte[] bytes = data.Skip(i).Take(j).Reverse().ToArray();
                    string bytePairHex = "0x" + BitConverter.ToString(bytes).Replace("-", "");
                    if(table.ContainsKey(bytePairHex))
                    {
                        decode += table[bytePairHex];
                        found = true;
                        i+=j;
                        break;
                    }
                }
                if(!found)
                {
                    byte[] bytes = data.Skip(i).Take(2).Reverse().ToArray();
                    string bytePairHex = BitConverter.ToString(bytes).Replace("-", "");

                    decode += $"<0x{bytePairHex}>";
                    i += 2;
                }
            }
            
            return decode;
        }
    }
}