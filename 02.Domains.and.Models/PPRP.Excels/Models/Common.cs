#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace PPRP.Models
{
    public interface IExcelModel
    {

    }

    [AttributeUsage(AttributeTargets.Property)]
    public class ExcelColumnAttribute : Attribute
    {
        public ExcelColumnAttribute(string headerText, [CallerMemberName] string properyName = null) : base()
        {
            this.PropertyName = properyName;
            if (!string.IsNullOrWhiteSpace(headerText))
                this.HeaderText = headerText;
            else this.HeaderText = this.PropertyName;
        }
        /// <summary>
        /// Gets or sets Column Header Text.
        /// </summary>
        public string HeaderText
        { 
            get; set; 
        }
        /// <summary>
        /// Gets the attach property.
        /// </summary>
        public string PropertyName 
        { 
            get; 
            private set;
        }
    }
}
