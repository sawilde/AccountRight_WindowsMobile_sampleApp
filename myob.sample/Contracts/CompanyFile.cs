using System;
using System.Runtime.Serialization;

namespace MYOB.Sample.Contracts
{
    /// <summary>
    /// Full company file contract can be found at 
    /// http://developer.myob.com/docs/read/endpoint_docs/accountright/The_Company_Files
    /// </summary>
    [DataContract]
    public class CompanyFile
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string ProductVersion { get; set; }

        [DataMember]
        public Uri Uri { get; set; }
    }
}
