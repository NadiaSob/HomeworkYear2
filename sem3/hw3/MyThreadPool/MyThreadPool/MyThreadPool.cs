using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace MyThreadPool
{
    /// <summary>
    /// Provides a pool of threads that can be used to execute tasks.
    /// </summary>
    public class MyThreadPool
    {
        public int NumberOfThreads { get; private set; }

        private int numberOfWorkingThreads = 0;

        private ConcurrentQueue<Action> taskQueue = new ConcurrentQueue<Action>();

        private CancellationTokenSource cancellationToken = new CancellationTokenSource();

        private Thread[] threads;

        private AutoResetEvent newTaskAvailable = new AutoResetEvent(false);

        private AutoResetEvent threadStopped = new AutoResetEvent(false);

        private object lockObject = new object();

        public MyThreadPool(int numberOfThreads)
        {
            if (numberOfThreads <= 0)
            {
                throw new ArgumentOutOfRangeException("Number of threads should be a positive number.");
            }

            NumberOfThreads = numberOfThreads;
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
                            Interlocked.Decrement(ref numberOfWorkingThreads);
                            threadStopped.Set();
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

                Interlocked.Increment(ref numberOfWorkingThreads);
            }
        }

        /// <summary>
        /// Queues a task for thread pool execution.
        /// </summary>
        /// <typeparam name="TResult">Type of the task result.</typeparam>
        /// <param name="supplier">Function on the basis of which the task is going to be created.</param>
        /// <returns>Queued task.</returns>
        public IMyTask<TResult> AddTask<TResult>(Func<TResult> supplier)
        {
            var myTask = new MyTask<TResult>(supplier, this);

            lock (lockObject)
            {
                if (!cancellationToken.Token.IsCancellationRequested)
                {
                    if (numberOfWorkingThreads != 0)
                    {
                        EnqueueTask(myTask.Calculate);
                        return myTask;
                    }
                }
            }

            throw new InvalidOperationException("Thread pool has been closed.");
        }

        private void EnqueueTask(Action task)
        {
            taskQueue.Enqueue(task);
            newTaskAvailable.Set();
        }

        /// <summary>
        /// Shuts down threads.
        /// </summary>
        public void Shutdown()
        {
            lock (lockObject)
            {
                cancellationToken.Cancel();
            }

            while (numberOfWorkingThreads != 0)
            {
                newTaskAvailable.Set();

                threadStopped.WaitOne();
            }
        }

        private class MyTask<TResult> : IMyTask<TResult>
        {
            /// <summary>
            /// Indicates whether the task is completed.
            /// </summary>
            public bool IsCompleted { get; private set; } = false;

            /// <summary>
            /// Result of the task.
            /// </summary>
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

            private ManualResetEvent isCompletedEvent = new ManualResetEvent(false);

            private MyThreadPool myThreadPool;

            private Queue<Action> localTaskQueue = new Queue<Action>();

            private object queueLockObject = new object();

            public MyTask(Func<TResult> supplier, MyThreadPool myThreadPool)
            {
                this.supplier = supplier;
                this.myThreadPool = myThreadPool;
            }

            /// <summary>
            /// Calculates the task.
            /// </summary>
            public void Calculate()
            {
                try
                {
                    result = supplier();
                }
                catch (Exception exception)
                {
                    aggregateException = new AggregateException(exception);
                }
                finally
                {
                    supplier = null;

                    lock (queueLockObject)
                    {
                        IsCompleted = true;
                        isCompletedEvent.Set();

                        while (localTaskQueue.Count != 0)
                        {
                            myThreadPool.EnqueueTask(localTaskQueue.Dequeue());
                        }
                    }
                }
            }

            /// <summary>
            /// Creates a new task which is applied to the result of this task. The new task is accepted for execution.
            /// </summary>
            /// <typeparam name="TNewResult">Type of the new task result.</typeparam>
            /// <param name="newSupplier">Function on the basis of which the new task is going to be created.</param>
            /// <returns>New task which is applied to the result of this task.</returns>
            public IMyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult, TNewResult> newSupplier)
            {
                if (myThreadPool.cancellationToken.Token.IsCancellationRequested)
                {
                    throw new InvalidOperationException("Thread pool has been closed.");
                }

                var newTask = new MyTask<TNewResult>(() => newSupplier(Result), myThreadPool);

                lock (queueLockObject)
                {
                    if (!IsCompleted)
                    {
                        localTaskQueue.Enqueue(newTask.Calculate);
                    }
                    else
                    {
                        myThreadPool.EnqueueTask(newTask.Calculate);
                    }

                    return newTask;
                }
            }
        }
    }
}
