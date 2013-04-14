using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using MyToolkit.Environment;
using MyToolkit.Media;
using MyToolkit.Multimedia;
using MyToolkit.Networking;

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
	/// <summary>
	/// Add YouTube play logic to Click event (see http://mytoolkit.codeplex.com/wikipage?title=YouTube)
	/// </summary>
	public partial class YouTubeButton : UserControl
	{
		public YouTubeButton()
		{
			InitializeComponent();

			SizeChanged += OnSizeChanged;
			Button.Click += (o, e) => { if (Click != null) Click(this, e); };

			PlayImage.Source = new BitmapImage(new Uri(PhoneApplication.IsDarkTheme ? "../Images/PlayIcon.png" : "../Images/PlayIconLight.png", UriKind.Relative));
		}

		public static readonly DependencyProperty ShowPlayIconProperty =
			DependencyProperty.Register("ShowPlayIcon", typeof (bool), typeof (YouTubeButton), new PropertyMetadata(true, ShowPlayIconPropertyChangedCallback));

		private static void ShowPlayIconPropertyChangedCallback(DependencyObject obj, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{
			var ctrl = (YouTubeButton)obj;
			ctrl.PlayImage.Visibility = ctrl.ShowPlayIcon ? Visibility.Visible : Visibility.Collapsed;
		}

		public bool ShowPlayIcon
		{
			get { return (bool) GetValue(ShowPlayIconProperty); }
			set { SetValue(ShowPlayIconProperty, value); }
		}

		public static readonly DependencyProperty YouTubeIDProperty =
			DependencyProperty.Register("YouTubeID", typeof (string), typeof (YouTubeButton), new PropertyMetadata(default(string), PropertyChangedCallback));

		public string YouTubeID
		{
			get { return (string) GetValue(YouTubeIDProperty); }
			set { SetValue(YouTubeIDProperty, value); }
		}

		public event RoutedEventHandler Click;

		private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{
			var ctrl = (YouTubeButton)dependencyObject;
			ctrl.LoadUri();
		}

		private void OnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
		{
			UpdateImage();
		}

		private void UpdateImage()
		{
			if (ActualWidth > 0.0)
			{
				Image.Width = ActualWidth;
				Image.Height = (ActualWidth / 480) * 360;

				Button.Width = ActualWidth;
				Button.Height = (ActualWidth / 480) * 360; 

				ImageHelper.SetSource(Image, imageUri);
			}
		}

		private Uri imageUri; 
		private void LoadUri()
		{
			if (!String.IsNullOrEmpty(YouTubeID))
			{
				imageUri = YouTube.GetThumbnailUri(YouTubeID);
				UpdateImage();
			}
			else
				imageUri = null; 
		}
	}
}
