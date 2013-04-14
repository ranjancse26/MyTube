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
