using System;
using System.IO;
using Temosoft.Bitcoin.Blockchain;

namespace BlockchainWalker
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
        private long _blks;

        protected override void ProcessBlock(Block block)
        {
            if (_blks++%(10*1000) == 0)
            {
                Console.WriteLine(_blks);
                Console.WriteLine(block.TimeStamp);
            }
        }
    }
}
