#region Using

using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NLib;
using NLib.Reflection;

#endregion

namespace PPRP
{
    #region ObjectPropertiesToString

    /// <summary>
    /// ObjectPropertiesToString Extnsion Methods
    /// </summary>
    public static class ObjectPropertiesToString
    {
        #region Static Variable

        private static Dictionary<Type, List<PropertyInfo>> Caches = new Dictionary<Type, List<PropertyInfo>>();

        #endregion

        #region Static Methods

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
                var properties = targetType.GetProperties(BindingFlags.Instance | BindingFlags.Public).ToList();
                Caches.Add(targetType, properties);
            }
            return Caches[targetType];
        }
        /// <summary>
        /// Gets Object Debug String.
        /// </summary>
        /// <param name="value">The object instance.</param>
        /// <returns>Returns string that contains all proeprty and its value.</returns>
        public static string DebugString(this object value)
        {
            string result = string.Empty;
            if (null == value) result = "instance is null.";
            else
            {
                Type type = value.GetType();
                var props = GetProperties(type);
                if (null != props && props.Count > 0)
                {
                    props.ForEach((prop) =>
                    {
                        result += prop.Name + ": " + DynamicAccess.Get(value, prop.Name).ToString() + Environment.NewLine;
                    });
                }
                else result = "object has no property.";
            }
            return result.Trim();
        }

        #endregion
    }


    #endregion
}
