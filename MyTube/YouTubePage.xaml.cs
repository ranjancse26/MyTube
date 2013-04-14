using System.ComponentModel;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using MyToolkit.Multimedia;
using Microsoft.Phone.Shell;
using System.Windows.Media;
using System.Diagnostics;

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