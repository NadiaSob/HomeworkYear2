using System;

namespace Lazy
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LazyFactory<T> 
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="supplier"></param>
        /// <returns></returns>
        public static ILazy<T> CreateLazy(Func<T> supplier) => new Lazy<T>(supplier);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="supplier"></param>
        /// <returns></returns>
        public static ILazy<T> CreateMultithreadedLazy(Func<T> supplier) => new MultithreadedLazy<T>(supplier);
    }
}
