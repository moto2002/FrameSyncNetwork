// automatically generated, do not modify

namespace Messages
{

using System;
using FlatBuffers;

public sealed class Vec3 : Struct {
  public Vec3 __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public float X { get { return bb.GetFloat(bb_pos + 0); } }
  public float Y { get { return bb.GetFloat(bb_pos + 4); } }
  public float Z { get { return bb.GetFloat(bb_pos + 8); } }

  public static Offset<Vec3> CreateVec3(FlatBufferBuilder builder, float X, float Y, float Z) {
    builder.Prep(4, 12);
    builder.PutFloat(Z);
    builder.PutFloat(Y);
    builder.PutFloat(X);
    return new Offset<Vec3>(builder.Offset);
  }
};


}
