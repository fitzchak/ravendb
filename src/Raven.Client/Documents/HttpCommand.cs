using System;
using System.Collections.Specialized;
using System.IO;
using System.Threading.Tasks;

namespace Raven.Client.Documents
{
    public class HttpCommand
    {
        public string Method;
        public string Url;
        public NameValueCollection RequestHeaders;

        public void Write(Stream stream)
        {
            throw new NotImplementedException();
        }

        public Task WriteAysnc(Stream stream)
        {
            throw new NotImplementedException();
        }

        public int ResponseCode;
        public NameValueCollection ResponseHeaders;
        public Stream ResposeBody;
    }
}