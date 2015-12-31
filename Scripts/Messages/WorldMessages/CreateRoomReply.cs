// automatically generated, do not modify

namespace WorldMessages
{

using System;
using FlatBuffers;

public sealed class CreateRoomReply : Table {
  public static CreateRoomReply GetRootAsCreateRoomReply(ByteBuffer _bb) { return GetRootAsCreateRoomReply(_bb, new CreateRoomReply()); }
  public static CreateRoomReply GetRootAsCreateRoomReply(ByteBuffer _bb, CreateRoomReply obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public CreateRoomReply __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public int ErrorCode { get { int o = __offset(4); return o != 0 ? bb.GetInt(o + bb_pos) : (int)0; } }
  public string Id { get { int o = __offset(6); return o != 0 ? __string(o + bb_pos) : null; } }
  public ArraySegment<byte>? GetIdBytes() { return __vector_as_arraysegment(6); }
  public int Capacity { get { int o = __offset(8); return o != 0 ? bb.GetInt(o + bb_pos) : (int)0; } }

  public static Offset<CreateRoomReply> CreateCreateRoomReply(FlatBufferBuilder builder,
      int errorCode = 0,
      StringOffset idOffset = default(StringOffset),
      int capacity = 0) {
    builder.StartObject(3);
    CreateRoomReply.AddCapacity(builder, capacity);
    CreateRoomReply.AddId(builder, idOffset);
    CreateRoomReply.AddErrorCode(builder, errorCode);
    return CreateRoomReply.EndCreateRoomReply(builder);
  }

  public static void StartCreateRoomReply(FlatBufferBuilder builder) { builder.StartObject(3); }
  public static void AddErrorCode(FlatBufferBuilder builder, int errorCode) { builder.AddInt(0, errorCode, 0); }
  public static void AddId(FlatBufferBuilder builder, StringOffset idOffset) { builder.AddOffset(1, idOffset.Value, 0); }
  public static void AddCapacity(FlatBufferBuilder builder, int capacity) { builder.AddInt(2, capacity, 0); }
  public static Offset<CreateRoomReply> EndCreateRoomReply(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<CreateRoomReply>(o);
  }
};


}
