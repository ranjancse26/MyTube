using System.ComponentModel;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using MyToolkit.Multimedia;
using Microsoft.Phone.Shell;
using System.Windows.Media;
using System.Diagnostics;

/* © Ranjan Dailata [2013]
 * All Rights Reserved
 * No part of this sourcecode or any of its contents may be reproduced, copied, modified or adapted, without the prior written consent of the author, 
 * unless otherwise indicated for stand-alone materials.
*/

namespace MyTube
{
	public partial class YouTubePage : PhoneApplicationPage
	{
        int _pageNumber = 1;
        CommentsViewModel _viewModel;
        int _offsetKnob = 7;
        YoutubeItem youtubeItem = null;

        public YouTubeQuality VideoQuality
        {
            get;
            set;
        }

		public YouTubePage()
		{
            InitializeComponent();
            this.DataContext = this;
            btnLowQuality.Background = new SolidColorBrush(Colors.Red);
            VideoQuality = YouTubeQuality.Quality480P;

            _viewModel = (CommentsViewModel)Resources["commentsViewModel"];
            resultListBox.ItemRealized += resultListBox_ItemRealized;

            if (PhoneApplicationService.Current.State["YoutubeItem"] != null)
            {
                youtubeItem = PhoneApplicationService.Current.State["YoutubeItem"] as YoutubeItem;
                _pageNumber = 1;
                _viewModel.LoadPage(youtubeItem.CommentsLink, _pageNumber++);
            }
		}

        void resultListBox_ItemRealized(object sender, ItemRealizationEventArgs e)
        {
            if (!_viewModel.IsLoading && resultListBox.ItemsSource != null && resultListBox.ItemsSource.Count >= _offsetKnob)
            {
                if (e.ItemKind == LongListSelectorItemKind.Item)
                {
                    if ((e.Container.Content as CommentItem).Equals(resultListBox.ItemsSource[resultListBox.ItemsSource.Count - _offsetKnob]))
                    {
                        Debug.WriteLine("Searching for {0}", _pageNumber);
                        _viewModel.LoadPage(youtubeItem.CommentsLink, _pageNumber++);
                    }
                }
            }
        }
               
		protected override void OnBackKeyPress(CancelEventArgs e)
		{
			if (YouTube.CancelPlay()) 
				e.Cancel = true;
			base.OnBackKeyPress(e);
		}

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            YouTube.CancelPlay();
        }

		private void OnPlay(object sender, RoutedEventArgs args)
		{
            var id = this.NavigationContext.QueryString["VideoId"].ToString();
            YouTube.Play(id, true, VideoQuality, er =>
            {
                if (er != null)
                    MessageBox.Show(er.Message);
            });  
		}

        private void YoutubePageLoaded(object sender, RoutedEventArgs e)
        {
            if (youtubeItem != null)
            {
                txtTitle.Text = "Title: " + youtubeItem.Title;
                txtViewCount.Text = youtubeItem.ViewCount;
            }
            youTubePlayer.YouTubeID = this.NavigationContext.QueryString["VideoId"].ToString();
        }

        private void btnLowQuality_Click(object sender, RoutedEventArgs e)
        {
            ResetButtonBackGroundColor();
            btnLowQuality.Background = new SolidColorBrush(Colors.Red);
            VideoQuality = YouTubeQuality.Quality480P;
        }

        private void ResetButtonBackGroundColor()
        {
            btnHighQuality.Background = new SolidColorBrush(Colors.Black);
            btnHighDefQuality.Background = new SolidColorBrush(Colors.Black);
            btnLowQuality.Background = new SolidColorBrush(Colors.Black);
        }

        private void btnHighQuality_Click(object sender, RoutedEventArgs e)
        {
            ResetButtonBackGroundColor();
            btnHighQuality.Background = new SolidColorBrush(Colors.Red);
            VideoQuality = YouTubeQuality.Quality720P;
        }

        private void btnHighDefQuality_Click(object sender, RoutedEventArgs e)
        {
            ResetButtonBackGroundColor();
            btnHighDefQuality.Background = new SolidColorBrush(Colors.Red);
            VideoQuality = YouTubeQuality.Quality1080P;
        }
	}
}