using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSF.Threading;

namespace GSF.Threading
{
    public class WeakActionAsync<T> : IDisposable
    {
        private WeakAction<T> m_remoteCallback;
        private Action<T> m_localCallback;
        public WeakActionAsync(Action<T> method)
        {
            m_remoteCallback = new WeakAction<T>(method);
            m_localCallback = Callback;
        }

        /// <summary>
        /// This is the item that should be passed to the <see cref="Task.ContinueWith"/> call.
        /// </summary>
        public Action<T> AsyncCallback => m_localCallback;

        private void Callback(T item)
        {
            m_remoteCallback.TryInvoke(item);
        }

        public void Dispose()
        {
            m_remoteCallback.Clear();
        }

    }
}
