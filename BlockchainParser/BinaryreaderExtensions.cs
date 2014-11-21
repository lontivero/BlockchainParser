using System.IO;
using System.Text;

namespace Temosoft.Bitcoin.Blockchain
{
    public static class BinaryReaderExtensions
    {
        public static long ReadVarInt(this BinaryReader reader)
        {
            var t = reader.ReadByte();
            if (t < 0xfd) return t;
            if (t == 0xfd)return reader.ReadInt16();
            if (t == 0xfe)return reader.ReadInt32();
            if (t == 0xff)return reader.ReadInt64();
            
            throw new InvalidDataException("Reading Var Int");
        }

        public static byte[] ReadStringAsByteArray(this BinaryReader reader)
        {
            var scriptLength = reader.ReadVarInt();
            return reader.ReadBytes((int)scriptLength);
        }

        public static byte[] ReadHashAsByteArray(this BinaryReader reader)
        {
            return reader.ReadBytes(32);
        }
    }

    public static class HashExtensions
    {
        public static string ToHashString(this byte[] byteArray)
        {
            return Encoding.UTF8.GetString(byteArray);
        }
    }

}