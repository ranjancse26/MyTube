using System.ComponentModel;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using MyToolkit.Multimedia;

namespace MyYouTube
{
	public partial class YouTubePage : PhoneApplicationPage
	{
        public YouTubePageViewModel Model { get { return (YouTubePageViewModel)Resources["viewModel"]; } }

		public YouTubePage()
		{
            InitializeComponent();
            this.DataContext = this;                    
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
            YouTube.Play(id, true, YouTubeQuality.Quality720P, er =>
            {
                if (er != null)
                    MessageBox.Show(er.Message);
            });  
		}

        private void YoutubePageLoaded(object sender, RoutedEventArgs e)
        {
            youTubePlayer.YouTubeID = this.NavigationContext.QueryString["VideoId"].ToString();
            Model.IsLoading = true;                 
        }
	}
}