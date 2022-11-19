#region Using

using System;

#endregion

namespace PPRP
{
    #region NSingelton<T>

    /// <summary>
    /// NSingelton class.
    /// </summary>
    /// <typeparam name="T">The target singelton class.</typeparam>
    public class NSingelton<T> where T : class
    {
        #region Singelton

        /// <summary>
        /// Static Readonly property.
        /// </summary>
        private static readonly Lazy<T> Lazy = new Lazy<T>(() =>
        {
            T ret = default(T);
            lock (typeof(NSingelton<T>))
            {
                try
                {
                    ret = Activator.CreateInstance(typeof(T), true) as T;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    ret = default(T);
                }
            }
            return ret;
        });
        /// <summary>
        /// Gets singleton instance.
        /// </summary>
        public static T Instance => Lazy.Value;

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        protected NSingelton() : base() { }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~NSingelton() { }

        #endregion
    }

    #endregion
}
