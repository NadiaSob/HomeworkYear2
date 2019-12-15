﻿using System;
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

        private int numberOfThreads;

        //private Object lockObject = new Object();

        private AutoResetEvent newTaskAvailable;

        public MyThreadPool(int numberOfThreads)
        {
            if (numberOfThreads <= 0)
            {
                throw new ArgumentOutOfRangeException("Number of threads should be a positive number.");
            }
            this.numberOfThreads = numberOfThreads;

            taskQueue = new ConcurrentQueue<Action>();
            cancellationToken = new CancellationTokenSource();
            threads = new Thread[numberOfThreads];
            newTaskAvailable = new AutoResetEvent(false);

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

            private readonly ManualResetEvent isCompletedEvent;

            private MyThreadPool myThreadPool;

            public MyTask(Func<TResult> supplier, MyThreadPool myThreadPool)
            {
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
                }
            }

            public IMyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult, TNewResult> newSupplier)
            {
                
            }
        }
    }
}
