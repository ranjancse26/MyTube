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
	public class YouTubePageViewModel : ModelBase 
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

        public YouTubePageViewModel()
        {
             this.YouTubeItemCollection = new ObservableCollection<YoutubeItem>();
             this.IsLoading = false;             
        }

        public ObservableCollection<YoutubeItem> YouTubeItemCollection
        {
            get;
            private set;
        }

        /// <summary>
        /// Load PhoneApplicationPage with YouTube videos based on the RequestUrl 
        /// </summary>
        /// <param name="requestUrl"></param>
        public void LoadPage(string requestUrl)
        {
            try
            {
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

        /// <summary> 
        /// Load PhoneApplicationPage with Youtube videos based on the search criteria and page numbers
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <param name="pageNumber"></param>
        public void LoadPage(string searchTerm, int pageNumber)
        {
            if (pageNumber == 1) this.YouTubeItemCollection.Clear();

            IsLoading = true;
            try
            {
                var requestUrl = string.Format("http://gdata.youtube.com/feeds/api/videos?start-index={0}&alt=rss&q={1}&v=2", pageNumber, searchTerm);
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(String.Format(requestUrl, searchTerm, pageNumber)));
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
                    XNamespace xmlns = "http://www.w3.org/2005/Atom";
                    XNamespace media = "http://search.yahoo.com/mrss/";
                    XNamespace gd = "http://schemas.google.com/g/2005";
                    XNamespace yt = "http://gdata.youtube.com/schemas/2007";

                    string commentsLink = "";

                    var items = xdoc.Root.Descendants("item").AsEnumerable();
                    YoutubeItem youTubeItem;

                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        foreach (var item in items)
                        {
                            var grp = item.Descendants(media + "group");
                            var ytNode = item.Descendants(yt + "statistics");
                            var ytRatingNode = item.Descendants(yt + "rating");
                            var comments = item.Descendants(gd + "comments");
                            if (comments.Count() > 0)
                                commentsLink = comments.Descendants(gd + "feedLink").Attributes("href").First().Value;

                            youTubeItem = new YoutubeItem
                            {
                                Title = grp.Elements(media + "title").First().Value,
                                PlayerUrl = grp.Elements(media + "player").First().Attribute("url").Value,
                                Description = grp.First().Value,
                                ThumbNailUrl = new Uri(grp.Elements(media + "thumbnail").Select(u => (string)u.Attribute("url")).First()),
                                ViewCount = ytNode.Attributes("viewCount").First().Value,                              
                                CommentsLink = commentsLink
                            };

                            if (ytRatingNode.Count() > 0)
                            {
                                youTubeItem.NumberOfLikes = ytRatingNode.Attributes("numLikes").First().Value;
                                youTubeItem.NumberOfDisLikes = ytRatingNode.Attributes("numDislikes").First().Value;
                            }
                            YouTubeItemCollection.Add(youTubeItem);
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
