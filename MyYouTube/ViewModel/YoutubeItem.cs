using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyYouTube
{
    public class YoutubeItem : ModelBase
    {
        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                if (_title == value)
                    return;
                _title = value;
                NotifyPropertyChanged("Title");
            }
        }
        private string _playerUrl;
        public string PlayerUrl
        {
            get { return _playerUrl; }
            set
            {
                if (_playerUrl == value)
                    return;
                _playerUrl = value;
                NotifyPropertyChanged("PlayerUrl");
            }
        }
        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                if (_description == value)
                    return;
                _description = value;
                NotifyPropertyChanged("Description");
            }
        }
        private Uri _thumbnailUrl;
        public Uri ThumbNailUrl
        {
            get { return _thumbnailUrl; }
            set
            {
                if (_thumbnailUrl == value)
                    return;
                _thumbnailUrl = value;
                NotifyPropertyChanged("ThumbNailUrl");
            }
        }

        private string _viewCount;
        public string ViewCount
        {
            get { return _viewCount; }
            set
            {
                if (_viewCount == value)
                    return;
                _viewCount = value;
                NotifyPropertyChanged("ViewCount");
            }
        }
    }
}
