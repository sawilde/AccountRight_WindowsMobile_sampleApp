using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using SharpCompress.Compressor;
using SharpCompress.Compressor.Deflate;

namespace MYOB.Sample.Communication
{
    public abstract class BaseRequestHandler
    {
        protected class RequestContext<T, TR>
        {
            public HttpWebRequest Request { get; set; }
            public T Body { get; set; }
            public Action<TR> OnSuccess { get; set; }
            public Action OnError { get; set; }
        }

        protected static void HandleResponseCallback<T, TReq, TResp>(IAsyncResult asynchronousResult)
            where T : RequestContext<TReq, TResp>
            where TResp : class
        {
            var requestData = (T)asynchronousResult.AsyncState;
            var request = requestData.Request;
            try
            {
                var response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    if (response.Headers["Content-Encoding"] != null && response.Headers["Content-Encoding"].Contains("gzip"))
                    {
                        var entity = ExtractCompressedEntity<TResp>(response);
                        requestData.OnSuccess(entity);
                    }
                    else
                    {
                        var entity = ExtractEntity<TResp>(response);
                        requestData.OnSuccess(entity);
                    }
                }
                else
                {
                    requestData.OnError();
                }
            }
            catch (Exception ex)
            {
                requestData.OnError();
            }
        }

        private static TResp ExtractEntity<TResp>(HttpWebResponse response)
        {
            var responseStream = response.GetResponseStream();
            using (var reader = new StreamReader(responseStream))
            {
                var data = reader.ReadToEnd();
                var serializer = new DataContractJsonSerializer(typeof(TResp));
                return (TResp)serializer.ReadObject(new MemoryStream(Encoding.UTF8.GetBytes(data)));
            }
        }

        private static TResp ExtractCompressedEntity<TResp>(HttpWebResponse response)
        {
            var responseStream = response.GetResponseStream();
            using (var decompress = new GZipStream(responseStream, CompressionMode.Decompress))
            {
                var serializer = new DataContractJsonSerializer(typeof(TResp));
                return (TResp)serializer.ReadObject(decompress);
            }
        }
    }
}