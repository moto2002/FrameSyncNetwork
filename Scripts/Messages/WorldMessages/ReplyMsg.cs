// automatically generated, do not modify

namespace WorldMessages
{

using System;
using FlatBuffers;

public sealed class ReplyMsg : Table {
  public static ReplyMsg GetRootAsReplyMsg(ByteBuffer _bb) { return GetRootAsReplyMsg(_bb, new ReplyMsg()); }
  public static ReplyMsg GetRootAsReplyMsg(ByteBuffer _bb, ReplyMsg obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public ReplyMsg __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public MessageType Type { get { int o = __offset(4); return o != 0 ? (MessageType)bb.GetInt(o + bb_pos) : MessageType.CreateRoom; } }
  public int MsgId { get { int o = __offset(6); return o != 0 ? bb.GetInt(o + bb_pos) : (int)0; } }
  public byte GetBuff(int j) { int o = __offset(8); return o != 0 ? bb.Get(__vector(o) + j * 1) : (byte)0; }
  public int BuffLength { get { int o = __offset(8); return o != 0 ? __vector_len(o) : 0; } }
  public ArraySegment<byte>? GetBuffBytes() { return __vector_as_arraysegment(8); }

  public static Offset<ReplyMsg> CreateReplyMsg(FlatBufferBuilder builder,
      MessageType type = MessageType.CreateRoom,
      int msgId = 0,
      VectorOffset buffOffset = default(VectorOffset)) {
    builder.StartObject(3);
    ReplyMsg.AddBuff(builder, buffOffset);
    ReplyMsg.AddMsgId(builder, msgId);
    ReplyMsg.AddType(builder, type);
    return ReplyMsg.EndReplyMsg(builder);
  }

  public static void StartReplyMsg(FlatBufferBuilder builder) { builder.StartObject(3); }
  public static void AddType(FlatBufferBuilder builder, MessageType type) { builder.AddInt(0, (int)type, 0); }
  public static void AddMsgId(FlatBufferBuilder builder, int msgId) { builder.AddInt(1, msgId, 0); }
  public static void AddBuff(FlatBufferBuilder builder, VectorOffset buffOffset) { builder.AddOffset(2, buffOffset.Value, 0); }
  public static VectorOffset CreateBuffVector(FlatBufferBuilder builder, byte[] data) { builder.StartVector(1, data.Length, 1); for (int i = data.Length - 1; i >= 0; i--) builder.AddByte(data[i]); return builder.EndVector(); }
  public static void StartBuffVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(1, numElems, 1); }
  public static Offset<ReplyMsg> EndReplyMsg(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<ReplyMsg>(o);
  }
};


}
