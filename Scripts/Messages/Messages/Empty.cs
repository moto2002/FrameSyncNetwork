// automatically generated, do not modify

namespace Messages
{

using System;
using FlatBuffers;

public sealed class Empty : Struct {
  public Empty __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }


  public static Offset<Empty> CreateEmpty(FlatBufferBuilder builder) {
    builder.Prep(1, 0);
    return new Offset<Empty>(builder.Offset);
  }
};


}
