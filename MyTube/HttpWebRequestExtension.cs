using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace MyYouTube
{
    public static class HttpWebRequestExtensions
    {
#if WINDOWS_PHONE
        private const int DefaultRequestTimeout = 5000;

        public static HttpWebResponse GetResponse(this HttpWebRequest request)
        {
            var dataReady = new AutoResetEvent(false);
            HttpWebResponse response = null;
            var callback = new AsyncCallback(delegate(IAsyncResult asynchronousResult)
            {
                response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
                dataReady.Set();
            });

            request.BeginGetResponse(callback, request);

            if (dataReady.WaitOne(DefaultRequestTimeout))
            {
                return response;
            }

            return null;
        }

        public static Stream GetRequestStream(this HttpWebRequest request)
        {
            var dataReady = new AutoResetEvent(false);
            Stream stream = null;
            var callback = new AsyncCallback(delegate(IAsyncResult asynchronousResult)
            {
                stream = (Stream)request.EndGetRequestStream(asynchronousResult);
                dataReady.Set();
            });

            request.BeginGetRequestStream(callback, request);
            if (!dataReady.WaitOne(DefaultRequestTimeout))
            {
                return null;
            }

            return stream;
        }
#endif
    }  
}
