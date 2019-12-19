using System;

namespace Lazy
{
    /// <summary>
    /// ILazy implementation that does not guarantee correct work in multithreaded mode.
    /// </summary>
    /// <typeparam name="T">Type of the calculating value.</typeparam>
    public class Lazy<T> : ILazy<T>
    {
        private bool isCalculated = false;

        private Func<T> supplier;

        private T result;

        public Lazy(Func<T> supplier)
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
                result = supplier();
                supplier = null;
                isCalculated = true;
            }

            return result;
        }
    }
}
