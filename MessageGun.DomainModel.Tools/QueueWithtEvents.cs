using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageGun.DomainModel.Tools
{
    public class QueueWithtEvents<T>
    {
        private readonly Queue<T> queue = new Queue<T>();

        public event EventHandler Enqueued;
        public event EventHandler Dequeued;

        protected virtual void OnEnqueued()
        {
            if (Enqueued != null)
                Enqueued(this, new EventArgs());
        }

        protected virtual void OnDequeued()
        {
            if (Dequeued != null)
                Dequeued(this, new EventArgs());
        }


        public virtual void Enqueue(T item)
        {
            queue.Enqueue(item);
            OnEnqueued();
        }

        public int Count
        {
            get => queue.Count;
        }

        public virtual T Dequeue()
        {
            T item = queue.Dequeue();
            OnDequeued();
            return item;
        }

    }
}
