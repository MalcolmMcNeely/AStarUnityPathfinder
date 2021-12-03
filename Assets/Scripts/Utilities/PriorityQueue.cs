namespace Utilities
{
    public class PriorityQueue<T> where T : class
    {
        PriorityQueueNode<T> _head;

        class PriorityQueueNode<S> where S : class
        {
            public S Data { get; set; }
            public int Priority { get; set; }
            public PriorityQueueNode<S> Next { get; set; }

            public override string ToString()
            {
                return $"{Priority}:{Data}";
            }
        }

        PriorityQueueNode<T> NewNode(T data, int priority)
        {
            return new PriorityQueueNode<T>
            {
                Data = data,
                Priority = priority,
                Next = null
            };
        }

        public T Top()
        {
            return _head?.Data;
        }

        public T Pop()
        {
            if (_head is null)
            {
                return null;
            }

            var temp = _head;
            _head = _head.Next;
            return temp.Data;
        }

        public void Push(T data, int priority)
        {
            var newNode = NewNode(data, priority);

            if (_head is null || _head.Priority > priority)
            {
                newNode.Next = _head;
                _head = newNode;
            }
            else
            {
                var currentNode = _head;

                while (currentNode.Next != null && currentNode.Next.Priority < priority)
                {
                    currentNode = currentNode.Next;
                }

                newNode.Next = currentNode.Next;
                currentNode.Next = newNode;
            }
        }

        public bool IsEmpty()
        {
            return _head is null;
        }

        public bool Contains(T data)
        {
            if (_head is null)
            {
                return false;
            }

            var currentNode = _head;

            if (currentNode.Data.Equals(data))
            {
                return true;
            }

            while (currentNode.Next != null)
            {
                currentNode = currentNode.Next;

                if (currentNode.Data.Equals(data))
                {
                    return true;
                }
            }

            return false;
        }
    }
}