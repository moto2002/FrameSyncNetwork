// automatically generated, do not modify

namespace WorldMessages
{

using System;
using FlatBuffers;

public sealed class WorldMessage : Table {
  public static WorldMessage GetRootAsWorldMessage(ByteBuffer _bb) { return GetRootAsWorldMessage(_bb, new WorldMessage()); }
  public static WorldMessage GetRootAsWorldMessage(ByteBuffer _bb, WorldMessage obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public WorldMessage __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public MessageType Type { get { int o = __offset(4); return o != 0 ? (MessageType)bb.GetInt(o + bb_pos) : MessageType.EnterRoom; } }
  public string PlayerId { get { int o = __offset(6); return o != 0 ? __string(o + bb_pos) : null; } }
  public ArraySegment<byte>? GetPlayerIdBytes() { return __vector_as_arraysegment(6); }
  public byte GetBuff(int j) { int o = __offset(8); return o != 0 ? bb.Get(__vector(o) + j * 1) : (byte)0; }
  public int BuffLength { get { int o = __offset(8); return o != 0 ? __vector_len(o) : 0; } }
  public ArraySegment<byte>? GetBuffBytes() { return __vector_as_arraysegment(8); }
  public int MsgId { get { int o = __offset(10); return o != 0 ? bb.GetInt(o + bb_pos) : (int)0; } }

  public static Offset<WorldMessage> CreateWorldMessage(FlatBufferBuilder builder,
      MessageType type = MessageType.EnterRoom,
      StringOffset playerIdOffset = default(StringOffset),
      VectorOffset buffOffset = default(VectorOffset),
      int msgId = 0) {
    builder.StartObject(4);
    WorldMessage.AddMsgId(builder, msgId);
    WorldMessage.AddBuff(builder, buffOffset);
    WorldMessage.AddPlayerId(builder, playerIdOffset);
    WorldMessage.AddType(builder, type);
    return WorldMessage.EndWorldMessage(builder);
  }

  public static void StartWorldMessage(FlatBufferBuilder builder) { builder.StartObject(4); }
  public static void AddType(FlatBufferBuilder builder, MessageType type) { builder.AddInt(0, (int)type, 1); }
  public static void AddPlayerId(FlatBufferBuilder builder, StringOffset playerIdOffset) { builder.AddOffset(1, playerIdOffset.Value, 0); }
  public static void AddBuff(FlatBufferBuilder builder, VectorOffset buffOffset) { builder.AddOffset(2, buffOffset.Value, 0); }
  public static VectorOffset CreateBuffVector(FlatBufferBuilder builder, byte[] data) { builder.StartVector(1, data.Length, 1); for (int i = data.Length - 1; i >= 0; i--) builder.AddByte(data[i]); return builder.EndVector(); }
  public static void StartBuffVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(1, numElems, 1); }
  public static void AddMsgId(FlatBufferBuilder builder, int msgId) { builder.AddInt(3, msgId, 0); }
  public static Offset<WorldMessage> EndWorldMessage(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<WorldMessage>(o);
  }
};


}
