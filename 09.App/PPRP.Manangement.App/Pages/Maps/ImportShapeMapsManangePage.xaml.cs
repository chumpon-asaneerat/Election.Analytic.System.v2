#region Using

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

using NLib;
using NLib.Services;

using PPRP.Models;
using PPRP.Models.ShapeFiles;
using PPRP.Services;

#endregion

namespace PPRP.Pages
{
    /// <summary>
    /// Interaction logic for ImportShapeMapsManangePage.xaml
    /// </summary>
    public partial class ImportShapeMapsManangePage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public ImportShapeMapsManangePage()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Class

        private class MapFiles : ModelBase
        {
            private string _ShapeFilePath = string.Empty;

            public MapFiles()
            {
                ShapeFilePath = @".\shapefiles";
                ADM0ShapeFile = "thailand_adm0.shp";
                ADM1ShapeFile = "thailand_adm1.shp";
                ADM2ShapeFile = "thailand_adm2.shp";
                ADM3ShapeFile = "thailand_adm3.shp";
            }
            public string ShapeFilePath
            {
                get { return _ShapeFilePath; }
                set
                {
                    if (_ShapeFilePath != value)
                    {
                        _ShapeFilePath = value;
                        RaiseChanged("ShapeFilePath");
                        RaiseChanged("ADM0ShapeFile");
                        RaiseChanged("ADM1ShapeFile");
                        RaiseChanged("ADM2ShapeFile");
                    }
                }
            }

            public string ADM0ShapeFile { get; private set; }
            public string ADM1ShapeFile { get; private set; }
            public string ADM2ShapeFile { get; private set; }
            public string ADM3ShapeFile { get; private set; }

            public string ADM0ShapeFullFileName
            {
                get { return System.IO.Path.Combine(ShapeFilePath, ADM0ShapeFile); }
                set { }
            }
            public string ADM1ShapeFullFileName
            {
                get { return System.IO.Path.Combine(ShapeFilePath, ADM1ShapeFile); }
                set { }
            }
            public string ADM2ShapeFullFileName
            {
                get { return System.IO.Path.Combine(ShapeFilePath, ADM2ShapeFile); }
                set { }
            }
            public string ADM3ShapeFullFileName
            {
                get { return System.IO.Path.Combine(ShapeFilePath, ADM3ShapeFile); }
                set { }
            }
        }

        #endregion

        #region Internal Variables

        private MapFiles _mapFile = new MapFiles();

        #endregion

        #region Button Handlers

        private void cmdHome_Click(object sender, RoutedEventArgs e)
        {
            GotoMainMenuPage();
        }

        private void cmdImportADM0_Click(object sender, RoutedEventArgs e)
        {
            ImportADM0();
        }

        private void cmdImportADM1_Click(object sender, RoutedEventArgs e)
        {
            ImportADM1();
        }

        private void cmdImportADM2_Click(object sender, RoutedEventArgs e)
        {
            ImportADM2();
        }

        private void cmdImportADM3_Click(object sender, RoutedEventArgs e)
        {
            ImportADM3();
        }

        #endregion

        #region Private Methods

        private void GotoMainMenuPage()
        {
            var page = PPRPApp.Pages.MainMenu;
            page.Setup();
            PageContentManager.Instance.Current = page;
        }
        private void ScanFiles()
        {
            if (null == _mapFile) return;
            txtADM0FileName.Text = _mapFile.ADM0ShapeFile;
            cmdImportADM0.IsEnabled = File.Exists(_mapFile.ADM0ShapeFullFileName) ? true : false;
            txtADM1FileName.Text = _mapFile.ADM1ShapeFile;
            cmdImportADM1.IsEnabled = File.Exists(_mapFile.ADM1ShapeFullFileName) ? true : false;
            txtADM2FileName.Text = _mapFile.ADM2ShapeFile;
            cmdImportADM2.IsEnabled = File.Exists(_mapFile.ADM2ShapeFullFileName) ? true : false;
            txtADM3FileName.Text = _mapFile.ADM3ShapeFile;
            cmdImportADM3.IsEnabled = File.Exists(_mapFile.ADM3ShapeFullFileName) ? true : false;
        }

        private void ImportADM0()
        {
            if (null == _mapFile) return;
            var fileName = _mapFile.ADM0ShapeFullFileName;
            if (!File.Exists(fileName)) return;

            // Start Db Sercice
            Task.Run(() =>
            {
                Dispatcher.Invoke(() =>
                {
                    cmdImportADM0.IsEnabled = false;
                    cmdImportADM1.IsEnabled = false;
                    cmdImportADM2.IsEnabled = false;
                    cmdImportADM3.IsEnabled = false;
                });
                using (Shapefile shapefile = new Shapefile(fileName))
                {
                    ShapeMapDbService.Instance.Start();
                    if (ShapeMapDbService.Instance.Connected)
                    {
                        var import = new ShapeFileDbImport();
                        import.Import(shapefile, (shapeNo, shapeCnt, partNo, partCnt, pointNo, pointMax) =>
                        {
                            Dispatcher.Invoke(() =>
                            {
                                string msg = string.Format("Shape:{0:n0}/{1:n0}, Part:{2:n0}/{3:n0}, Point:{4:n0}/{5:n0}",
                                    shapeNo, shapeCnt,
                                    partNo, partCnt,
                                    pointNo, pointMax);
                                txtADM0ProcessPoint.Text = msg;
                            });
                        });
                    }
                    // Shutdown Db Sercice
                    ShapeMapDbService.Instance.Shutdown();
                }
                Dispatcher.Invoke(() =>
                {
                    cmdImportADM0.IsEnabled = true;
                    cmdImportADM1.IsEnabled = true;
                    cmdImportADM2.IsEnabled = true;
                    cmdImportADM3.IsEnabled = true;
                    txtADM0ProcessPoint.Text = string.Empty;
                });
            });
        }

        private void ImportADM1()
        {
            if (null == _mapFile) return;
            var fileName = _mapFile.ADM1ShapeFullFileName;
            if (!File.Exists(fileName)) return;

            // Start Db Sercice
            Task.Run(() =>
            {
                Dispatcher.Invoke(() => 
                { 
                    cmdImportADM0.IsEnabled = false;
                    cmdImportADM1.IsEnabled = false;
                    cmdImportADM2.IsEnabled = false;
                    cmdImportADM3.IsEnabled = false;
                });
                using (Shapefile shapefile = new Shapefile(fileName))
                {
                    ShapeMapDbService.Instance.Start();
                    if (ShapeMapDbService.Instance.Connected)
                    {
                        var import = new ShapeFileDbImport();
                        import.Import(shapefile, (shapeNo, shapeCnt, partNo, partCnt, pointNo, pointMax) =>
                        {
                            Dispatcher.Invoke(() =>
                            {
                                string msg = string.Format("Shape:{0:n0}/{1:n0}, Part:{2:n0}/{3:n0}, Point:{4:n0}/{5:n0}",
                                    shapeNo, shapeCnt,
                                    partNo, partCnt,
                                    pointNo, pointMax);
                                txtADM0ProcessPoint.Text = msg;
                            });
                        });
                    }
                    // Shutdown Db Sercice
                    ShapeMapDbService.Instance.Shutdown();
                }
                Dispatcher.Invoke(() =>
                {
                    cmdImportADM0.IsEnabled = true;
                    cmdImportADM1.IsEnabled = true;
                    cmdImportADM2.IsEnabled = true;
                    cmdImportADM3.IsEnabled = true;
                    txtADM0ProcessPoint.Text = string.Empty;
                });
            });
        }

        private void ImportADM2()
        {

        }

        private void ImportADM3()
        {

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup.
        /// </summary>
        public void Setup()
        {
            ScanFiles();
        }

        #endregion
    }
}
