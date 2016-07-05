using System.Collections.Generic;
using System.Linq;

namespace Priority_Queue
{
    //     public class PriorityQueue<T>
    //     {
    //         LinkedList<KeyValuePair<T, float>> Nodes;
    //         public int Count { get { return Nodes.Count; } }
    // 
    //         public PriorityQueue()
    //         {
    //             Nodes = new LinkedList<KeyValuePair<T, float>>();
    //         }
    // 
    //         public void Enqueue(T payload, float priority)
    //         {
    //             KeyValuePair<T, float> payloadPair = new KeyValuePair<T, float>(payload, priority);
    //             LinkedListNode<KeyValuePair<T, float>> node = new LinkedListNode<KeyValuePair<T, float>>(payloadPair);
    // 
    //             InsertSorted(node);
    //         } 
    // 
    //         public T Dequeue()
    //         {
    //             T dequeuedValue = GetPayload(Nodes.First);
    //             Nodes.RemoveFirst();
    //             return dequeuedValue;
    //         }
    // 
    //         public bool IsEmpty()
    //         {
    //             return Nodes.Count == 0;
    //         }
    // 
    //         float GetPriority(LinkedListNode<KeyValuePair<T, float>> node)
    //         {
    //             return node.Value.Value;
    //         }
    // 
    //         T GetPayload(LinkedListNode<KeyValuePair<T, float>> node)
    //         {
    //             return node.Value.Key;
    //         }
    // 
    //         void InsertSorted(LinkedListNode<KeyValuePair<T, float>> node)
    //         {
    //             if (Nodes.Count == 0 || GetPriority(node) <= GetPriority(Nodes.First))
    //             {
    //                 Nodes.AddFirst(node);
    //             }
    //             else if(GetPriority(node) > GetPriority(Nodes.Last))
    //             {
    //                 Nodes.AddLast(node);
    //             }
    //             else
    //             {
    //                 LinkedListNode<KeyValuePair<T, float>> currentNode;
    //                 for (currentNode = Nodes.First; currentNode != null; currentNode = currentNode.Next)
    //                 {
    //                     if ((GetPriority(node) > GetPriority(currentNode)) && (GetPriority(node) <= GetPriority(currentNode.Next))) 
    //                     {
    //                         Nodes.AddAfter(currentNode, node);
    //                         break;
    //                     }
    //                 }
    //             }
    //         }
    //     }


    class PriorityQueue<P, V>
    {
        private readonly SortedDictionary<P, Queue<V>> list = new SortedDictionary<P, Queue<V>>();

        public void Enqueue(P priority, V value)
        {
            Queue<V> q;
            if (!list.TryGetValue(priority, out q))
            {
                q = new Queue<V>();
                list.Add(priority, q);
            }
            q.Enqueue(value);
        }

        public V Dequeue()
        {
            // will throw if there isn't any first element!
            var pair = list.First();
            var v = pair.Value.Dequeue();
            if (pair.Value.Count == 0) // nothing left of the top priority.
                list.Remove(pair.Key);
            return v;
        }

        public bool IsEmpty
        {
            get { return !list.Any(); }
        }
    }


}