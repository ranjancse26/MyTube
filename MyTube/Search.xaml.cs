using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MyToolkit.Multimedia;
using System.ComponentModel;
using Coding4Fun.Phone.Controls;
using System.Diagnostics;
using Microsoft.Devices;
using System.Windows.Data;

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
    public partial class SearchPage : PhoneApplicationPage
    {
        int _pageNumber = 1;
        string _searchTerm = "";
        YouTubePageViewModel _viewModel;
        int _offsetKnob = 7;

        public SearchPage()
        {
            InitializeComponent();
            _viewModel = (YouTubePageViewModel)Resources["viewModel"];
            resultListBox.ItemRealized += resultListBox_ItemRealized;
        }

        void resultListBox_ItemRealized(object sender, ItemRealizationEventArgs e)
        {
            if (!_viewModel.IsLoading && resultListBox.ItemsSource != null && resultListBox.ItemsSource.Count >= _offsetKnob)
            {
                if (e.ItemKind == LongListSelectorItemKind.Item)
                {
                    if ((e.Container.Content as YoutubeItem).Equals(resultListBox.ItemsSource[resultListBox.ItemsSource.Count - _offsetKnob]))
                    {
                        Debug.WriteLine("Searching for {0}", _pageNumber);
                        _viewModel.LoadPage(_searchTerm, _pageNumber++);
                    }
                }
            }
        }

        private void SearchPageLoaded(object sender, RoutedEventArgs e)
        {
            txtSearch.Text = "";
            var progressIndicator = SystemTray.ProgressIndicator;
            if (progressIndicator != null)
            {
                return;
            }

            progressIndicator = new ProgressIndicator();

            SystemTray.SetProgressIndicator(this, progressIndicator);

            Binding binding = new Binding("IsLoading") { Source = _viewModel };
            BindingOperations.SetBinding(progressIndicator, ProgressIndicator.IsVisibleProperty, binding);

            binding = new Binding("IsLoading") { Source = _viewModel };
            BindingOperations.SetBinding(progressIndicator, ProgressIndicator.IsIndeterminateProperty, binding);

            progressIndicator.Text = "Loading...";
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            if (YouTube.CancelPlay())
                e.Cancel = true;
            base.OnBackKeyPress(e);
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

        private void Search(object sender, RoutedEventArgs e)
        {
            _searchTerm = txtSearch.Text.Trim();

            if (String.IsNullOrEmpty(_searchTerm))
            {
                VibrateController.Default.Start(TimeSpan.FromMilliseconds(200));
                return;
            }

            this.Focus();

            _pageNumber = 1;

            _viewModel.LoadPage(_searchTerm, _pageNumber++);
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
    }
}