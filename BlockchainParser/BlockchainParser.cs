using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;

namespace Temosoft.Bitcoin.Blockchain
{
    public abstract class BlockchainParser
    {
        public void Parse(string[] filesPath)
        {
#if true
            var streams = filesPath
                .Select(filePath => MemoryMappedFile.CreateFromFile(filePath, FileMode.Open, Path.GetFileName(filePath), 0, MemoryMappedFileAccess.Read))
                .Select(mmf => mmf.CreateViewStream(0, 0, MemoryMappedFileAccess.Read))
                .Cast<Stream>()
                .ToList();
#else
            var streams = filesPath
                .Select(filePath => new FileStream(filePath, FileMode.Open, FileAccess.Read))
                .ToList();
            
#endif
            var bufferedStream = new MultipleFilesStream(streams);
            Parse(bufferedStream);
        }

        private void Parse(Stream stream)
        {
            using (var reader = new BinaryReader(stream))
            {
                while(ReadMagic(reader))
                {
                    var block = ReadBlock(reader);
                    ProcessBlock(block);
                }
            }
        }

        private bool ReadMagic(BinaryReader reader)
        {
            try
            {
                ini:
                byte b0 = reader.ReadByte();
                if (b0 != 0xF9) goto ini;
                b0 = reader.ReadByte();
                if (b0 != 0xbe) goto ini;
                b0 = reader.ReadByte();
                if (b0 != 0xb4) goto ini;
                b0 = reader.ReadByte();
                if (b0 != 0xd9) goto ini;
                return true;
            }
            catch( EndOfStreamException)
            {
                return false;
            }
        }

        protected virtual void ProcessBlock(Block block)
        {
        }

        private static Block ReadBlock(BinaryReader reader)
        {
            //var magic = reader.ReadUInt32();
            //if(magic != 3652501241) throw new Exception("el numero magico no coincide");

            var block = new Block(reader.BaseStream);
            //block.MagicId = magic;
            block.HeaderLength = reader.ReadUInt32();
            reader.BaseStream.Seek(block.HeaderLength, SeekOrigin.Current);
            return block;
#if false
            block.VersionNumber = reader.ReadUInt32();
            block.PreviousBlockHash = reader.ReadHashAsByteArray();
            block.MerkleRoot = reader.ReadHashAsByteArray();
            block.TimeStamp = reader.ReadUInt32();
            block.Bits = reader.ReadUInt32();
            block.Nonce = reader.ReadUInt32();

            var transactionCount = reader.ReadVarInt();
            block.Transactions = new Transaction[transactionCount];

            for (var ti = 0; ti < transactionCount; ti++)
            {
                var t = new Transaction();
                t.VersionNumber = reader.ReadUInt32();

                var inputCount = reader.ReadVarInt();
                t.Inputs = new Input[inputCount];

                for (var ii = 0; ii < inputCount; ii++)
                {
                    var input = new Input();
                    input.TransactionHash = reader.ReadHashAsByteArray();
                    input.TransactionIndex = reader.ReadUInt32();
                    input.Script = reader.ReadStringAsByteArray();
                    input.SequenceNumber = reader.ReadUInt32();
                    t.Inputs[ii] = input;
                }

                var outputCount = reader.ReadVarInt();
                t.Outputs = new Output[outputCount];

                for (var oi = 0; oi < outputCount; oi++)
                {
                    var output = new Output();
                    output.Value = reader.ReadUInt64();
                    output.Script = reader.ReadStringAsByteArray();
                    t.Outputs[oi] = output;
                }
                block.LockTime = reader.ReadUInt32();
                block.Transactions[ti] = t;
            }

            return block;
#endif
        }
    }
}
