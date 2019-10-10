using System;
using System.Threading;

namespace Lazy
{
    /// <summary>
    /// ILazy implementation that guarantee correct work in multithreaded mode.
    /// </summary>
    /// <typeparam name="T">Type of the calculating value.</typeparam>
    public class MultithreadedLazy<T> : ILazy<T>
    {
        private bool isCalculated = false;

        private Func<T> supplier;

        private T result;

        private object locker = new object();

        public MultithreadedLazy(Func<T> supplier)
        {
            this.supplier = supplier ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// Gets result of the calculation if it is not already calculated.
        /// </summary>
        /// <returns>Result of the calculation.</returns>
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
