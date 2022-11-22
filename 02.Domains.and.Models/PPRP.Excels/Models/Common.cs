#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace PPRP.Models.Excel
{
    #region Interface

    /// <summary>
    /// The IExcelModel interface.
    /// </summary>
    public interface IExcelModel
    {

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
        /// <param name="properyName">The target class's property name (Optional).</param>
        public ExcelColumnAttribute(string headerText, [CallerMemberName] string properyName = null) : base()
        {
            this.PropertyName = properyName;
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
        /// Gets the attach property.
        /// </summary>
        public string PropertyName { get; private set; }

        #endregion
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
}
