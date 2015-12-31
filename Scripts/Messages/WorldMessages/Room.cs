// automatically generated, do not modify

namespace WorldMessages
{

using System;
using FlatBuffers;

public sealed class Room : Table {
  public static Room GetRootAsRoom(ByteBuffer _bb) { return GetRootAsRoom(_bb, new Room()); }
  public static Room GetRootAsRoom(ByteBuffer _bb, Room obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public Room __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public string Id { get { int o = __offset(4); return o != 0 ? __string(o + bb_pos) : null; } }
  public ArraySegment<byte>? GetIdBytes() { return __vector_as_arraysegment(4); }
  public int PlayerCount { get { int o = __offset(6); return o != 0 ? bb.GetInt(o + bb_pos) : (int)0; } }
  public int Capacity { get { int o = __offset(8); return o != 0 ? bb.GetInt(o + bb_pos) : (int)0; } }

  public static Offset<Room> CreateRoom(FlatBufferBuilder builder,
      StringOffset idOffset = default(StringOffset),
      int playerCount = 0,
      int capacity = 0) {
    builder.StartObject(3);
    Room.AddCapacity(builder, capacity);
    Room.AddPlayerCount(builder, playerCount);
    Room.AddId(builder, idOffset);
    return Room.EndRoom(builder);
  }

  public static void StartRoom(FlatBufferBuilder builder) { builder.StartObject(3); }
  public static void AddId(FlatBufferBuilder builder, StringOffset idOffset) { builder.AddOffset(0, idOffset.Value, 0); }
  public static void AddPlayerCount(FlatBufferBuilder builder, int playerCount) { builder.AddInt(1, playerCount, 0); }
  public static void AddCapacity(FlatBufferBuilder builder, int capacity) { builder.AddInt(2, capacity, 0); }
  public static Offset<Room> EndRoom(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<Room>(o);
  }
};


}
