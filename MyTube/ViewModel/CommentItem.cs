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
    public class CommentItem : ModelBase
    {
        private string _author;
        public string Author
        {
            get { return _author; }
            set
            {
                if (_author == value)
                    return;
                _author = value;
                NotifyPropertyChanged("Author");
            }
        }

        private string _desc;
        public string Description
        {
            get { return _desc; }
            set
            {
                if (_desc == value)
                    return;
                _desc = value;
                NotifyPropertyChanged("Description");
            }
        }     
    }
}
