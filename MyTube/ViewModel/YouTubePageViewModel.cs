using System;
using System.Linq;
using System.Net;
using System.Windows;
using System.Xml.Linq;
using System.IO;
using System.Collections.ObjectModel;
using System.ComponentModel;

/* © Ranjan Dailata [2013]
 * All Rights Reserved
 * No part of this sourcecode or any of its contents may be reproduced, copied, modified or adapted, without the prior written consent of the author, 
 * unless otherwise indicated for stand-alone materials.
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
                var requestUrl = string.Format("http://gdata.youtube.com/feeds/api/videos?start-index={0}&alt=rss&q={1}", pageNumber, searchTerm);
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

                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        foreach (var item in items)
                        {
                            var grp = item.Descendants(media + "group");
                            var ytNode = item.Descendants(yt + "statistics");
                            var comments = item.Descendants(gd + "comments");
                            if (comments.Count() > 0)
                                commentsLink = comments.Descendants(gd + "feedLink").Attributes("href").First().Value;

                            YouTubeItemCollection.Add(new YoutubeItem
                            {
                                Title = grp.Elements(media + "title").First().Value,
                                PlayerUrl = grp.Elements(media + "player").First().Attribute("url").Value,
                                Description = grp.First().Value,
                                ThumbNailUrl = new Uri(grp.Elements(media + "thumbnail").Select(u => (string)u.Attribute("url")).First()),
                                ViewCount = ytNode.Attributes("viewCount").First().Value,
                                CommentsLink = commentsLink
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
