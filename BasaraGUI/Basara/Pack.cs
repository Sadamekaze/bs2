using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using YZ2Wrapper;

namespace Basara
{
    public static class Pack
    {
        public static void Export(string inputFilePath, string outputPath)
        {

            bool isFileCompressed = false;
            string extractedFolder = $"{outputPath}/{Path.GetFileNameWithoutExtension(inputFilePath)}";
            BinaryReader packReader = new BinaryReader(File.OpenRead(inputFilePath));

            string magic = new string(packReader.ReadChars(4));
            if (magic.ToLower() != "#fpk") // if magic is not #fpk maybe it means compressed
            {
                byte[] decompressedData= YZ2Compressor.YZ2Decode(inputFilePath);
                packReader.Close();

                packReader = new BinaryReader(new MemoryStream(decompressedData));
                magic = new string(packReader.ReadChars(4));

                if (magic.ToLower() != "#fpk")
                {
                    MessageBox.Show("Not a .pack file");
                    return;
                    //throw new Exception("Not a .pack file");
                }
                isFileCompressed = true;
            }
            if (!Directory.Exists(extractedFolder))
                Directory.CreateDirectory(extractedFolder);

            int fileCount = packReader.ReadInt32();
            int unk = packReader.ReadInt32(); //always 0x10
            int headerSize = packReader.ReadInt32();

            // Read offsets and size
            Dictionary<int, int> fileOffsetAndSize = new Dictionary<int, int>();
            int zeroOffsetCount = 0;
            for (int i = 0; i < fileCount; i++)
            {
                int offsetFile = packReader.ReadInt32();
                int sizeFile = packReader.ReadInt32();

                if (offsetFile != 0)
                    fileOffsetAndSize.Add(offsetFile, sizeFile);
                else
                    zeroOffsetCount++;
            }
            fileCount -= zeroOffsetCount;

            using StreamWriter listWriter = new StreamWriter(File.Create($"{extractedFolder}/file.LIST"));
            listWriter.Write($"FileCount: {fileCount}\n");
            listWriter.Write($"isFileCompressed: {isFileCompressed}\n");

            int fileIndex = 0;
            foreach (KeyValuePair<int, int> offsetAndSize in fileOffsetAndSize)
            {
                packReader.BaseStream.Seek(offsetAndSize.Key, SeekOrigin.Begin);
                byte[] fileBytes = packReader.ReadBytes(offsetAndSize.Value);
                string nameFile;

                if ((char)fileBytes[0] == '#')
                {
                    string extension = Encoding.ASCII.GetString(fileBytes.Skip(1).Take(3).ToArray());
                    nameFile = $"{fileIndex}.{extension.ToUpper()}";
                }
                else
                {
                    nameFile = $"{fileIndex}.UNK";
                }

                using BinaryWriter fileExport = new BinaryWriter(File.Create($"{extractedFolder}/{nameFile}"));
                listWriter.Write($"{nameFile}\n");
                fileExport.Write(fileBytes);
                fileIndex++;
            }
            packReader.Close();
        }

        public static void Import(string inputPath, string outputFilePath)
        {
            // Check if the .list file exists
            string fileListPath = $"{inputPath}/file.LIST";
            if (!File.Exists(fileListPath)) { MessageBox.Show("There is no file named file.LIST."); return; }

            // Read all lines from the .list file
            string[] fileListLines = File.ReadAllLines(fileListPath);
            int fileCount = int.Parse(fileListLines[0].Split(':')[1].Trim());
            bool isFileCompressed = bool.Parse(fileListLines[1].Split(':')[1].Trim());

            // Create tempPack in memory
            MemoryStream memoryStream = new MemoryStream();
            using BinaryWriter tempPackWriter = new BinaryWriter(memoryStream);

            // Write Header Info
            tempPackWriter.Write(Encoding.UTF8.GetBytes("#fpk")); // Magic
            tempPackWriter.Write(fileCount);
            tempPackWriter.Write(0x10); // Unk. Always 0x10
            tempPackWriter.Write(0x10 + (8 * fileCount)); // HeaderSize
            // Padding for offset and size placement
            int paddingOffsetAndSize = 8 * fileCount; 
            paddingOffsetAndSize += paddingOffsetAndSize % 0x10;
            tempPackWriter.Write(new byte[paddingOffsetAndSize]);
            tempPackWriter.Write(new byte[0x10 * 3]); // Separator

            // Read and Write bytes and get offset and size to tempPack
            Dictionary<int, int> offsetAndSizeFile = new Dictionary<int, int>();
            foreach (string nameFile in fileListLines.Skip(2))
            {
                byte[] byteFile = File.ReadAllBytes($"{inputPath}/{nameFile}");
                offsetAndSizeFile.Add((int)tempPackWriter.BaseStream.Position, byteFile.Length);
                tempPackWriter.Write(byteFile);
                tempPackWriter.Write(new byte[(0x10 * 3) + (byteFile.Length % 0x10)]); // separator
            }

            // Write offset and size to header
            tempPackWriter.BaseStream.Seek(0x10, SeekOrigin.Begin);
            foreach (KeyValuePair<int, int> offsetAndSize in offsetAndSizeFile)
            {
                tempPackWriter.Write(offsetAndSize.Key);   // offset
                tempPackWriter.Write(offsetAndSize.Value); // size
            }

            // Write all data to the real pack file, check if the file is compressed
            using BinaryWriter PackWriter = new BinaryWriter(File.Create(outputFilePath));
            byte[] uncompressedPackData = memoryStream.ToArray();
            if (isFileCompressed)
            {
                byte[] compressedData = YZ2Compressor.YZ2Encode(uncompressedPackData);
                PackWriter.Write(compressedData);
            }
            else
            {
                PackWriter.Write(uncompressedPackData);
            }
        }

    }
}
