using System;

namespace Lazy
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Lazy<T> : ILazy<T>
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

        public Lazy(Func<T> supplier)
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
                result = supplier();
                supplier = null;
                isCalculated = true;
            }

            return result;
        }
    }
}
