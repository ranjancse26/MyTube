using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;

/* © Ranjan Dailata [2013]
 * All Rights Reserved
 * No part of this sourcecode or any of its contents may be reproduced, copied, modified or adapted, without the prior written consent of the author, 
 * unless otherwise indicated for stand-alone materials.
*/

namespace MyTube
{
    public partial class Settings : PhoneApplicationPage
    {
        IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
           
        public Settings()
        {
            InitializeComponent();

            int searchResultsCount = int.Parse(settings["SearchResultsCount"].ToString());
            txtSearchResultsCount.Text = searchResultsCount.ToString();
        }

        private void SaveApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            settings["SearchResultsCount"] = txtSearchResultsCount.Text.Trim();
            MessageBox.Show("Saved Successfully!");
        }
    }
}