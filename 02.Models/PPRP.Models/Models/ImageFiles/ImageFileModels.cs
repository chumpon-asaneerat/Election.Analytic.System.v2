#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;

using NLib;
using NLib.Reflection;
using OfficeOpenXml;

#endregion

namespace PPRP.Models
{
    #region ImageFile

    /// <summary>
    /// The ImageFile class.
    /// </summary>
    public class ImageFile : NInpc
    {
        #region Internal Variables

        private byte[] _data = null;
        private ImageSource _imgSrc = null;
        private bool _loaded = false;

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        private ImageFile() : base() { }
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="fullFileName"></param>
        public ImageFile(string fullFileName) : base()
        {
            Exist = File.Exists(fullFileName);
            if (Exist)
            {
                FullFileName = fullFileName;
                PathName = Path.GetDirectoryName(fullFileName);
                FileName = Path.GetFileName(fullFileName);
                FileNameOnly = Path.GetFileNameWithoutExtension(fullFileName);
                Extension = Path.GetExtension(fullFileName);
            }
        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~ImageFile()
        {
            FreeImage();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets Image Data buffers.
        /// </summary>
        /// <returns>Returns image data in byte array.</returns>
        public byte[] GetImageData()
        {
            byte[] rets = null;
            if (File.Exists(FullFileName))
            {
                // Set to property for notify changed (careful to prevent stack overflow)
                rets = ByteUtils.GetFileBuffer(FullFileName);
            }
            return rets;
        }

        /// <summary>
        /// Load Image.
        /// </summary>
        public void LoadImage()
        {
            if (_loaded)
                return;
            _loaded = true; // careful to update flag immediately to prevent stack overflow.

            lock (this)
            {
                // set field
                _data = null;
                _imgSrc = null;

                Data = GetImageData();
            }
        }
        /// <summary>
        /// Free Image.
        /// </summary>
        public void FreeImage()
        {
            lock (this)
            {
                _data = null;
                _imgSrc = null;
                GC.Collect(0);
                GC.SuppressFinalize(this);
                _loaded = false;
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets is directory is exists.
        /// </summary>
        public bool Exist { get; private set; }
        /// <summary>
        /// Gets Path Name.
        /// </summary>
        public string PathName { get; private set; }
        /// <summary>
        /// Gets Full FileName.
        /// </summary>
        public string FullFileName { get; private set; }
        /// <summary>
        /// Gets FileName with extension.
        /// </summary>
        public string FileName { get; private set; }
        /// <summary>
        /// Gets File Name only.
        /// </summary>
        public string FileNameOnly { get; private set; }
        /// <summary>
        /// Gets File Extension.
        /// </summary>
        public string Extension { get; private set; }
        /// <summary>
        /// Gets or sets Image Data buffers.
        /// </summary>
        public byte[] Data
        {
            get 
            {
                if (!_loaded) LoadImage();
                return _data; 
            }
            set
            {
                _data = value;
                Raise(() => this.Data);

                System.Windows.Application.Current.MainWindow.Dispatcher.Invoke(() =>
                {
                    _imgSrc = ByteUtils.GetImageSource(_data);
                    Raise(() => this.Image);
                });
            }
        }
        /// <summary>
        /// Gets ImageSource.
        /// </summary>
        public ImageSource Image
        {
            get 
            {
                if (!_loaded) LoadImage();
                return _imgSrc; 
            }
            set { }
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Open Image File.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="initDir"></param>
        /// <returns>Returns ImageFile instance if image file is selected.</returns>
        public static ImageFile OpenFile(string title = "กรุณาเลือก image file ที่ต้องการอ่านข้อมูล",
            string initDir = null)
        {
            return OpenFile(null, title, initDir);
        }
        /// <summary>
        /// Open Image File.
        /// </summary>
        /// <param name="owner">The owner window.</param>
        /// <param name="title"></param>
        /// <param name="initDir"></param>
        /// <returns>Returns ImageFile instance if image file is selected.</returns>
        public static ImageFile OpenFile(Window owner,
            string title = "กรุณาเลือก image file ที่ต้องการอ่านข้อมูล",
            string initDir = null)
        {
            ImageFile inst = null;

            // setup dialog options
            var od = new Microsoft.Win32.OpenFileDialog();
            od.Multiselect = false;
            od.InitialDirectory = initDir;
            od.Title = string.IsNullOrEmpty(title) ? "กรุณาเลือก image file ที่ต้องการอ่านข้อมูล" : title;
            od.Filter = "Excel Files(*.xls, *.xlsx)|*.xls;*.xlsx";

            var ret = od.ShowDialog(owner) == true;
            if (ret)
            {
                // assigned to FileName
                var fileName = od.FileName;
                if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName))
                {
                    inst = new ImageFile(fileName);
                }
            }

            return inst;
        }

        #endregion
    }

    #endregion

    #region ImageFileSource

    /// <summary>
    /// The ImageFileSource class.
    /// </summary>
    public class ImageFileSource
    {
        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public ImageFileSource() : base() 
        {
            this.Items = new List<ImageFile>();
        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~ImageFileSource() { }

        #endregion

        #region Public Methods


        /// <summary>
        /// Open Folder.
        /// </summary>
        /// <param name="owner">The owner window.</param>
        /// <returns></returns>
        public bool OpenFolder(Window owner)
        {
            string targetPath = string.Empty;
            FolderBrowserDialog fd = new FolderBrowserDialog();
            fd.Description = "กรูณาเลือกโฟลเดอร์รูป";
            if (fd.ShowDialog(owner.GetIWin32Window()) == System.Windows.Forms.DialogResult.OK)
            {
                targetPath = fd.SelectedPath;
            }
            fd = null;

            bool ret = !string.IsNullOrEmpty(targetPath);
            if (ret)
            {
                this.ImagePath = targetPath; // set target path;
            }

            return ret;
        }
        /// <summary>
        /// Gets all items (files).
        /// </summary>
        /// <returns>Returns list of image file.</returns>
        public List<ImageFile> GetAllItems()
        {
            var rets = new List<ImageFile>();

            if (!Directory.Exists(ImagePath))
                return rets;

            DirectoryInfo di = new DirectoryInfo(ImagePath);

            string searchPattern = "*.*";
            var exts = new string[] { /*"*.png",*/ "*.jpg", "*.jpeg" };

            List<string> allFiles = di.GetFiles(searchPattern, SearchOption.AllDirectories)
                .Where(f => /*f.Extension == ".png" ||*/ f.Extension == ".jpg" || f.Extension == ".jpeg")
                .Select(x => x.FullName)
                .ToList();

            if (null != allFiles && allFiles.Count > 0)
            {
                allFiles.ForEach(file =>
                {
                    var imgFile = new ImageFile(file);
                    if (imgFile.Exist)
                    {
                        rets.Add(imgFile);
                    }
                });
            }

            return rets;
        }
        /// <summary>
        /// Load Items (files) on specificed PageNo with limit RowPerPage.
        /// </summary>
        /// <param name="pageNo"></param>
        /// <param name="rowsPerPage"></param>
        public void LoadItems(int pageNo = 1, int rowsPerPage = 40)
        {
            if (null != Items)
            {
                Items.Clear();
            }
            if (!Directory.Exists(ImagePath))
                return;

            DirectoryInfo di = new DirectoryInfo(ImagePath);

            string searchPattern = "*.*";
            var exts = new string[] { /*"*.png",*/ "*.jpg", "*.jpeg" };

            List<string> allFiles = di.GetFiles(searchPattern, SearchOption.AllDirectories)
                .Where(f => /*f.Extension == ".png" ||*/ f.Extension == ".jpg" || f.Extension == ".jpeg")
                .Select(x => x.FullName)
                .ToList();

            int totalRows = allFiles.Count;

            int maxPages = Convert.ToInt32(Math.Floor((decimal)(totalRows / rowsPerPage)));
            if ((maxPages * rowsPerPage) < totalRows) maxPages++;

            TotalRecords = totalRows;
            MaxPages = maxPages;
            RowsPerPage = rowsPerPage;
            PageNo = pageNo;

            var files = allFiles.Skip((pageNo - 1) * rowsPerPage).Take(rowsPerPage).ToList();

            if (null != files && files.Count > 0)
            {
                files.ForEach(file =>
                {
                    var imgFile = new ImageFile(file);
                    if (imgFile.Exist)
                    {
                        Items.Add(imgFile);
                    }
                });
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets current image path.
        /// </summary>
        public string ImagePath { get; protected set; }
        /// <summary>
        /// Gets or sets total record or file count.
        /// </summary>
        public int TotalRecords { get; internal set; }
        /// <summary>
        /// Gets or sets rows (or files) per page.
        /// </summary>
        public int RowsPerPage { get; internal set; }
        /// <summary>
        /// Gets or sets Max Pages.
        /// </summary>
        public int MaxPages { get; internal set; }
        /// <summary>
        /// Gets or sets current page number.
        /// </summary>
        public int PageNo { get; internal set; }

        /// <summary>
        /// Gets Items on current page.
        /// </summary>
        public List<ImageFile> Items { get; private set; }

        #endregion
    }

    #endregion

    #region WpfExtensionMethods class

    /// <summary>
    /// Wpf Extension Methods.
    /// </summary>
    internal static class WpfExtensionMethods
    {
        public static IWin32Window GetIWin32Window(this Visual visual)
        {
            var source = PresentationSource.FromVisual(visual) as System.Windows.Interop.HwndSource;
            IWin32Window win = new OldWindow(source.Handle);
            return win;
        }

        private class OldWindow : IWin32Window
        {
            private readonly IntPtr _handle;

            public OldWindow(IntPtr handle)
            {
                _handle = handle;
            }

            #region IWin32Window Members

            IntPtr IWin32Window.Handle
            {
                get { return _handle; }
            }

            #endregion
        }
    }

    #endregion
}
