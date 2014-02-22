using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace CellAO.Core.Pathfinding.General
{
    public class SingletonHolder<T> where T : new()
    {
        private static T m_instance;

        private SingletonHolder()
        {
        }

        public static T Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new T();
                }
                return m_instance;
            }
        }
    }
}
