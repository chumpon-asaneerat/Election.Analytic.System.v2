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
            PathName = Path.GetDirectoryName(fullFileName);
            FileName = Path.GetFileName(fullFileName);
            FileNameOnly = Path.GetFileNameWithoutExtension(fullFileName);
            Extension = Path.GetExtension(fullFileName);
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
        /// Gets Path Name.
        /// </summary>
        public string PathName { get; private set; }

        public string FileName { get; private set; }

        public string FileNameOnly { get; private set; }

        public string Extension { get; private set; }

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

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets current image path.
        /// </summary>
        public string ImagePath { get; protected set; }

        #endregion
    }

    #endregion
}
