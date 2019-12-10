using System;

namespace Lazy
{
    /// <summary>
    /// Interface for lazy evaluation.
    /// </summary>
    /// <typeparam name="T">Type of the calculating value.</typeparam>
    public interface ILazy<T>
    {
        /// <summary>
        /// Gets result of the calculation if it is not already calculated.
        /// </summary>
        /// <returns>Result of the calculation.</returns>
        T Get();
    }
}
