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

        #region Button Handlers

        private void cmdChooseImageFile_Click(object sender, RoutedEventArgs e)
        {
            ChooseImageFile();
        }

        #endregion

        #region Internal Variables

        #endregion

        #region Private Methods

        private void ChooseImageFile()
        {

        }

        #endregion

        #region Public Methods

        #endregion

        #region Public Properties

        #endregion
    }
}
