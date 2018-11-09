using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityEngine
{
    public class Component : Object
    {
        GameObject m_gameObject = null;
        public GameObject gameObject
        {
            get 
            {
                return m_gameObject;
            }
        }

        public T GetComponent<T>() where T : Component
        {
            return gameObject.GetComponent<T>();
        }
    }
}
