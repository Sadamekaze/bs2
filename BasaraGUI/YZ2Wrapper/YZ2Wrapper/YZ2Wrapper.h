#pragma once

using namespace System;

namespace YZ2Wrapper {
    public ref class YZ2Compressor abstract sealed
    {
    public:
        static array<Byte>^ YZ2Decode(String^ filePath);

        static array<Byte>^ YZ2Decode(array<Byte>^ inputBytes);

        static array<Byte>^ YZ2Encode(String^ filePath);

        static array<Byte>^ YZ2Encode(array<Byte>^ inputBytes);
    };
}