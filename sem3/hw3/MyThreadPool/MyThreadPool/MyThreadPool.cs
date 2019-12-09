using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyThreadPool
{
    public class MyThreadPool
    {
        /// <summary>
        /// Очередь задач, принятых к исполнению.
        /// </summary>
        private ConcurrentQueue<Action> taskQueue;

        /// <summary>
        /// Возвращает связанный с ним CancellationToken.
        /// </summary>
        private CancellationTokenSource cancellationToken;

        public MyThreadPool(int numberOfThreads)
        {
            taskQueue = new ConcurrentQueue<Action>();
            cancellationToken = new CancellationTokenSource();
        }

        public void AddTask<TResult>(Func<TResult> supplier)
        {
            if (cancellationToken.Token.IsCancellationRequested)
            {
                throw new InvalidOperationException();
            }

            var myTask = new MyTask<TResult>(supplier);
            taskQueue.Enqueue(myTask.Calculate);
        }

        /// <summary>
        /// Завершает работу потоков.
        /// </summary>
        public void Shutdown()
        {
            cancellationToken.Cancel();
        }

        private class MyTask<TResult> : IMyTask<TResult>
        {
            public bool IsCompleted;

            public TResult Result;

            private Func<TResult> supplier;

            public MyTask(Func<TResult> supplier)
            {
                this.supplier = supplier;
            }

            public void Calculate()
            {
                try
                {
                    Result = supplier();
                }
                catch(Exception exception)
                {
                    throw new AggregateException(exception);
                }

            }

            public IMyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult, TNewResult> newTask)
            {
                
            }
        }
    }
}
