#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#endregion

namespace PPRP.Models
{
    public class ExcelColumnMap<T>
    {
        private static Dictionary<Type, List<PropertyInfo>> Caches = new Dictionary<Type, List<PropertyInfo>>();

        public List<PropertyInfo> GetProperties<TAttr>() 
            where TAttr: Attribute
        {
            var t = typeof(T);
            if (!Caches.ContainsKey(t))
            {
                var properties = typeof(T).GetProperties()
                    .Where(prop => prop.IsDefined(typeof(TAttr), false)).ToList();
                Caches.Add(t, properties);
            }
            return Caches[t];
        }
    }


    public class LambdaMap<T>
    {
        public virtual PropertyInfo PropertyInfo<U>(Expression<Func<T, U>> expression)
        {
            var member = expression.Body as MemberExpression;
            if (member != null && member.Member is PropertyInfo)
                return member.Member as PropertyInfo;

            throw new ArgumentException("Expression is not a Property", "expression");
        }

        public virtual string PropertyName<U>(Expression<Func<T, U>> expression)
        {
            return PropertyInfo<U>(expression).Name;
        }

        public virtual Type PropertyType<U>(Expression<Func<T, U>> expression)
        {
            return PropertyInfo<U>(expression).PropertyType;
        }
    }

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
