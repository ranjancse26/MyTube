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

/* © Ranjan Dailata [2013]
 * All Rights Reserved
 * No part of this sourcecode or any of its contents may be reproduced, copied, modified or adapted, without the prior written consent of the author, 
 * unless otherwise indicated for stand-alone materials.
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