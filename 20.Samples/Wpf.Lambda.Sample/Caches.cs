using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf.Lambda.Sample
{
    public class Property<T> 
    { 
        public T Value { get; set; }
    }
    public class Caches
    {
        private static Caches _instance = null;
        
        public static Caches Instance
        {
            get 
            {
                if (null == _instance)
                {
                    lock (typeof(Caches))
                    {
                        _instance = new Caches();
                    }
                }
                return _instance;
            }
        }

        public T Of<T>()
        {
            return default;
        }
    }

    public class Test
    {
        public void Setup()
        {
            string name = Caches.Instance.Of<Test>().FullName;
            if (null != name) return;
        }

        public string FullName { get; set; }
    }
}
