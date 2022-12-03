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
    /// Interaction logic for MPartyImageEditorControl.xaml
    /// </summary>
    public partial class MPartyImageEditorControl : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public MPartyImageEditorControl()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private MParty _item = null;

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup.
        /// </summary>
        /// <param name="item">The item instance.</param>
        public void Setup(MParty item)
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
        public MParty Item { get { return _item; } }

        #endregion
    }
}
