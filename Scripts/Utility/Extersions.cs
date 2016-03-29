using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Utility
{
    public static class ComponentExtension 
    {
        public static UdpNetBehaviour GetUdpNetwork(this UnityEngine.Component c)
        {
            return c.GetComponent<UdpNetBehaviour>();
        }

        public static UdpNetBehaviour GetUdpNetwork(this UnityEngine.GameObject g)
        {
            return g.GetComponent<UdpNetBehaviour>();
        }
    }
}
