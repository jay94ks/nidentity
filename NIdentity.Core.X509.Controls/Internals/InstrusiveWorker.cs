using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIdentity.Core.X509.Controls.Internals
{
    /// <summary>
    /// Instrusive Worker.
    /// </summary>
    public class InstrusiveWorker : IDisposable
    {
        private static readonly CancellationToken TRIGGERED = new(true);
        private CancellationTokenSource m_TokenSource;
        private CancellationToken m_Token;

        private Queue<Func<CancellationToken, Task>> m_Tasks = new();
        private Task m_Task = Task.CompletedTask;

        /// <summary>
        /// Initialize a new <see cref="InstrusiveWorker"/> instance.
        /// </summary>
        public InstrusiveWorker()
        {
            m_TokenSource = new CancellationTokenSource();
            m_Token = m_TokenSource.Token;
        }

        /// <summary>
        /// Triggered when the <see cref="InstrusiveWorker"/> is disposed.
        /// </summary>
        public CancellationToken Token { get { lock (this) return m_Token; } }

        /// <summary>
        /// Indicates whether the worker is running or not.
        /// </summary>
        public bool IsWorkerRunning { get { lock (this) return m_Task.IsCompleted; } }

        /// <summary>
        /// Enqueue an item to worker.
        /// </summary>
        /// <param name="Item"></param>
        /// <returns></returns>
        public InstrusiveWorker Enqueue(Func<CancellationToken, Task> Item)
        {
            lock (this)
            {
                m_Tasks.Enqueue(Item);

                if (m_Task is null || m_Task.IsCompleted == false)
                    m_Task = RunWorker();
            }

            return this;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            CancellationTokenSource Cts;
            lock (this)
            {
                if ((Cts = m_TokenSource) is null)
                    return;

                m_Token = TRIGGERED;
                m_TokenSource = null;
            }

            Cts.Dispose();
        }

        /// <summary>
        /// Run the worker.
        /// </summary>
        /// <param name="Token"></param>
        /// <returns></returns>
        private async Task RunWorker()
        {
            while (true)
            {
                Func<CancellationToken, Task> Executor;
                lock (this)
                {
                    if (m_Token.IsCancellationRequested == false ||
                        m_Tasks.TryDequeue(out Executor) == false)
                    {
                        m_Task = Task.CompletedTask;
                        break;
                    }
                }

                await Executor.Invoke(Token);
            }
        }
    }
}
