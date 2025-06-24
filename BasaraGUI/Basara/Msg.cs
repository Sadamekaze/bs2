using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Basara
{
    public static class Msg
    {
        public static void Export(string inputFilePath, string outputFilePath, Dictionary<string, string> table)
        {
            using BinaryReader msgReader = new BinaryReader(File.OpenRead(inputFilePath));
            using StreamWriter txtWriter = new StreamWriter(File.Create(outputFilePath));
            
            string magic = new string(msgReader.ReadChars(4));
            if (magic.ToLower() != "#msg")
            {
                MessageBox.Show("Not a .pack file");
                return;
                //throw new Exception("Not a .msg file");
            }

            int sizeData = msgReader.ReadInt32() * 2;
            int offsetAndSizeCount = msgReader.ReadInt32();
            int sizeHeader = msgReader.ReadInt32();
            int offsetToData = msgReader.ReadInt32();

            Dictionary<int,int> stringOffsetAndSize = new Dictionary<int,int>();
            for (int i = 0; i < offsetAndSizeCount; i++)
            {
                int offsetString = (msgReader.ReadInt32() * 2) + offsetToData;
                int sizeString = msgReader.ReadInt32() * 2;
                stringOffsetAndSize.Add(offsetString,sizeString);
            }

            foreach(var offsetsAndSize in stringOffsetAndSize)
            {
                msgReader.BaseStream.Seek(offsetsAndSize.Key, SeekOrigin.Begin);
                byte[] stringLineEncode = msgReader.ReadBytes(offsetsAndSize.Value);
                string stringLineDecode = TableAndEncodeDecode.DecodeBytes(stringLineEncode, table);
                txtWriter.Write(stringLineDecode + "\n");
            }
        }
        public static void Import(string inputFilePath, string outputFilePath, Dictionary<string, string> table)
        {
            string[] allLines = File.ReadAllLines(inputFilePath);
            using BinaryWriter msgWritter = new BinaryWriter(File.Create(outputFilePath));

            // Write header info
            msgWritter.Write(Encoding.UTF8.GetBytes("#MSG"));
            msgWritter.Write(new byte[4]); //padding for sizeData
            msgWritter.Write(allLines.Length);
            msgWritter.Write(0x14); //size header always 0x14
            int paddingSizeOffsetAndSize = 4 * (allLines.Length * 2);
            msgWritter.Write(0x14 + paddingSizeOffsetAndSize);
            msgWritter.Write(new byte[paddingSizeOffsetAndSize]);

            Dictionary<int, int> offsetAndSizeString = new Dictionary<int, int>();

            foreach (string line in allLines)
            {
                byte[] encodeString = TableAndEncodeDecode.EncodeBytes(line, table);
                int offset = (int)msgWritter.BaseStream.Position - (0x14 + paddingSizeOffsetAndSize);
                offsetAndSizeString.Add(offset / 2, encodeString.Length / 2);
                msgWritter.Write(encodeString);
            }

            int sizeData = (int)msgWritter.BaseStream.Position - (0x14 + paddingSizeOffsetAndSize);
            msgWritter.BaseStream.Seek(0x4, SeekOrigin.Begin);
            msgWritter.Write(sizeData / 2);

            msgWritter.BaseStream.Seek(0x14, SeekOrigin.Begin);
            foreach(KeyValuePair<int,int> offsetAndSize in offsetAndSizeString)
            {
                msgWritter.Write(offsetAndSize.Key);
                msgWritter.Write(offsetAndSize.Value);
            }
        }
    }
}
