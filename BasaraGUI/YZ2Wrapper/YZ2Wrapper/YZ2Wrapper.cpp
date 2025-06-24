#include "YZ2.h" 

using namespace System;
using namespace System::IO;

namespace YZ2Wrapper {
    public ref class YZ2Compressor abstract sealed
    {
    public:

        static array<Byte>^ YZ2Decode(String^ filePath)
        {
            FileInfo^ fileInfo = gcnew FileInfo(filePath);
            FileStream^ stream = fileInfo->OpenRead();

            array<Byte>^ in_header = gcnew array<Byte>(32);
            stream->Read(in_header, 0, 32);

            bool HasInvalidChar = false;
            bool Has0x09 = false;
            bool Has0x0A = false;

            for (size_t i = 0; i < in_header->Length; i++)
            {
                if (in_header[i] == 0x09 && Has0x09 == false)
                {
                    Has0x09 = true;
                }
                else if (in_header[i] == 0x0A && Has0x0A == false)
                {
                    Has0x0A = true;
                }
                else if (
                    (in_header[i] >= 0x01 && in_header[i] <= 0x2F)
                    || (in_header[i] >= 0x3A && in_header[i] <= 0x40)
                    || (in_header[i] >= 0x47 && in_header[i] <= 0x60)
                    || (in_header[i] >= 0x67 && in_header[i] <= 0xFF)
                    )
                {
                    HasInvalidChar = true;
                }
            }

            if (!HasInvalidChar && Has0x09 && Has0x0A) // valid YZ2
            {
                array<unsigned char>^ in_content = gcnew array<unsigned char>(stream->Length);

                stream->Position = 0;
                stream->Read(in_content, 0, fileInfo->Length);
                stream->Close();

                array<unsigned char>^ out_content;

                YZ2::YZ2Actions::YZ2Decode(in_content, out_content);

                return out_content;
            }
            else
            {
                stream->Close();
                throw gcnew Exception("The provided file is not a valid YZ2 file.");
            }
        }

        static array<Byte>^ YZ2Decode(array<Byte>^ inputBytes)
        {
            array<Byte>^ in_header = gcnew array<Byte>(32);
            Array::Copy(inputBytes, 0, in_header, 0, 32);

            bool HasInvalidChar = false;
            bool Has0x09 = false;
            bool Has0x0A = false;

            for (size_t i = 0; i < in_header->Length; i++)
            {
                if (in_header[i] == 0x09 && Has0x09 == false)
                {
                    Has0x09 = true;
                }
                else if (in_header[i] == 0x0A && Has0x0A == false)
                {
                    Has0x0A = true;
                }
                else if (
                    (in_header[i] >= 0x01 && in_header[i] <= 0x2F)
                    || (in_header[i] >= 0x3A && in_header[i] <= 0x40)
                    || (in_header[i] >= 0x47 && in_header[i] <= 0x60)
                    || (in_header[i] >= 0x67 && in_header[i] <= 0xFF)
                    )
                {
                    HasInvalidChar = true;
                }
            }

            if (!HasInvalidChar && Has0x09 && Has0x0A) // valid YZ2
            {
                array<unsigned char>^ in_content = inputBytes;
                array<unsigned char>^ out_content;

                YZ2::YZ2Actions::YZ2Decode(in_content, out_content);

                return out_content;
            }
            else
            {
                throw gcnew Exception("The provided file is not a valid YZ2 file.");
            }
        }

        static array<Byte>^ YZ2Encode(String^ filePath)
        {
            FileInfo^ fileInfo = gcnew FileInfo(filePath);
            FileStream^ stream = fileInfo->OpenRead();

            array<unsigned char>^ in_content = gcnew array<unsigned char>(fileInfo->Length);

            stream->Read(in_content, 0, fileInfo->Length);
            stream->Close();

            array<unsigned char>^ out_content;

            YZ2::YZ2Actions::YZ2Encode(in_content, out_content);

            return out_content;
        }

        static array<Byte>^ YZ2Encode(array<Byte>^ inputBytes)
        {
            array<unsigned char>^ in_content = inputBytes;

            array<unsigned char>^ out_content;

            YZ2::YZ2Actions::YZ2Encode(in_content, out_content);

            return out_content;
        }
    };
}