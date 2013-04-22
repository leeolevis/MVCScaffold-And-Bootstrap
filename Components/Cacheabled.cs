using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp4.Components
{
    public class Cacheabled<T, K>
        where K : class
    {
        private static K current;

        public static K Current
        {
            get
            {
                if (current == null)
                    RaisLoad();
                return current;
            }
        }

        public static event Func<K> OnLoaded;

        private static void RaisLoad()
        {
            if (OnLoaded != null)
                current = OnLoaded();
        }

        public static void Clear()
        {
            current = null;
        }
    }
}