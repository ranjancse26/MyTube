using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public class ModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
