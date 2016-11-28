using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Techamante.Base
{
    public class Singleton<T> where T : new()
    {
        private static T instance;
        private static object syncObject;

        private static object InternalSyncObject
        {
            get
            {
                if (syncObject == null)
                {
                    object obj = new object();
                    Interlocked.CompareExchange(ref syncObject, obj, null);
                }
                return syncObject;
            }
        }

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (InternalSyncObject)
                    {
                        if (instance == null)
                        {
                            instance = new T();
                        }
                    }
                }
                return instance;
            }
        }

        static Singleton()
        {
            instance = new T();
        }
    }
}

