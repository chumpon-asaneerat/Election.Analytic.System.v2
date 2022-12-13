#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Forms;

using NLib;
using NLib.Reflection;
using NLib.Services;

using PPRP.Models;

#endregion

namespace PPRP.Windows
{
    /// <summary>
    /// Interaction logic for MPD2562ViewWindow.xaml
    /// </summary>
    public partial class MPD2562ViewWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MPD2562ViewWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private MPDVoteSummary _item = null;

        #endregion

        #region Button Handlers
        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        #endregion

        #region Public Methods

        public void Setup(MPDVoteSummary item)
        {
            _item = item;
            this.DataContext = _item;

            if (null != _item)
            {
                // set load person image.
                Defaults.RunInBackground(() =>
                {
                    var person = MPerson.Get(_item.PersonId).Value();

                    if (null != person)
                    {
                        var imgSrc = ByteUtils.GetImageSource(person.Data);
                        imgPreson.Source = imgSrc;
                    }
                    else
                    {
                        imgPreson.Source = Defaults.Person;
                    }
                });
            }
            else
            {
                // set default image.
                Defaults.RunInBackground(() => {
                    imgPreson.Source = Defaults.Person;
                });
                
            }
        }

        #endregion
    }
}
