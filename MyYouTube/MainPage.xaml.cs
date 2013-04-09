using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;
using System.IO;
using System.Collections.ObjectModel;
using MyToolkit.Multimedia;
using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace MyYouTube
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void MainPageLoaded(object sender, RoutedEventArgs e)
        {
            txtSearch.Text = "";
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

                var items = xdoc.Root.Descendants("item").AsEnumerable();
                foreach(var item in items){
                    var grp = item.Descendants(media + "group");
                    var ytNode = item.Descendants(yt + "statistics");

                    youtubeItemsList.Add(new YoutubeItem
                    {
                        Title = (string)grp.Elements(media + "title").First().Value,
                        PlayerUrl = (string)grp.Elements(media + "player").First().Attribute("url").Value ,
                        Description = grp.First().Value,
                        ThumbNailUrl = new Uri(grp.Elements(media + "thumbnail")
                            .Select(u => (string)u.Attribute("url"))
                            .First()),
                        ViewCount = (string)ytNode.Attributes("viewCount").First().Value 
                    });
                }

                listBox.ItemsSource  = youtubeItemsList;
            }
        }

        private void ImageClick(object sender, RoutedEventArgs e)
        {
            YoutubeItem data = (sender as Button).DataContext as YoutubeItem;
            PhoneApplicationService.Current.State["YoutubeItem"] = data;

            var tag = ((Button)sender).Tag;
            int indexOfEqual = tag.ToString().IndexOf("=");
            var id = tag.ToString().Substring(indexOfEqual + 1, (tag.ToString().IndexOf("&") -1) - indexOfEqual);   
            this.NavigationService.Navigate(new Uri(string.Format("/YouTubePage.xaml?VideoId={0}", id), UriKind.Relative));
        }

        private void Search(object sender, RoutedEventArgs e)
        {
            try
            {
                var requestUrl = "http://gdata.youtube.com/feeds/api/videos?max-results=20&alt=rss&q=" + txtSearch.Text.Trim();
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
    }
}