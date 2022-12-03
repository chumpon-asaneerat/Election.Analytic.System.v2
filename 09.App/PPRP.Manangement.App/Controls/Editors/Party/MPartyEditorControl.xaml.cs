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
    /// Interaction logic for MPartyEditorControl.xaml
    /// </summary>
    public partial class MPartyEditorControl : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public MPartyEditorControl()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private MParty _item;

        #endregion

        #region Private Methods

        #endregion

        #region Public Methods

        public void Setup(MParty value)
        {
            _item = value;
            // set data context
            DataContext = _item;
            ctrlPartyName.Setup(_item);
            ctrlPartyImage.Setup(_item);
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
