// automatically generated, do not modify

namespace WorldMessages
{

using System;
using FlatBuffers;

public sealed class GetRoomListReply : Table {
  public static GetRoomListReply GetRootAsGetRoomListReply(ByteBuffer _bb) { return GetRootAsGetRoomListReply(_bb, new GetRoomListReply()); }
  public static GetRoomListReply GetRootAsGetRoomListReply(ByteBuffer _bb, GetRoomListReply obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public GetRoomListReply __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public Room GetRoom(int j) { return GetRoom(new Room(), j); }
  public Room GetRoom(Room obj, int j) { int o = __offset(4); return o != 0 ? obj.__init(__indirect(__vector(o) + j * 4), bb) : null; }
  public int RoomLength { get { int o = __offset(4); return o != 0 ? __vector_len(o) : 0; } }

  public static Offset<GetRoomListReply> CreateGetRoomListReply(FlatBufferBuilder builder,
      VectorOffset RoomOffset = default(VectorOffset)) {
    builder.StartObject(1);
    GetRoomListReply.AddRoom(builder, RoomOffset);
    return GetRoomListReply.EndGetRoomListReply(builder);
  }

  public static void StartGetRoomListReply(FlatBufferBuilder builder) { builder.StartObject(1); }
  public static void AddRoom(FlatBufferBuilder builder, VectorOffset RoomOffset) { builder.AddOffset(0, RoomOffset.Value, 0); }
  public static VectorOffset CreateRoomVector(FlatBufferBuilder builder, Offset<Room>[] data) { builder.StartVector(4, data.Length, 4); for (int i = data.Length - 1; i >= 0; i--) builder.AddOffset(data[i].Value); return builder.EndVector(); }
  public static void StartRoomVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(4, numElems, 4); }
  public static Offset<GetRoomListReply> EndGetRoomListReply(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<GetRoomListReply>(o);
  }
};


}
