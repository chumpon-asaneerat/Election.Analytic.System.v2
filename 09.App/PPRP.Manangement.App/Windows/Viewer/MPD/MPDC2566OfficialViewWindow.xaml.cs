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
    /// Interaction logic for MPDC2566OfficialViewWindow.xaml
    /// </summary>
    public partial class MPDC2566OfficialViewWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MPDC2566OfficialViewWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private MPDCOfficial _item = null;

        #endregion

        #region Button Handlers

        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        #endregion

        #region Public Methods

        public void Setup(MPDCOfficial item)
        {
            _item = item;
            this.DataContext = _item;

            if (null != _item)
            {
                // set load person image.
                var person = MPerson.Get(_item.PersonId).Value();
                imgPreson.DataContext = person;
            }
            else
            {
                // set default image by assigned new person instance.
                imgPreson.DataContext = new MPerson();
            }
        }

        #endregion
    }
}
