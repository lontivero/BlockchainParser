using System;
using System.Collections.Generic;
using System.IO;

namespace Temosoft.Bitcoin.Blockchain
{
    public class Block
    {
        private static DateTime _epochBaseDate = new DateTime(1970,1,1);
        private readonly long _position;
        private Stream _stream;
        private readonly Lazy<BinaryReader> _reader;

        public uint HeaderLength;

        private uint _versionNumber;
        private byte[] _previousBlockHash;
        private byte[] _merkleRoot;
        private DateTime _timeStamp;
        private uint _lockTime;
        private long _size;
        private uint _nonce;
        private uint _bits;
        private long _transactionCount;

        public uint VersionNumber
        {
            get
            {
                ReadHeader();
                return _versionNumber;
            }
        }

        public byte[] PreviousBlockHash
        {
            get
            {
                ReadHeader();
                return _previousBlockHash;
            }
        }

        public byte[] MerkleRoot
        {
            get
            {
                ReadHeader();
                return _merkleRoot;
            }
        }

        public DateTime TimeStamp
        {
            get
            {
                ReadHeader();
                return _timeStamp;
            }
        }
        public uint Bits
        {
            get
            {
                ReadHeader();
                return _bits;
            }
        }
        public uint Nonce
        {
            get
            {
                ReadHeader();
                return _nonce;
            }
        }
        public uint LockTime
        {
            get
            {
                ReadHeader();
                return _lockTime;
            }
        }
        public long Size
        {
            get
            {
                ReadHeader();
                return _size;
            }
        }

        private void ReadHeader()
        {
            if(_versionNumber > 0) return;
            var r = _reader.Value;
            r.BaseStream.Position = _position + 4;
            _versionNumber = r.ReadUInt32();
            _previousBlockHash = r.ReadHashAsByteArray();
            _merkleRoot = r.ReadHashAsByteArray();
            _timeStamp = _epochBaseDate.AddSeconds(r.ReadUInt32());
            _bits = r.ReadUInt32();
            _nonce = r.ReadUInt32();
            _transactionCount = r.ReadVarInt();
        }

        public long Difficulty
        {
            get { return CalculateDifficulty(); }
        }

        public IEnumerable<Transaction> Transactions
        {
            get
            {
                var r = _reader.Value;
                for (var ti = 0; ti < _transactionCount; ti++)
                {
                    var t = new Transaction();
                    t.VersionNumber = r.ReadUInt32();

                    var inputCount = r.ReadVarInt();
                    t.Inputs = new Input[inputCount];

                    var outputCount = r.ReadVarInt();
                    t.Outputs = new Output[outputCount];

                    yield return t;
                }
            }
        }

        public Block(Stream stream)
        {
            _position = stream.Position;
            _stream = stream;
            _reader = new Lazy<BinaryReader>(() => new BinaryReader(stream));
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