#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
    #region ExcelColumnMode

    /// <summary>
    /// The ExcelColumnMode Enum.
    /// </summary>
    public enum ExcelColumnMode
    {
        /// <summary>
        /// Import and Export.
        /// </summary>
        Both = 0,
        /// <summary>
        /// Import Only.
        /// </summary>
        Import = 1,
        /// <summary>
        /// Export Only.
        /// </summary>
        Export = 2
    }

    #endregion

    #region ExcelColumnAttribute

    /// <summary>
    /// The ExcelColumnAttribute class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ExcelColumnAttribute : Attribute
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="headerText">The excel column header's text.</param>
        /// <param name="mode">The excel column mode.</param>
        /// <param name="properyName">The target class's property name (Optional).</param>
        public ExcelColumnAttribute(string headerText, ExcelColumnMode mode = ExcelColumnMode.Both, [CallerMemberName] string properyName = null) : base()
        {
            this.PropertyName = properyName;

            this.Mode = mode;

            if (!string.IsNullOrWhiteSpace(headerText))
                this.HeaderText = headerText;
            else this.HeaderText = this.PropertyName;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets Column Header Text.
        /// </summary>
        public string HeaderText { get; set; }
        /// <summary>
        /// Gets or sets Column Mode.
        /// </summary>
        public ExcelColumnMode Mode { get; private set; }
        /// <summary>
        /// Gets the attach property.
        /// </summary>
        public string PropertyName { get; private set; }

        #endregion
    }

    #endregion

    #region ExcelColumnMap

    public class ExcelColumnMap
    {
    }

    #endregion

    #region NExcelColumn

    /// <summary>
    /// The NExcelColumn class.
    /// </summary>
    public class NExcelColumn
    {
        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public NExcelColumn() : this(-1, string.Empty, string.Empty) { }
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="columnIndex">The Column Index. index start with 1.</param>
        /// <param name="columnLetter">The Column Letter like 'A', 'B'.</param>
        public NExcelColumn(int columnIndex, string columnLetter) : this(columnIndex, columnLetter, string.Empty) { }
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="columnIndex">The Column Index. index start with 1.</param>
        /// <param name="columnLetter">The Column Letter like 'A', 'B'.</param>
        /// <param name="columnName">The Column Name (normally is from first row in excel).</param>
        public NExcelColumn(int columnIndex, string columnLetter, string columnName) : base()
        {
            this.ColumnIndex = columnIndex;
            this.ColumnLetter = columnLetter;
            this.ColumnName = columnName;
        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~NExcelColumn() { }

        #endregion

        #region Override Methods

        /// <summary>
        /// Equals.
        /// </summary>
        /// <param name="obj">The target object instance.</param>
        /// <returns>Returns true if target instance is equal to current instance</returns>
        public override bool Equals(object obj)
        {
            if (null == obj) return false;
            var curr = this.GetHashCode();
            var target = obj.GetHashCode();
            return curr.Equals(target);
        }
        /// <summary>
        /// GetHashCode.
        /// </summary>
        /// <returns>Returns hash code of object instance.</returns>
        public override int GetHashCode()
        {
            string sVal = this.ToString();
            return sVal.GetHashCode();
        }
        /// <summary>
        /// ToString.
        /// </summary>
        /// <returns>Returns string that represents object instance.</returns>
        public override string ToString()
        {
            string code;
            //code = string.Format("{0}_{1}",
            //    string.IsNullOrWhiteSpace(this.ColumnLetter) ? string.Empty : this.ColumnLetter.Trim(),
            //    string.IsNullOrWhiteSpace(this.ColumnName) ? string.Empty : this.ColumnName.Trim());

            code = string.Format("{0}",
                string.IsNullOrWhiteSpace(this.ColumnLetter) ? string.Empty : this.ColumnLetter.Trim());
            return code.ToString();
        }

        #endregion

        #region Public Properties

        /// <summary>Gets or sets excel worksheet's column name.</summary>
        public string ColumnName { get; set; }
        /// <summary>Gets or sets excel worksheet's column index. This index is start with 1.</summary>
        public int ColumnIndex { get; set; }

        /// <summary>Gets or sets excel worksheet's column letter like 'A', 'B', ..., 'AA', etc.</summary>
        public string ColumnLetter { get; set; }

        #endregion
    }

    #endregion

    #region NExcelMapProperty

    /// <summary>
    /// The NExcelMapProperty class. Use to map class' property to Excel column index.
    /// </summary>
    public class NExcelMapProperty : NInpc
    {
        #region Internal Variables

        private int _ColumnIndex = -1;
        private string _ColumnLetter = string.Empty;

        private NExcelColumn _SelectedColumn;

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor (default disable).
        /// </summary>
        private NExcelMapProperty() : base() { }
        /// <summary>
        /// Constructor.
        /// </summary>
        public NExcelMapProperty(NExcelWorksheet sheet) : base() 
        {
            this.Sheet = sheet;
            this.Columns = new List<NExcelColumn>();
        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~NExcelMapProperty()
        {
            if (null != this.Columns)
            {
                lock (this)
                {
                    this.Columns.Clear();
                    this.Columns = null;
                }
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets Target property name.
        /// </summary>
        public string PropertyName { get; set; }
        /// <summary>
        /// Gets or sets Target Display Text.
        /// </summary>
        public string DisplayText { get; set; }
        /// <summary>
        /// Gets or sets Excel column'name like 'A', 'B', 'C', etc.
        /// </summary>
        public string ColumnLetter
        {
            get { return _ColumnLetter; }
            set
            {
                if (_ColumnLetter != value)
                {
                    _ColumnLetter = value;
                    this.Raise(() => this.ColumnLetter);
                    this.Raise(() => this.DebugInfo);
                }
            }
        }
        /// <summary>
        /// Gets or sets Column Index.
        /// </summary>
        public int ColumnIndex
        {
            get { return _ColumnIndex; }
            set
            {
                if (_ColumnIndex != value)
                {
                    _ColumnIndex = value;
                    this.Raise(() => this.ColumnIndex);
                    this.Raise(() => this.DebugInfo);
                }
            }
        }
        /// <summary>Gets Instance degug data.</summary>
        public string DebugInfo
        {
            get 
            {
                string msg = string.Format("'{0}' => Letter: '{1}', Index: '{2}'", 
                    PropertyName, _ColumnLetter, _ColumnIndex);
                return msg;
            }
            set { }
        }

        #endregion

        #region Public Properties (For binding Map Columns)

        /// <summary>
        /// Gets Excel Worksheet.
        /// </summary>
        public NExcelWorksheet Sheet { get; protected set; }
        /// <summary>
        /// Gets or sets all avaliable excel Columns.
        /// </summary>
        public List<NExcelColumn> Columns
        {
            get { return (null != Sheet) ? Sheet.Columns : null; }
            set { }
        }
        /// <summary>
        /// The selected column for lookup bindings (like ComboBox.SelectedItem).
        /// </summary>
        public NExcelColumn SelectedColumn
        {
            get { return _SelectedColumn; }
            set
            {
                if (_SelectedColumn != value)
                {
                    _SelectedColumn = value;
                    this.ColumnLetter = (null != _SelectedColumn) ? _SelectedColumn.ColumnLetter : string.Empty;
                    this.ColumnIndex = (null != _SelectedColumn) ? _SelectedColumn.ColumnIndex : -1;
                    // Raise Event
                    this.Raise(() => this.SelectedColumn);
                }
            }
        }

        #endregion
    }

    #endregion

    #region NExcelWorksheet

    /// <summary>
    /// The NExcelWorksheet class.
    /// </summary>
    public class NExcelWorksheet
    {
        #region Reflection Utils class

        /// <summary>
        /// The Reflection Utils class.
        /// </summary>
        protected class Utils
        {
            #region Static Variable

            private static Dictionary<Type, List<PropertyInfo>> Caches = new Dictionary<Type, List<PropertyInfo>>();

            #endregion

            #region Public Static Methods

            /// <summary>
            /// Gets Properties of target type.
            /// </summary>
            /// <param name="targetType">The Target type.</param>
            /// <returns>Returns all property that has attribute ExcelColumnAttribute.</returns>
            public static List<PropertyInfo> GetProperties(Type targetType)
            {
                if (null == targetType)
                    return null;
                if (!Caches.ContainsKey(targetType))
                {
                    var properties = targetType.GetProperties()
                        .Where(prop => prop.IsDefined(typeof(ExcelColumnAttribute), false)).ToList();
                    Caches.Add(targetType, properties);
                }
                return Caches[targetType];
            }
            /// <summary>
            /// Gets ExcelColumnAttribute from target property.
            /// </summary>
            /// <param name="prop">The PropertyInfo instance.</param>
            /// <returns>Returns instance of ExcelColumnAttribute.</returns>
            public static ExcelColumnAttribute GetAttribute(PropertyInfo prop)
            {
                if (null == prop) return null;
                return (ExcelColumnAttribute)prop.GetCustomAttributes(typeof(ExcelColumnAttribute), false).First();
            }

            #endregion
        }

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        private NExcelWorksheet() : base() 
        {
            // prepare mapping column collections.
            this.Columns = new List<NExcelColumn>();
            // Auto load mapping from target class.
            this.Mappings = new List<NExcelMapProperty>();
        }
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="model">The Excel Models</param>
        public NExcelWorksheet(ExcelModel model) : this()
        {
            this.Model = model;
        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~NExcelWorksheet() 
        {
            // Free Mappings.
            if (null != this.Mappings)
            {
                lock (this)
                {
                    this.Mappings.Clear();
                    this.Mappings = null;
                }
            }
            // Free Columns.
            if (null != this.Columns)
            {
                lock (this)
                {
                    this.Columns.Clear();
                    this.Columns = null;
                }
            }
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// Equals.
        /// </summary>
        /// <param name="obj">The target object instance.</param>
        /// <returns>Returns true if target instance is equal to current instance</returns>
        public override bool Equals(object obj)
        {
            if (null == obj) return false;
            var curr = this.GetHashCode();
            var target = obj.GetHashCode();
            return curr.Equals(target);
        }
        /// <summary>
        /// GetHashCode.
        /// </summary>
        /// <returns>Returns hash code of object instance.</returns>
        public override int GetHashCode()
        {
            string sVal = this.ToString();
            return sVal.GetHashCode();
        }
        /// <summary>
        /// ToString.
        /// </summary>
        /// <returns>Returns string that represents object instance.</returns>
        public override string ToString()
        {
            string code;
            //int colCnt = (null == this.Columns) ? -1 : this.Columns.Count;
            //code = string.Format("{0}_{1}",
            //  string.IsNullOrWhiteSpace(this.SheetName) ? null : this.SheetName.Trim(), 
            //  colCnt);

            code = string.Format("{0}",
                string.IsNullOrWhiteSpace(this.SheetName) ? string.Empty : this.SheetName.Trim());

            return code.ToString();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Map Columns and Properties.
        /// </summary>
        /// <param name="targetType">The Target type.</param>
        private void MapColumnsProperties(Type targetType)
        {
            var props = Utils.GetProperties(targetType);
            foreach (var prop in props)
            {
                var attr = Utils.GetAttribute(prop);
                string propertyName = attr.PropertyName;
                string displayText = attr.HeaderText;

                if (Mode == ExcelColumnMode.Import)
                {
                    if (attr.Mode == ExcelColumnMode.Export)
                        continue; // mismatch mode
                }
                else if (Mode == ExcelColumnMode.Export)
                {
                    if (attr.Mode == ExcelColumnMode.Import)
                        continue; // mismatch mode
                }

                this.Mappings.Add(new NExcelMapProperty(this)
                {
                    PropertyName = propertyName,
                    DisplayText = displayText,
                    ColumnLetter = null,
                    ColumnIndex = -1
                });
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Call to invoke SheetItemChanges event. 
        /// </summary>
        public void RefrehsWorksheet()
        {
            if (null != Model)
                Model.RefrehsWorksheet(this);
        }
        /// <summary>
        /// Refresh mapping columns
        /// </summary>
        /// <param name="targetType">The Target type.</param>
        public void MapColumns(Type targetType)
        {
            MapColumnsProperties(targetType);
        }
        /// <summary>
        /// Load Items.
        /// </summary>
        /// <typeparam name="T">The target item type.</typeparam>
        /// <returns>
        /// Returns List of target item with data that read from worksheet.
        /// </returns>
        public List<T> LoadItems<T>()
            where T : class, new()
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            var results = new List<T>();

            #region Checking

            try
            {
                if (null == Model ||
                    string.IsNullOrWhiteSpace(Model.FileName) ||
                    string.IsNullOrWhiteSpace(SheetName) ||
                    null == Mappings || Mappings.Count <= 0)
                {
                    // something is missing.
                    return results;
                }
            }
            catch (Exception ex1)
            {
                med.Err(ex1);

                return results; // Error so exit.
            }

            #endregion

            #region Create local mappings

            Dictionary<string, int> columns = new Dictionary<string, int>();
            foreach (var map in Mappings)
            {
                if (map.ColumnIndex < 1)
                    continue; // ignore if column < 1

                if (!columns.ContainsKey(map.PropertyName))
                    columns.Add(map.PropertyName, map.ColumnIndex);
                else columns[map.PropertyName] = map.ColumnIndex;
            }

            #endregion

            #region Load data

            using (var package = new ExcelPackage(Model.FileName))
            {
                try
                {
                    var sheet = package.Workbook.Worksheets[SheetName];
                    if (null != sheet)
                    {
                        int colCount = sheet.Dimension.End.Column;  // get Column Count
                        int rowCount = sheet.Dimension.End.Row;     // get row count
                        // start row at position 2.
                        for (int row = 2; row <= rowCount; row++)
                        {
                            var inst = new T(); // create instance.

                            foreach (var key in columns.Keys)
                            {
                                int colIdx = columns[key];
                                if (colIdx < 1) continue;
                                try
                                {
                                    object oVal = sheet.Cells[row, columns[key]].Value;
                                    DynamicAccess<T>.Set(inst, key, oVal);
                                }
                                catch (Exception ex)
                                {
                                    //Console.WriteLine(ex);
                                    med.Err(ex);
                                }
                            }
                            // append to result list
                            results.Add(inst);
                        }
                    }
                }
                catch (Exception ex2)
                {
                    med.Err(ex2);
                }
            }

            #endregion

            #region Clear local mappings

            if (null != columns)
                columns.Clear();
            columns = null;

            #endregion

            return results;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets Excel Model.
        /// </summary>
        public ExcelModel Model { get; private set; }
        /// <summary>
        /// Gets or sets excel worksheet name.
        /// </summary>
        public string SheetName { get; set; }
        /// <summary>
        /// Gets Mappings.
        /// </summary>
        public List<NExcelMapProperty> Mappings { get; protected set; }
        /// <summary>
        /// Gets or sets all avaliable excel Columns.
        /// </summary>
        public List<NExcelColumn> Columns { get; protected set; }
        /// <summary>
        /// Gets or sets ExcelColumnMode.
        /// </summary>
        public ExcelColumnMode Mode { get; set; }

        #endregion
    }

    #endregion

    #region ExcelModel Event Handlers and Event Args

    /// <summary>
    /// The ExcelWorksheetArgs class.
    /// </summary>
    public class ExcelWorksheetArgs
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public ExcelWorksheetArgs() : base() { }
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="sheet"></param>
        public ExcelWorksheetArgs(NExcelWorksheet sheet) : this()
        {
            Sheet = sheet;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets Worksheet.
        /// </summary>
        public NExcelWorksheet Sheet { get; set;}

        #endregion
    }
    /// <summary>
    /// The ExcelWorksheetItemsChanges delegate.
    /// </summary>
    /// <param name="sender">The sender object.</param>
    /// <param name="evt">The ExcelWorksheetArgs instance.</param>
    public delegate void ExcelWorksheetItemsChanges(object sender, ExcelWorksheetArgs evt);

    #endregion

    #region ExcelModel

    /// <summary>
    /// The Excel Model class.
    /// </summary>
    public class ExcelModel
    {
        #region Dialog class

        /// <summary>
        /// Dialogs utils class
        /// </summary>
        public class Dialogs
        {
            #region Show Open Excel Dialog

            /// <summary>
            /// Show Open Excel File Dialog.
            /// </summary>
            /// <param name="title">The Dialog Title.</param>
            /// <param name="initDir">The initial directory path.</param>
            /// <returns>Returns FileName if user choose file otherwise return null.</returns>
            public static string OpenDialog(string title = "กรุณาเลือก excel file ที่ต้องการนำเข้าข้อมูล",
                string initDir = null)
            {
                return OpenDialog(null, title, initDir);
            }
            /// <summary>
            /// Show Open Excel File Dialog.
            /// </summary>
            /// <param name="owner">The owner window.</param>
            /// <param name="title">The Dialog Title.</param>
            /// <param name="initDir">The initial directory path.</param>
            /// <returns>Returns FileName if user choose file otherwise return null.</returns>
            public static string OpenDialog(Window owner,
                string title = "กรุณาเลือก excel file ที่ต้องการนำเข้าข้อมูล",
                string initDir = null)
            {
                string fileName = null;

                // setup dialog options
                var od = new Microsoft.Win32.OpenFileDialog();
                od.Multiselect = false;
                od.InitialDirectory = initDir;
                od.Title = string.IsNullOrEmpty(title) ? "กรุณาเลือก excel file ที่ต้องการนำเข้าข้อมูล" : title;
                od.Filter = "Excel Files(*.xls, *.xlsx)|*.xls;*.xlsx";

                var ret = od.ShowDialog(owner) == true;
                if (ret)
                {
                    // assigned to FileName
                    fileName = od.FileName;
                }
                od = null;

                return fileName;
            }

            #endregion

            #region Show Save Excel Dialog

            /// <summary>
            /// Show Save Excel File Dialog.
            /// </summary>
            /// <param name="defaultFileName">The Default File Name.</param>
            /// <returns>Returns FileName if user choose file otherwise return null.</returns>
            public static string SaveDialog(string defaultFileName)
            {
                return SaveDialog(null, null, "กรุณาระบุขื่อ excel file ที่ต้องการนำส่งออกข้อมูล", defaultFileName);
            }
            /// <summary>
            /// Show Save Excel File Dialog.
            /// </summary>
            /// <param name="title">The Dialog Title.</param>
            /// <param name="initDir">The initial directory path.</param>
            /// <returns>Returns FileName if user choose file otherwise return null.</returns>
            public static string SaveDialog(string title = "กรุณาระบุขื่อ excel file ที่ต้องการนำส่งออกข้อมูล",
                string initDir = null)
            {
                return SaveDialog(null, title, initDir);
            }
            /// <summary>
            /// Show Save Excel File Dialog.
            /// </summary>
            /// <param name="owner">The owner window.</param>
            /// <param name="title">The Dialog Title.</param>
            /// <param name="initDir">The initial directory path.</param>
            /// <param name="defaultFileName">The Default File Name.</param>
            /// <returns>Returns FileName if user choose file otherwise return null.</returns>
            public static string SaveDialog(Window owner,
                string title = "กรุณาเลือก excel file ที่ต้องการนำเข้าข้อมูล",
                string initDir = null,
                string defaultFileName = "")
            {
                string fileName = null;

                // setup dialog options
                var sd = new Microsoft.Win32.SaveFileDialog();
                sd.InitialDirectory = initDir;
                sd.Title = string.IsNullOrEmpty(title) ? "กรุณาระบุขื่อ excel file ที่ต้องการนำส่งออกข้อมูล" : title;
                sd.Filter = "Excel Files(*.xls, *.xlsx)|*.xls;*.xlsx";
                sd.FileName = defaultFileName;
                var ret = sd.ShowDialog(owner) == true;
                if (ret)
                {
                    // assigned to FileName
                    fileName = sd.FileName;
                }
                sd = null;

                return fileName;
            }

            #endregion
        }

        #endregion

        #region Static Methods - Register License

        /// <summary>
        /// Register License.
        /// </summary>
        public static void RegisterLicense()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public ExcelModel() : base()
        {
            // Register license.
            RegisterLicense();
            // Create worksheet list.
            this.Worksheets = new List<NExcelWorksheet>();
        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~ExcelModel()
        {
            // Free worksheets
            if (null != this.Worksheets)
            {
                lock (this)
                {
                    this.Worksheets.Clear();
                    this.Worksheets = null;
                }
            }
        }

        #endregion

        #region Private Methods

        private bool LaodWorksheets()
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            bool ret = false;

            if (string.IsNullOrWhiteSpace(FileName))
            {
                return ret;
            }

            if (null == Worksheets) Worksheets = new List<NExcelWorksheet>();
            Worksheets.Clear(); // clear exists worksheets

            using (var package = new ExcelPackage(FileName))
            {
                try
                {
                    var xlsSheets = package.Workbook.Worksheets;
                    foreach (var xlsSheet in xlsSheets)
                    {
                        // Create new NExcelWorksheet.
                        var nSheet = new NExcelWorksheet(this) { SheetName = xlsSheet.Name };
                        // Extract columns into new NExcelWorksheet.
                        if (null != xlsSheet &&
                            null != xlsSheet.Dimension &&
                            null != xlsSheet.Dimension.End &&
                            xlsSheet.Dimension.End.Column > 0)
                        {
                            // row always 1
                            int iRow = 1;
                            for (int iCol = 1; iCol <= xlsSheet.Dimension.End.Column; iCol++)
                            {
                                // get column value
                                var oVal = xlsSheet.Cells[iRow, iCol].Value;
                                if (null != oVal)
                                {
                                    var nColumn = new NExcelColumn()
                                    {
                                        ColumnName = oVal.ToString(),
                                        ColumnIndex = iCol,
                                        ColumnLetter = ExcelCellAddress.GetColumnLetter(iCol)
                                    };
                                    // Add to column list
                                    nSheet.Columns.Add(nColumn);
                                }
                            }
                        }
                        // Append to list.
                        Worksheets.Add(nSheet);
                    }

                    ret = true;
                }
                catch (Exception ex)
                {
                    med.Err(ex);
                    try
                    {
                        if (null != package) package.Dispose();
                    }
                    catch
                    {
                        Console.WriteLine("package dispose error.");
                    }
                }
            }

            return ret;
        }

        #endregion

        #region Public Methods

        #region Event Raisers

        /// <summary>
        /// Call to invoke SheetItemChanges event. 
        /// </summary>
        /// <param name="sheet">The target Worksheet.</param>
        public void RefrehsWorksheet(NExcelWorksheet sheet)
        {
            if (null == sheet)
                return; // no worksheet so ignore.
            SheetItemChanges.Call(this, new ExcelWorksheetArgs(sheet));
        }

        #endregion

        #region Open

        /// <summary>
        /// Open File.
        /// </summary>
        /// <returns>Returns true if file selected</returns>
        public bool Open()
        {
            bool ret = false;

            string file = Dialogs.OpenDialog();
            if (!string.IsNullOrWhiteSpace(file))
            {
                FileName = file;
                ret = LaodWorksheets();
            }

            return ret;
        }

        #endregion

        #region Save

        /// <summary>
        /// Save File as.
        /// </summary>
        /// <returns>Returns true if file selected</returns>
        public bool SaveAs()
        {
            bool ret = false;

            string file = Dialogs.SaveDialog();
            if (!string.IsNullOrWhiteSpace(file))
            {
                FileName = file;
                // need create worksheet function.
            }

            return ret;
        }

        #endregion

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets File Name.
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// Gets Excel Worksheets.
        /// </summary>
        public List<NExcelWorksheet> Worksheets { get; protected set; }

        #endregion

        #region Public Events

        /// <summary>
        /// The SheetItemChanges event.
        /// </summary>
        public event ExcelWorksheetItemsChanges SheetItemChanges;

        #endregion
    }

    #endregion
}
