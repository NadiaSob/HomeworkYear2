using System;
using System.Threading;

namespace Lazy
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MultithreadedLazy<T> : ILazy<T>
    {
        /// <summary>
        ///  
        /// </summary>
        private bool isCalculated = false;

        /// <summary>
        /// 
        /// </summary>
        private Func<T> supplier;

        /// <summary>
        /// 
        /// </summary>
        private T result;

        /// <summary>
        /// 
        /// </summary>
        private object locker = new object();

        public MultithreadedLazy(Func<T> supplier)
        {
            if (supplier == null)
            {
                throw new ArgumentNullException();
            }

            this.supplier = supplier;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public T Get()
        {
            if (!isCalculated)
            {
                lock (locker)
                {
                    if (isCalculated)
                    {
                        return result;
                    }

                    result = supplier();
                    supplier = null;
                    isCalculated = true;
                }
            }

            return result;
        }
    }
}
