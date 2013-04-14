using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
