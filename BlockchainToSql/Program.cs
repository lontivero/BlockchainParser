using System;
using System.Collections.Generic;
using System.IO;
using Temosoft.Bitcoin.Blockchain;

namespace BlockchainToSql
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var blocksFolder = Environment.ExpandEnvironmentVariables(@"%AppData%\Bitcoin\blocks");
            var filesPath = Directory.GetFiles(blocksFolder, "blk*.dat", SearchOption.TopDirectoryOnly);
            var parser = new BlockchainProcessor();
            parser.Parse(filesPath);
        }
    }

    internal class BlockchainProcessor : BlockchainParser
    {
        private static DateTime First1970 = new DateTime(1970, 1, 1);
        private readonly List<blocks> _blocks = new List<blocks>();
        private long _blockId = 0;
        private long _txId = 0;
        private long _inId = 0;
        private long _outId = 0;
        private long _records = 0;

        private DateTime _begin = DateTime.UtcNow;

        protected override void ProcessBlock(Block block)
        {
            if(_blocks.Count == 10)
            {
                using(var dbContext = new blockchainEntities() )
                {
                    dbContext.Configuration.AutoDetectChangesEnabled = false;
                    dbContext.Configuration.ValidateOnSaveEnabled = false;
                    
                    foreach (var b in _blocks)
                    {
                        dbContext.blocks.Add(b);
                    }

                    var d = DateTime.UtcNow;
                    dbContext.SaveChanges();
                    dbContext.Dispose();
                    Console.Write("{0,8:F0} milliseconds  ->", (DateTime.UtcNow - d).TotalMilliseconds );

                    _blocks.Clear();
                    var segs = (DateTime.UtcNow - _begin).TotalSeconds;
                    Console.WriteLine("{0,6} records -> 10 blocks in {1,6:F3} segs = {2,8:F4} seg/records", _records, segs, segs / _records);
                    _records = 0;
                    _begin = DateTime.UtcNow;
                }
            }
            else
            {
                var blockEntity = new blocks {
                    Length = (int) block.HeaderLength,
                    LockTime = block.LockTime,
                    MerkleRoot = block.MerkleRoot,
                    Nonce = block.Nonce,
                    PreviousBlockHash = block.PreviousBlockHash,
                    TargetDifficulty = block.TimeStamp,
                    TimeStamp = First1970.AddSeconds(block.TimeStamp)
                };
                _records++;

                foreach (var transaction in block.Transactions)
                {
                    var transactionEntity = new Transactions();
                    transactionEntity.Version = transaction.VersionNumber;
                    _records++;

                    foreach (var input in transaction.Inputs)
                    {
                        transactionEntity.Inputs.Add(new Inputs {
                            Script = input.Script,
                            SequenceNumber = input.SequenceNumber,
                            TransactionHash = input.TransactionHash,
                            TransactionIndex = input.TransactionIndex
                        });
                        _records++;
                    }

                    foreach (var output in transaction.Outputs)
                    {
                        transactionEntity.Outputs.Add(new Outputs {
                            Script = output.Script,
                            Value = (long) output.Value
                        });
                        _records++;
                    }
                    blockEntity.Transactions.Add(transactionEntity);
                }
                _blocks.Add(blockEntity);
            }
        }
    }
}
