using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* © Ranjan Dailata [2013]
 * All Rights Reserved
 * No part of this sourcecode or any of its contents may be reproduced, copied, modified or adapted, without the prior written consent of the author, 
 * unless otherwise indicated for stand-alone materials.
*/

namespace MyTube
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
            get { return "Views: "+ _viewCount; }
            set
            {
                if (_viewCount == value)
                    return;
                _viewCount = value;
                NotifyPropertyChanged("ViewCount");
            }
        }

        private string _commentsLink;
        public string CommentsLink
        {
            get { return _commentsLink; }
            set
            {
                if (_commentsLink == value)
                    return;
                _commentsLink = value;
                NotifyPropertyChanged("CommentsLink");
            }
        }
    }
}
