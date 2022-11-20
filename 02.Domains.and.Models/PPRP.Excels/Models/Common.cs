#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace PPRP.Models
{
    public interface IExcelModel
    {

    }

    public class ExcelColumnAttribute : Attribute
    {
        public ExcelColumnAttribute() : base() 
        { 
        }
        public ExcelColumnAttribute(string displayName) : this()
        {
            if (!string.IsNullOrWhiteSpace(displayName))
                this.DisplayName = displayName;
            else this.DisplayName = this.PropertyName;
        }

        public string DisplayName
        { 
            get; set; 
        }
        public string PropertyName 
        { 
            get; 
            set; 
        }
    }
}
