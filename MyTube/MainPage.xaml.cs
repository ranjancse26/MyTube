using System;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Xml.Linq;
using System.IO;
using System.Collections.ObjectModel;
using MyToolkit.Multimedia;
using System.ComponentModel;
using Coding4Fun.Phone.Controls;
using System.IO.IsolatedStorage;

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

Neither the name of the <ORGANIZATION> nor the names of its contributors may
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
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void MainPageLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
                int count = int.Parse(settings["SearchResultsCount"].ToString());

                var requestUrl = string.Format("http://gdata.youtube.com/feeds/api/standardfeeds/most_viewed?max-results={0}&alt=rss", count.ToString());
                WebClient webClient = new WebClient();
                webClient.AllowReadStreamBuffering = true;
                webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted);
                webClient.DownloadStringAsync(new Uri(requestUrl));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            if (YouTube.CancelPlay())
                e.Cancel = true;
            base.OnBackKeyPress(e);
        }

        void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                var youtubeItemsList = new ObservableCollection<YoutubeItem>();

                TextReader tr = new StringReader(e.Result );
                XDocument xdoc = XDocument.Load(tr);
                XNamespace xmlns = "http://www.w3.org/2005/Atom";
                XNamespace media = "http://search.yahoo.com/mrss/";
                XNamespace gd = "http://schemas.google.com/g/2005";
                XNamespace yt = "http://gdata.youtube.com/schemas/2007";
                string commentsLink = "";
              
                var items = xdoc.Root.Descendants("item").AsEnumerable();
                foreach(var item in items){
                    var grp = item.Descendants(media + "group");
                    var ytNode = item.Descendants(yt + "statistics");
                    var comments = item.Descendants(gd + "comments");
                    if (comments.Count() > 0 )
                        commentsLink = comments.Descendants(gd + "feedLink").Attributes("href").First().Value;

                    youtubeItemsList.Add(new YoutubeItem
                    {
                        Title = grp.Elements(media + "title").First().Value,
                        PlayerUrl = grp.Elements(media + "player").First().Attribute("url").Value ,
                        Description = grp.First().Value,
                        ThumbNailUrl = new Uri(grp.Elements(media + "thumbnail")
                            .Select(u => (string)u.Attribute("url"))
                            .First()),
                        ViewCount = ytNode.Attributes("viewCount").First().Value,
                        CommentsLink = commentsLink
                    });
                }
                listBox.ItemsSource  = youtubeItemsList;
            }
        }

        private void ImageClick(object sender, RoutedEventArgs e)
        {
            YoutubeItem data = (sender as Button).DataContext as YoutubeItem;
            PhoneApplicationService.Current.State["YoutubeItem"] = data;

            string url = data.PlayerUrl;
            int indexOfEqual = url.IndexOf("=");
            var id = url.Substring(indexOfEqual + 1, (url.IndexOf("&") - 1) - indexOfEqual);   
            this.NavigationService.Navigate(new Uri(string.Format("/YouTubePage.xaml?VideoId={0}", id), UriKind.Relative));
        }

        private void AboutApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            AboutPrompt about = new AboutPrompt();
            about.Title = "About";
            about.VersionNumber = "Version 1.0";
            about.Body = new TextBlock { Text = "Designed and Developed by OpenSource Team - BDotNet", TextWrapping = TextWrapping.Wrap };
            about.Show();
        }

        private void SettingsApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri(string.Format("/Settings.xaml"), UriKind.Relative));
        }

        private void SearchApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri(string.Format("/Search.xaml"), UriKind.Relative));
        }
    }
}