using System;

namespace Temosoft.Bitcoin.Blockchain
{
    public class Block
    {
        public uint MagicId;
        public uint HeaderLength;
        public uint VersionNumber;
        public byte[] PreviousBlockHash;
        public byte[] MerkleRoot;
        public uint TimeStamp;
        public uint Bits;
        public uint Nonce;
        public Transaction[] Transactions;
        public uint LockTime;
        public long Size;
        public long Difficulty
        {
            get { return CalculateDifficulty(); }
        }

        private long CalculateDifficulty()
        {
            //is 0x1b0404cb, the hexadecimal target is
            //0x0404cb * 2**(8*(0x1b - 3)) = 0x00000000000404CB000000000000000000000000000000000000000000000000
            uint p = Bits & 0x00FFFFFF;
            uint e = (Bits & 0xFF000000) >> 24;
            var dif = p*Math.Pow(2, (8*(e - 3)));
            return (long)dif;
        }
    }
}