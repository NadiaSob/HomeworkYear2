using System;

namespace MyThreadPool
{
    /// <summary>
    /// Task for MyThreadPool to execute.
    /// </summary>
    /// <typeparam name="TResult">Type of the task result.</typeparam>
    public interface IMyTask<TResult>
    {
        /// <summary>
        /// Indicates whether the task is completed.
        /// </summary>
        bool IsCompleted { get; }

        /// <summary>
        /// Result of the task.
        /// </summary>
        TResult Result { get; }

        /// <summary>
        /// Creates a new task which is applied to the result of this task. The new task is accepted for execution.
        /// </summary>
        /// <typeparam name="TNewResult">Type of the new task result.</typeparam>
        /// <param name="newSupplier">Function on the basis of which the new task is going to be created.</param>
        /// <returns>New task which is applied to the result of this task.</returns>
        IMyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult, TNewResult> newSupplier);
    }
}
