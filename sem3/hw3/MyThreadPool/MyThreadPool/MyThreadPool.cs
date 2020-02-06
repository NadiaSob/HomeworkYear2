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

        private Thread[] threads;

        private AutoResetEvent newTaskAvailable;

        public MyThreadPool(int numberOfThreads)
        {
            if (numberOfThreads <= 0)
            {
                throw new ArgumentOutOfRangeException("Number of threads should be a positive number.");
            }

            taskQueue = new ConcurrentQueue<Action>();
            cancellationToken = new CancellationTokenSource();
            newTaskAvailable = new AutoResetEvent(false);
            CreateThreads(numberOfThreads);
        }

        private void CreateThreads(int numberOfThreads)
        {
            threads = new Thread[numberOfThreads];

            for (var i = 0; i < numberOfThreads; ++i)
            {
                threads[i] = new Thread(() =>
                {
                    while (true)
                    {
                        if (cancellationToken.Token.IsCancellationRequested && taskQueue.IsEmpty)
                        {
                            return;
                        }

                        if (taskQueue.TryDequeue(out var action))
                        {
                            action();
                        }
                        else
                        {
                            newTaskAvailable.WaitOne();
                        }
                    }
                });
                threads[i].Start();
            }
        }

        public IMyTask<TResult> AddTask<TResult>(Func<TResult> supplier)
        {
            if (cancellationToken.Token.IsCancellationRequested)
            {
                throw new InvalidOperationException("Thread pool has been closed.");
            }

            var myTask = new MyTask<TResult>(supplier, this);
            taskQueue.Enqueue(myTask.Calculate);
            newTaskAvailable.Set();

            return myTask;
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
            public bool IsCompleted { get; private set; } = false;

            public TResult Result { 
                get
                {
                    isCompletedEvent.WaitOne();

                    if (aggregateException != null)
                    {
                        throw aggregateException;
                    }

                    return result;
                }

                private set
                {
                    result = value;
                }
            }

            private TResult result;

            private Func<TResult> supplier;

            private AggregateException aggregateException;

            private ManualResetEvent isCompletedEvent;

            private MyThreadPool myThreadPool;

            private Queue<Action> localTaskQueue;

            private object queueLockObject = new object();

            public MyTask(Func<TResult> supplier, MyThreadPool myThreadPool)
            {
                localTaskQueue = new Queue<Action>();
                this.supplier = supplier;
                isCompletedEvent = new ManualResetEvent(false);
                this.myThreadPool = myThreadPool;
            }

            public void Calculate()
            {
                try
                {
                    Result = supplier();
                }
                catch (Exception exception)
                {
                    aggregateException = new AggregateException(exception);
                }
                finally
                {
                    supplier = null;
                    IsCompleted = true;
                    isCompletedEvent.Set();

                    lock (queueLockObject)
                    {
                        while (localTaskQueue.Count != 0)
                        {
                            myThreadPool.AddTask(localTaskQueue.Dequeue);
                        }
                    }
                }
            }

            public IMyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult, TNewResult> newSupplier)
            {
                var newTask = new MyTask<TNewResult>(() => newSupplier(Result), myThreadPool);

                lock (queueLockObject)
                {
                    if (!IsCompleted)
                    {
                        localTaskQueue.Enqueue(() => newSupplier(Result));
                        return newTask;
                    }
                }
                return myThreadPool.AddTask(() => newSupplier(Result));
            }
        }
    }
}
