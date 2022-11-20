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

    public class ExcelColumnAttribute : Attribute
    {
        public ExcelColumnAttribute(string displayName, [CallerMemberName] string properyName = null) : base()
        {
            this.PropertyName = properyName;
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
            private set;
        }
    }
}
