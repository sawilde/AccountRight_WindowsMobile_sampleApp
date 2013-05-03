using System;
using System.Runtime.Serialization;

namespace MYOB.Sample.Contracts
{
    [DataContract]
    public class PagedCollection<T>
    {
        [DataMember]
        public T[] Items { get; set; }

        [DataMember]
        public int Count { get; set; }
    }

    /// <summary>
    /// Full account contract can be found at 
    /// http://developer.myob.com/docs/read/endpoint_docs/accountright/Account
    /// </summary>
    [DataContract]
    public class Account
    {
        [DataMember(Name = "AccountName")]
        public string Name { get; set; }

        [DataMember]
        public decimal CurrentAccountBalance { get; set; }

        [DataMember]
        public bool IsHeader { get; set; }

        [DataMember]
        public Uri Uri { get; set; }
    }
}