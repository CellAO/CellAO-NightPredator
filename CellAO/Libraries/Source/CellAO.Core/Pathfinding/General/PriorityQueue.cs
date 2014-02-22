using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace CellAO.Core.Pathfinding.General
{
    public class PriorityQueue<T> where T : IComparable
    {
        private List<T> m_data;

        public PriorityQueue()
        {
            this.m_data = new List<T>();
        }

        public void Enqueue(T queueItem)
        {
            m_data.Add(queueItem);
            m_data.Sort();
        }

        public void Clear()
        {
            m_data.Clear();
        }


        public T Dequeue()
        {
            T frontItem = m_data[0];
            m_data.RemoveAt(0);
            return frontItem;
        }

        public T Peek()
        {
            T frontItem = m_data[0];
            return frontItem;
        }

        public bool Contains(T queueItem)
        {
            return m_data.Contains(queueItem);
        }
        public int Count
        {
            get
            {
                return m_data.Count;
            }
        }
    }
}
