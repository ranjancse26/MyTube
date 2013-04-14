using System;
using System.Linq;
using System.Net;
using System.Windows;
using System.Xml.Linq;
using System.IO;
using System.Collections.ObjectModel;
using System.ComponentModel;

/*Copyright (c) 2013, Ranjan Dailata
All rights reserved.

Redistribution and use in source and binary forms, with or without 
modification, are permitted provided that the following conditions 
are met:

Redistributions of source code must retain the above copyright notice,
this list of conditions and the following disclaimer.

Redistributions in binary form must reproduce the above copyright notice,
this list of conditions and the following disclaimer in the documentation
and/or other materials provided with the distribution.

Neither the name of the Organization BDotNet nor the names of its contributors may
be used to endorse or promote products derived from this software without
specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE
LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF
THE POSSIBILITY OF SUCH DAMAGE.
*/

namespace MyTube
{
	public class CommentsViewModel : ModelBase 
	{
        private bool _isLoading = false;

        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                _isLoading = value;
                NotifyPropertyChanged("IsLoading");
            }
        }

        public CommentsViewModel()
        {
             this.CommentsItemCollection = new ObservableCollection<CommentItem>();
             this.IsLoading = false;             
        }

        public ObservableCollection<CommentItem> CommentsItemCollection
        {
            get;
            private set;
        }
        
        /// <summary>
        /// Load PhoneApplicationPage with comments based on the Comments Url and Page Number.
        /// </summary>
        /// <param name="commentsUrl"></param>
        /// <param name="pageNumber"></param>
        public void LoadPage(string commentsUrl, int pageNumber)
        {
            if (pageNumber == 1) this.CommentsItemCollection.Clear();

            IsLoading = true;
            try
            {
                var requestUrl = string.Format(string.Format("{0}?start-index={1}&alt=rss", commentsUrl, pageNumber));
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(requestUrl));
                request.BeginGetResponse(new AsyncCallback(ReadCallback), request); 
            }
            catch (Exception ex)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    MessageBox.Show("Network error occured " + ex.Message);
                });
            }
        }

        private void ReadCallback(IAsyncResult asynchronousResult)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    XDocument xdoc = XDocument.Load(reader);
                    var items = xdoc.Root.Descendants("item").AsEnumerable();

                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        foreach (var item in items)
                        {                         
                            CommentsItemCollection.Add(new CommentItem 
                            {
                                 Description = item.Descendants("description").First().Value,
                                 Author  = "Author: "+ item.Descendants("author").First().Value,
                            });
                        }
                        IsLoading = false;
                    });
                }
            }
            catch (Exception e)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    MessageBox.Show("Network error occured " + e.Message);
                });
            }
        }        
	}
}
