using System.Collections.Generic;

namespace Temosoft.Bitcoin.Blockchain
{
    public class Transaction
    {
        public uint VersionNumber;
        public Input[] Inputs;
        public Output[] Outputs;
    }
}