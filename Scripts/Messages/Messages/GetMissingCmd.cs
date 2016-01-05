// automatically generated, do not modify

namespace Messages
{

using System;
using FlatBuffers;

public sealed class GetMissingCmd : Table {
  public static GetMissingCmd GetRootAsGetMissingCmd(ByteBuffer _bb) { return GetRootAsGetMissingCmd(_bb, new GetMissingCmd()); }
  public static GetMissingCmd GetRootAsGetMissingCmd(ByteBuffer _bb, GetMissingCmd obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public GetMissingCmd __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public string Player { get { int o = __offset(4); return o != 0 ? __string(o + bb_pos) : null; } }
  public ArraySegment<byte>? GetPlayerBytes() { return __vector_as_arraysegment(4); }
  public int Frame { get { int o = __offset(6); return o != 0 ? bb.GetInt(o + bb_pos) : (int)0; } }

  public static Offset<GetMissingCmd> CreateGetMissingCmd(FlatBufferBuilder builder,
      StringOffset playerOffset = default(StringOffset),
      int frame = 0) {
    builder.StartObject(2);
    GetMissingCmd.AddFrame(builder, frame);
    GetMissingCmd.AddPlayer(builder, playerOffset);
    return GetMissingCmd.EndGetMissingCmd(builder);
  }

  public static void StartGetMissingCmd(FlatBufferBuilder builder) { builder.StartObject(2); }
  public static void AddPlayer(FlatBufferBuilder builder, StringOffset playerOffset) { builder.AddOffset(0, playerOffset.Value, 0); }
  public static void AddFrame(FlatBufferBuilder builder, int frame) { builder.AddInt(1, frame, 0); }
  public static Offset<GetMissingCmd> EndGetMissingCmd(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<GetMissingCmd>(o);
  }
};


}
