using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

interface IBufferRW
{
    ArraySegment<byte> Serilize(object[] args);
    object[] Deserilize(ArraySegment<byte> dataBuffSeg);
}

