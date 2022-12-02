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
        public ImageFileSource() : base() 
        {
            this.Items = new List<ImageFile>();
        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~ImageFileSource() { }

        #endregion


        #region Public Properties

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


    #region ImageFiles

    /// <summary>
    /// The ImageFiles class.
    /// </summary>
    public class ImageFiles
    {
        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public ImageFiles() : base() { }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~ImageFiles() { }

        #endregion

        #region Public Methods

        public void Setup(string path)
        {
            ImagePath = path;
            Directory.GetFiles(path, "", SearchOption.AllDirectories);
        }

        public ImageFileSource GetItems(int pageNo = 1, int rowsPerPage = 40)
        {
            ImageFileSource ret = null;

            if (!Directory.Exists(ImagePath))
                return ret;

            DirectoryInfo di = new DirectoryInfo(ImagePath);

            string searchPattern = "*.*";
            var exts = new string[] { /*"*.png",*/ "*.jpg" };

            List<string> allFiles = di.GetFiles(searchPattern, SearchOption.AllDirectories)
                .Where(f => /*f.Extension == ".png" ||*/ f.Extension == ".jpg")
                .Select(x => x.FullName)
                .ToList();

            int totalRows = allFiles.Count;
            ret = new ImageFileSource();

            int maxPages = Convert.ToInt32(Math.Floor((decimal)(totalRows / rowsPerPage)));
            if ((maxPages * totalRows) < totalRows) maxPages++;

            ret.TotalRecords = totalRows;
            ret.MaxPages = maxPages;
            ret.RowsPerPage = rowsPerPage;
            ret.PageNo = pageNo;

            var files = allFiles.Skip((pageNo - 1) * rowsPerPage).Take(rowsPerPage).ToList();

            if (null != files && files.Count > 0)
            {
                files.ForEach(file => 
                {
                    var imgFile = new ImageFile(file);
                    if (imgFile.Exist)
                    {
                        ret.Items.Add(imgFile);
                    }
                });
            }

            return ret;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets current image path.
        /// </summary>
        public string ImagePath { get; protected set; }

        #endregion

        #region Static Methods


        /// <summary>
        /// Open Folder Browser to choose selected folder.
        /// </summary>
        /// <returns>Returns ImageFiles instance of selected folder.</returns>
        public static ImageFiles ChooseFolder(Window owner)
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

            ImageFiles ret = new ImageFiles();
            ret.Setup(targetPath);
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
