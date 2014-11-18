using System;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;

namespace Temosoft.Bitcoin.Blockchain
{
    public abstract class Parser
    {
        public void Parse(string[] filesPath)
        {
            foreach (var filePath in filesPath)
            {
                ParseFile(filePath);
            }
        }

        private void ParseFile(string filePath)
        {
            Console.WriteLine("Processing {0}", Path.GetFileName(filePath));
            using (var mmf = MemoryMappedFile.CreateFromFile(filePath, FileMode.Open, "Blockchain", 0, MemoryMappedFileAccess.Read))
            {
                using (var stream = mmf.CreateViewStream(0, 0, MemoryMappedFileAccess.Read))
                {
                    using (var reader = new BinaryReader(stream))
                    {
                        do
                        {
                            var block = ReadBlock(reader);
                            ProcessBlock(block);
                            while (reader.PeekByte() == 0x00) 
                                reader.ReadByte();
                        } while (reader.PeekByte() != 0xff);
                    }
                }
            }
        }

        protected virtual void ProcessBlock(Block block)
        {
        }

        private static Block ReadBlock(BinaryReader reader)
        {
            var block = new Block();
            block.MagicId = reader.ReadUInt32();
            block.HeaderLength = reader.ReadUInt32();
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
        }
    }
}
