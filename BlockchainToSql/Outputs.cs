//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BlockchainToSql
{
    using System;
    using System.Collections.Generic;
    
    public partial class Outputs
    {
        public long ID { get; set; }
        public long TransactionID { get; set; }
        public long Value { get; set; }
        public byte[] Script { get; set; }
    
        public virtual Transactions Transactions { get; set; }
    }
}
