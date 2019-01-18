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

        public static void Register<T>(int poolSize) where T : new()
        {
            instance.RegisterPool<T>(poolSize);
        }

        public static T Get<T>() where T : new()
        {
            return instance.GetObject<T>();
        }

        private void RegisterPool<T>(int poolSize) where T : new()
        {
            var hash = typeof(T).GetHashCode();
            if (!pools.ContainsKey(hash))
            {
                pools.Add(hash, new ObjectPool<T>(poolSize));
            }
        }

        public T GetObject<T>() where T : new()
        {
            var hash = typeof(T).GetHashCode();
            return GetPool<T>().Get();
        }

        public void Free<T>(T item) where T : new()
        {
            var hash = typeof(T).GetHashCode();
            GetPool<T>().Free(item);
        }

        private ObjectPool<T> GetPool<T>() where T: new()
        {
            var hash = typeof(T).GetHashCode();
            if (!pools.ContainsKey(hash))
            {
                pools.Add(hash, new ObjectPool<T>(1));
            }

            return ((ObjectPool<T>)pools[hash]);
        }
    }
}
