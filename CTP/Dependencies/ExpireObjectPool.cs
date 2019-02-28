using System;
using System.Collections.Generic;
using System.Text;
using GSF;
using GSF.Diagnostics;
using GSF.Threading;

namespace CTP.Collection
{
    public class ExpireObjectPool<T>
        where T : class
    {
        private readonly LogPublisher m_log = Logger.CreatePublisher(typeof(ExpireObjectPool<T>), MessageClass.Component);

        private readonly Queue<T> m_queue;
        private readonly Func<T> m_instanceObject;
        private readonly int m_lifeCycleInSeconds;
        private readonly object m_syncRoot;
        private readonly ScheduledTask m_collection;

        private int m_minQueueSize;
        private int m_maxQueueSize;
        private int m_allocations;
        private int m_enqueueCount;
        private int m_dequeueCount;

        /// <summary>
        /// Creates a new Resource Queue.
        /// </summary>
        /// <param name="instance">A delegate that will return the necessary queue.</param>
        /// <param name="lifetimeInSeconds">The age of the object before it is expired.</param>
        public ExpireObjectPool(Func<T> instance, int lifetimeInSeconds)
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));

            m_syncRoot = new object();
            m_lifeCycleInSeconds = lifetimeInSeconds;

            m_instanceObject = instance;
            m_queue = new Queue<T>();
            m_allocations = 0;
            m_enqueueCount = 0;
            m_dequeueCount = 0;
            m_maxQueueSize = 0;
            m_minQueueSize = 0;

            m_collection = new ScheduledTask();
            m_collection.Running += CollectionRunning;
            m_collection.Start(1000 * m_lifeCycleInSeconds);
        }

        private void CollectionRunning(object sender, EventArgs<ScheduledTaskRunningReason> e)
        {
            m_collection.Start(1000 * m_lifeCycleInSeconds);

            StringBuilder sb = new StringBuilder(1000);

            int allocations = 0;
            lock (m_syncRoot)
            {
                //Determines how many items in this list have expired based on time.
                int itemsProcessedPerCollectionCycle = Math.Max(m_enqueueCount, m_dequeueCount);
                int error = Math.Max(4, itemsProcessedPerCollectionCycle >> 2); //Allow 25% error.
                int expiredItemsCount = m_queue.Count - itemsProcessedPerCollectionCycle - error;

                //Determine how many items in this list are never accessed.
                //The minimum size of the queue during this window, minus 25% of the range of the queue.
                int listSizeExcess = m_minQueueSize - ((m_maxQueueSize - m_minQueueSize) >> 2);

                int retireItems = Math.Max(listSizeExcess, expiredItemsCount);

                //Don't shrink the list based on the number of allocations.
                for (int x = m_allocations; x < retireItems; x++)
                {
                    (m_queue.Dequeue() as IDisposable)?.Dispose();
                }

                sb.Append("Allocations: ");
                sb.Append(m_allocations);
                sb.Append(" Enqueue Count: ");
                sb.Append(m_enqueueCount);
                sb.Append(" Dequeue Count: ");
                sb.Append(m_dequeueCount);

                sb.Append(" Max Queue Size: ");
                sb.Append(m_maxQueueSize);

                sb.Append(" Min Queue Size: ");
                sb.Append(m_minQueueSize);

                sb.Append(" Expired Items Count: ");
                sb.Append(expiredItemsCount);

                sb.Append(" List Size Excess: ");
                sb.Append(listSizeExcess);

                allocations = m_allocations;
                m_allocations = 0;
                m_enqueueCount = 0;
                m_dequeueCount = 0;
                m_maxQueueSize = m_queue.Count;
                m_minQueueSize = m_queue.Count;
            }

            if (allocations > 250)
                m_log.Publish(MessageLevel.Error, MessageFlags.PerformanceIssue | MessageFlags.UsageIssue, "Object Pool is likely not optimized", sb.ToString());
            else if (allocations > 50)
                m_log.Publish(MessageLevel.Warning, "Items Created since last collection cycle.", sb.ToString());
            else if (allocations > 0)
                m_log.Publish(MessageLevel.Info, "Items Created since last collection cycle.", sb.ToString());

        }

        /// <summary>
        /// Removes an item from the queue. If one does not exist, one is created.
        /// </summary>
        /// <returns></returns>
        public T Dequeue()
        {
            lock (m_syncRoot)
            {
                if (m_queue.Count > 0)
                {
                    var rv = m_queue.Dequeue();
                    if (m_minQueueSize > m_queue.Count)
                        m_minQueueSize = m_queue.Count;
                    m_dequeueCount++;
                    return rv;
                }
                m_allocations++;
            }
            return m_instanceObject();
        }

        /// <summary>
        /// Adds an item back to the queue.
        /// </summary>
        /// <param name="resource">The resource to queue.</param>
        public void Enqueue(T resource)
        {
            lock (m_syncRoot)
            {
                m_enqueueCount++;
                m_queue.Enqueue(resource);
                if (m_maxQueueSize < m_queue.Count)
                {
                    m_maxQueueSize = m_queue.Count;
                }
            }
        }
    }
}