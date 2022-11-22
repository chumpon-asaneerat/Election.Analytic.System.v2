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
using PPRP.Excel;
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

        class Test : NInpc, IExcelModel
        {
            private string _fullName;

            //[ExcelColumn(DisplayName = "ชื่อ")]
            [ExcelColumn("ชื่อ")]
            public string FullName
            {
                get { return _fullName; }
                set 
                {
                    if (_fullName != value)
                    {
                        _fullName = value;
                        // raise events
                        Raise(() => this.FullName);
                        Raise(() => this.Description);
                    }
                }

            }
            [ExcelColumn("รายละเอียด")]
            public string Description 
            {
                get { return string.Format("Your Name: {0}", _fullName); }
                set { }
            }
        }

        private ExcelModel<Test> model;

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
            ItemsControlSample();
            ExcalColumnSample();
        }


        private void ItemsControlSample()
        {
            model = new ExcelModel<Test>();
            for (int i = 0; i < 10; ++i) 
            {
                model.Items.Add(new Test() { FullName = "ทดสอบ " + i.ToString() });
            }
            // bind to items control
            list.DataContext = model;
            list.ItemsSource = model.Items;
        }

        private void ExcalColumnSample()
        {
            ExcelImport import = new ExcelImport();
            if (null == import) return;
            import.Map();

            Test obj = new Test();
            this.DataContext = obj;

            /*
            var attr = obj.GetAttribute<ExcelColumnAttribute, Test, string>((x) => x.Name);
            txtAttrInfo.Text = string.Format("DisplayText: {0}",  attr.DisplayName);
            */
            /*
            var attr = obj.GetPropertyAttributes(x => x.FullName).GetFirst<ExcelColumnAttribute>();
            txtAttrInfo.Text = string.Format("DisplayText: {0}, PropertyName: {1}", attr.HeaderText, attr.PropertyName);
            */
            /*
            var map = new LambdaMap<Test>();
            txtAttrInfo.Text = string.Format("DisplayText: {0}", map.PropertyName((x) => x.FullName));
            */

            var map = new ExcelColumnMap<Test>();
            var props = map.GetProperties<ExcelColumnAttribute>();
            foreach (var prop in props)
            {
                Console.WriteLine("Property: {0}", prop.Name);
            }
        }

        #endregion
    }
}
