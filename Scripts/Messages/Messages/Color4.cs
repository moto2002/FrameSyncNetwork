// automatically generated, do not modify

namespace Messages
{

using System;
using FlatBuffers;

public sealed class Color4 : Struct {
  public Color4 __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public float R { get { return bb.GetFloat(bb_pos + 0); } }
  public float G { get { return bb.GetFloat(bb_pos + 4); } }
  public float B { get { return bb.GetFloat(bb_pos + 8); } }
  public float A { get { return bb.GetFloat(bb_pos + 12); } }

  public static Offset<Color4> CreateColor4(FlatBufferBuilder builder, float R, float G, float B, float A) {
    builder.Prep(4, 16);
    builder.PutFloat(A);
    builder.PutFloat(B);
    builder.PutFloat(G);
    builder.PutFloat(R);
    return new Offset<Color4>(builder.Offset);
  }
};


}
