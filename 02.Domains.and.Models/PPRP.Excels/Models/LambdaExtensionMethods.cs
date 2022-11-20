#region Using

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#endregion

namespace PPRP.Models
{
    public static class LambdaExtensionMethods
    {
        public static TAttribute GetAttribute<TAttribute, TObj, TProperty>(this TObj inst,
            Expression<Func<TObj, TProperty>> propertyExpression)
                 where TAttribute: Attribute
                 where TObj: IExcelModel
        {
            if (null == inst) return default;
            var body = propertyExpression.Body as MemberExpression;
            var expression = body.Member as PropertyInfo;
            var ret = (TAttribute)expression.GetCustomAttributes(typeof(TAttribute), false).First();

            return ret;
        }

        public static object[] GetPropertyAttributes<TObj, TProperty>(
            this TObj instance,
            Expression<Func<TObj, TProperty>> propertySelector)
                 where TObj : IExcelModel
        {
            //consider handling exceptions and corner cases
            var propertyName = ((PropertyInfo)((MemberExpression)propertySelector.Body).Member).Name;
            var property = instance.GetType().GetProperty(propertyName);
            return property.GetCustomAttributes(false);
        }

        public static T GetFirst<T>(this object[] input) where T : Attribute
        {
            //consider handling exceptions and corner cases
            return input.OfType<T>().First();
        }
    }
}
