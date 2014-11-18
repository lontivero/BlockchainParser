namespace Temosoft.Bitcoin.Blockchain
{
    public class Input
    {
        public byte[] TransactionHash;
        public uint TransactionIndex;
        public byte[] Script;
        public uint SequenceNumber;
    }
}