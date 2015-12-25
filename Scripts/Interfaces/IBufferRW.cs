using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


interface IBufferRW
{
    byte[] Serilize(object[] args);
    object[] Deserilize(byte[] bytes);
}

