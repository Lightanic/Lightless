using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Memory
{
    public sealed class ObjectManager
    {
        private static ObjectManager instance = null;
        private static readonly object padlock = new object();

        private Dictionary<int, IObjectPool> pools = new Dictionary<int, IObjectPool>();

        public static ObjectManager Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new ObjectManager();
                    }
                    return instance;
                }
            }
        }

        private ObjectManager()
        {
        }

        public static void Register<T>(int poolSize)
        {
            instance.RegisterPool<T>(poolSize);
        }

        public static T Get<T>()
        {
            return instance.GetObject<T>();
        }

        private void RegisterPool<T>(int poolSize)
        {
            var hash = typeof(T).GetHashCode();
            if(!pools.ContainsKey(hash))
            {
                pools.Add(hash, new ObjectPool<T>(poolSize));
            }
        }

        public T GetObject<T>()
        {
            var hash = typeof(T).GetHashCode();
            return ((ObjectPool<T>)pools[hash]).Get();
        }

        public void Free<T>(T item)
        {
            var hash = typeof(T).GetHashCode();
           ((ObjectPool<T>)pools[hash]).Free(item);
        }
    }
}
