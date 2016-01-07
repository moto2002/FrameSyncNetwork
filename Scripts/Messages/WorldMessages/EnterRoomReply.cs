// automatically generated, do not modify

namespace WorldMessages
{

using System;
using FlatBuffers;

public sealed class EnterRoomReply : Table {
  public static EnterRoomReply GetRootAsEnterRoomReply(ByteBuffer _bb) { return GetRootAsEnterRoomReply(_bb, new EnterRoomReply()); }
  public static EnterRoomReply GetRootAsEnterRoomReply(ByteBuffer _bb, EnterRoomReply obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public EnterRoomReply __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public string GetPlayers(int j) { int o = __offset(4); return o != 0 ? __string(__vector(o) + j * 4) : null; }
  public int PlayersLength { get { int o = __offset(4); return o != 0 ? __vector_len(o) : 0; } }
  public EnterRoomResult Result { get { int o = __offset(6); return o != 0 ? (EnterRoomResult)bb.GetInt(o + bb_pos) : EnterRoomResult.Ok; } }

  public static Offset<EnterRoomReply> CreateEnterRoomReply(FlatBufferBuilder builder,
      VectorOffset playersOffset = default(VectorOffset),
      EnterRoomResult result = EnterRoomResult.Ok) {
    builder.StartObject(2);
    EnterRoomReply.AddResult(builder, result);
    EnterRoomReply.AddPlayers(builder, playersOffset);
    return EnterRoomReply.EndEnterRoomReply(builder);
  }

  public static void StartEnterRoomReply(FlatBufferBuilder builder) { builder.StartObject(2); }
  public static void AddPlayers(FlatBufferBuilder builder, VectorOffset playersOffset) { builder.AddOffset(0, playersOffset.Value, 0); }
  public static VectorOffset CreatePlayersVector(FlatBufferBuilder builder, StringOffset[] data) { builder.StartVector(4, data.Length, 4); for (int i = data.Length - 1; i >= 0; i--) builder.AddOffset(data[i].Value); return builder.EndVector(); }
  public static void StartPlayersVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(4, numElems, 4); }
  public static void AddResult(FlatBufferBuilder builder, EnterRoomResult result) { builder.AddInt(1, (int)result, 0); }
  public static Offset<EnterRoomReply> EndEnterRoomReply(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<EnterRoomReply>(o);
  }
};


}
