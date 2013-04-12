using System.ComponentModel;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using MyToolkit.Multimedia;
using Microsoft.Phone.Shell;
using System.Windows.Media;

namespace MyYouTube
{
	public partial class YouTubePage : PhoneApplicationPage
	{
        public YouTubePageViewModel Model { get { return (YouTubePageViewModel)Resources["viewModel"]; } }

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
            Model.IsLoading = false;
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
            if (PhoneApplicationService.Current.State["YoutubeItem"] != null)
            {
                var youtubeItem = PhoneApplicationService.Current.State["YoutubeItem"] as YoutubeItem;
                txtTitle.Text = "Title: " + youtubeItem.Title;
                txtViewCount.Text = "Views: " + youtubeItem.ViewCount;
            }
            youTubePlayer.YouTubeID = this.NavigationContext.QueryString["VideoId"].ToString();
            Model.IsLoading = true;                 
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