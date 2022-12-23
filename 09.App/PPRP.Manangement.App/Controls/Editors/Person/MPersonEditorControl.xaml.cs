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
    /// Interaction logic for MPersonEditorControl.xaml
    /// </summary>
    public partial class MPersonEditorControl : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public MPersonEditorControl()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private MPerson _item;

        #endregion

        #region Private Methods

        #endregion

        #region Public Methods

        public void Setup(MPerson value)
        {
            _item = value;
            // set data context
            DataContext = _item;

            ctrlPrefix.Setup(_item);
            ctrlFirstName.Setup(_item);
            ctrlLastName.Setup(_item);
            ctrlDOB.Setup(_item);
            ctrlGenderId.Setup(_item);
            ctrlEducationId.Setup(_item);
            ctrlOccupationId.Setup(_item);
            ctrlRemark.Setup(_item);
            ctrlPersonImage.Setup(_item);
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
