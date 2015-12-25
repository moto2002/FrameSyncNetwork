// automatically generated, do not modify

namespace Messages
{

using System;
using FlatBuffers;

public sealed class Quat : Struct {
  public Quat __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public float X { get { return bb.GetFloat(bb_pos + 0); } }
  public float Y { get { return bb.GetFloat(bb_pos + 4); } }
  public float Z { get { return bb.GetFloat(bb_pos + 8); } }
  public float W { get { return bb.GetFloat(bb_pos + 12); } }

  public static Offset<Quat> CreateQuat(FlatBufferBuilder builder, float X, float Y, float Z, float W) {
    builder.Prep(4, 16);
    builder.PutFloat(W);
    builder.PutFloat(Z);
    builder.PutFloat(Y);
    builder.PutFloat(X);
    return new Offset<Quat>(builder.Offset);
  }
};


}
