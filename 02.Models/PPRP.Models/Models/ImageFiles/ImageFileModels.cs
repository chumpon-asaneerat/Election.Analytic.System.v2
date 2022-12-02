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
    public class ImageFile
    {
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
            Exist = Directory.Exists(fullFileName);
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
        private ImageFileSource() : base() { }
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="imagePath"></param>
        public ImageFileSource(string imagePath) : base() 
        {
            this.ImagePath = imagePath;

            this.Items = new List<ImageFile>();
        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~ImageFileSource() { }

        #endregion

        #region Public Methods

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
            var exts = new string[] { /*"*.png",*/ "*.jpg" };

            List<string> allFiles = di.GetFiles(searchPattern, SearchOption.AllDirectories)
                .Where(f => /*f.Extension == ".png" ||*/ f.Extension == ".jpg")
                .Select(x => x.FullName)
                .ToList();

            int totalRows = allFiles.Count;

            int maxPages = Convert.ToInt32(Math.Floor((decimal)(totalRows / rowsPerPage)));
            if ((maxPages * totalRows) < totalRows) maxPages++;

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

        #region Static Methods


        /// <summary>
        /// Open Folder Browser to choose selected folder.
        /// </summary>
        /// <returns>Returns ImageFiles instance of selected folder.</returns>
        public static ImageFileSource ChooseFolder(Window owner)
        {
            string targetPath = string.Empty;
            FolderBrowserDialog fd = new FolderBrowserDialog();
            fd.Description = "กรูณาเลือกโฟลเดอร์รูป";
            if (fd.ShowDialog(owner.GetIWin32Window()) == System.Windows.Forms.DialogResult.OK)
            {
                targetPath = fd.SelectedPath;
            }
            fd = null;

            if (string.IsNullOrEmpty(targetPath))
                return null;

            ImageFileSource ret = new ImageFileSource(targetPath);
            return ret;
        }

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
