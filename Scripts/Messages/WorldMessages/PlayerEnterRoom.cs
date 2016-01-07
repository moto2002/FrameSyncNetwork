// automatically generated, do not modify

namespace WorldMessages
{

using System;
using FlatBuffers;

public sealed class PlayerEnterRoom : Table {
  public static PlayerEnterRoom GetRootAsPlayerEnterRoom(ByteBuffer _bb) { return GetRootAsPlayerEnterRoom(_bb, new PlayerEnterRoom()); }
  public static PlayerEnterRoom GetRootAsPlayerEnterRoom(ByteBuffer _bb, PlayerEnterRoom obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public PlayerEnterRoom __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public string RoomId { get { int o = __offset(4); return o != 0 ? __string(o + bb_pos) : null; } }
  public ArraySegment<byte>? GetRoomIdBytes() { return __vector_as_arraysegment(4); }
  public string PlayerId { get { int o = __offset(6); return o != 0 ? __string(o + bb_pos) : null; } }
  public ArraySegment<byte>? GetPlayerIdBytes() { return __vector_as_arraysegment(6); }

  public static Offset<PlayerEnterRoom> CreatePlayerEnterRoom(FlatBufferBuilder builder,
      StringOffset roomIdOffset = default(StringOffset),
      StringOffset playerIdOffset = default(StringOffset)) {
    builder.StartObject(2);
    PlayerEnterRoom.AddPlayerId(builder, playerIdOffset);
    PlayerEnterRoom.AddRoomId(builder, roomIdOffset);
    return PlayerEnterRoom.EndPlayerEnterRoom(builder);
  }

  public static void StartPlayerEnterRoom(FlatBufferBuilder builder) { builder.StartObject(2); }
  public static void AddRoomId(FlatBufferBuilder builder, StringOffset roomIdOffset) { builder.AddOffset(0, roomIdOffset.Value, 0); }
  public static void AddPlayerId(FlatBufferBuilder builder, StringOffset playerIdOffset) { builder.AddOffset(1, playerIdOffset.Value, 0); }
  public static Offset<PlayerEnterRoom> EndPlayerEnterRoom(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<PlayerEnterRoom>(o);
  }
};


}
