using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyThreadPool
{
    public interface IMyTask<TResult>
    {
        /// <summary>
        /// Возвращает true, если задача выполнена.
        /// </summary>
        bool IsCompleted { get; }

        /// <summary>
        /// Результат выполнения задачи.
        /// </summary>
        TResult Result { get; }

        IMyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult, TNewResult> newTask);
    }
}
