using System;

namespace Lazy
{
    /// <summary>
    /// Class that creates ILazy objects.
    /// </summary>
    /// <typeparam name="T">Type of the calculating value.</typeparam>
    public class LazyFactory<T> 
    {
        /// <summary>
        /// Returns ILazy object that does not guarantee correct work in multithreaded mode.
        /// </summary>
        /// <param name="supplier">Object that does the calculation.</param>
        /// <returns>ILazy object that does not guarantee correct work in multithreaded mode.</returns>
        public static ILazy<T> CreateLazy(Func<T> supplier) => new Lazy<T>(supplier);

        /// <summary>
        /// Returns ILazy object that guarantee correct work in multithreaded mode.
        /// </summary>
        /// <param name="supplier">Object that does the calculation.</param>
        /// <returns>ILazy object that guarantee correct work in multithreaded mode.</returns>
        public static ILazy<T> CreateMultithreadedLazy(Func<T> supplier) => new MultithreadedLazy<T>(supplier);
    }
}
