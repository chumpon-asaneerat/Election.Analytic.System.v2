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

#endregion

using PPRP;
using PPRP.Models;

namespace Wpf.Lambda.Sample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constructor

        public MainWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Class

        class Test : NInpc
        {
            private string _name;

            [ExcelColumn("ชื่อ")]
            public string Name
            {
                get { return _name; }
                set 
                {
                    if (_name != value)
                    {
                        _name = value;
                        // raise events
                        Raise(() => this.Name);
                        Raise(() => this.Description);
                    }
                }

            }
            [ExcelColumn("รายละเอียด")]
            public string Description 
            {
                get { return string.Format("Your Name: {0}", _name); }
                set { }
            }
        }

        #endregion

        #region Loaded

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Run();
        }

        #endregion

        #region Private Methods

        private void Run()
        {
            ExcelImport import = new ExcelImport();
            if (null == import) return;
            import.Map();

            Test obj = new Test();
            this.DataContext = obj;

            var attr = obj.GetAttribute<ExcelColumnAttribute, Test, string>((x) => x.Name);
            txtAttrInfo.Text = string.Format("DisplayText: {0}",  attr.DisplayName);
        }

        #endregion
    }
}
