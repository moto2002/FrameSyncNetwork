using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlatBuffers;
namespace Utility
{
    public static class ByteBufferExtension
    {
        public static ArraySegment<byte> GetArraySegment(this ByteBuffer bb)
        {
            return new ArraySegment<byte>(bb.Data, bb.Position, bb.Length - bb.Position);
        }
    }

    public static class FlatBufferBuilderExtension
    {
        public static FlatBuffers.VectorOffset CreateBuffVector(this FlatBuffers.FlatBufferBuilder builder, System.Action<FlatBuffers.FlatBufferBuilder, int> startVecAction, ArraySegment<byte> buffSeg)
        {
            startVecAction(builder, buffSeg.Count);
            for (int i = buffSeg.Offset + buffSeg.Count - 1; i >= buffSeg.Offset; i--)
            {
                builder.AddByte(buffSeg.Array[i]);
            }
            return builder.EndVector();
        }
    }
}
