#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using NLib;
using NLib.Services;

using PPRP.Models;

#endregion

namespace PPRP.Controls
{
    /// <summary>
    /// Interaction logic for MPersonImageEditorControl.xaml
    /// </summary>
    public partial class MPersonImageEditorControl : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public MPersonImageEditorControl()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private MPerson _item = null;

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup.
        /// </summary>
        /// <param name="item">The item instance.</param>
        public void Setup(MPerson item)
        {
            _item = item;
            if (null != _item)
            {

            }
            this.DataContext = _item;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets current item.
        /// </summary>
        public MPerson Item { get { return _item; } }

        #endregion
    }
}
