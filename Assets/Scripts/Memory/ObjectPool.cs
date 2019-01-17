using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Memory
{
    interface IObjectPool
    {
    }

    class ObjectPool<T> : IObjectPool where T : new()
    {
        private readonly int poolSize;
        private int currentIndex;
        private T[] buffer;
        private Stack<T> freeStack = new Stack<T>();

        public ObjectPool(int poolCount)
        {
            poolSize = poolCount;
            buffer = new T[poolCount];
            currentIndex = 0;
        }

        public T Get()
        {
            if (freeStack.Count == 0)
            {
                int returnIndex = currentIndex;
                currentIndex++;
                return buffer[returnIndex];
            }
            else
            {
                return freeStack.Pop();
            }
        }

        public T Get(params object[] args)
        {
            return freeStack.Pop();
        }

        public void Free(T item)
        {
            freeStack.Push(item);
        }
    }
}
