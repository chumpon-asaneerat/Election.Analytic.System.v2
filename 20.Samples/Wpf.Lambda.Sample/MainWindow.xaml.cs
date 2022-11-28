﻿#region Using

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

        class Test : NInpc, IExcelModel
        {
            private string _fullName;
            private int _age;
            private DateTime _DOB;

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

            [ExcelColumn("อายุ", ExcelColumnMode.Import)]
            public int Age
            {
                get { return _age; }
                set
                {
                    if (_age != value)
                    {
                        _age = value;
                        // raise events
                        Raise(() => this.Age);
                        Raise(() => this.Description);
                    }
                }
            }

            [ExcelColumn("วันเดือนปีเกิด", ExcelColumnMode.Export)]
            public DateTime DOB
            {
                get { return _DOB; }
                set
                {
                    if (_DOB != value)
                    {
                        _DOB = value;
                        // raise events
                        Raise(() => this.Age);
                        Raise(() => this.Description);
                    }
                }
            }

            //[ExcelColumn("รายละเอียด")]
            public string Description 
            {
                get { return string.Format("Your Name: {0}, Age: {1}", _fullName, _age); }
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
            ListViewSample();
        }

        private void cmdResetMapProperty_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var ctx = (null != button) ? button.DataContext : null;
            var map = (null != ctx) ? ctx as NExcelMapProperty : null;
            if (null != map)
            {
                map.SelectedColumn = null; // reset
            }
        }

        private void ListViewSample()
        {
            model = new ExcelModel<Test>();

            // Simulate load Excel Columns from worksheet.
            var letters = new string[] 
            { 
                "A", "B", "C", "D", "E", "F", "G", "H", "I", "J",
                "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T",
                "U", "V", "W", "X", "Y", "Z"
            };

            int iMax = 20;

            for (int iCol = 0; iCol < iMax; ++iCol)
            {
                model.Columns.Add(new NExcelColumn() 
                { 
                    ColumnName = "Col" + iCol.ToString(), 
                    ColumnLetter = letters[iCol],
                    ColumnIndex = iCol + 1 
                });
            }

            //model.Mode = ExcelColumnMode.Import;
            model.Mode = ExcelColumnMode.Export;
            model.MapColumns(); // call for update colimn mappings

            // bind to list view
            lv.DataContext = model;
            lv.ItemsSource = model.Mappings;

            /*
            for (int iCnt = 0; iCnt < 10; ++iCnt)
            {
                model.Items.Add(new Test() { FullName = "ทดสอบ " + iCnt.ToString() });
            }
            */
        }

        #endregion
    }
}
