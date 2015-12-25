// automatically generated, do not modify

namespace Messages
{

using System;
using FlatBuffers;

public sealed class GenMessage : Table {
  public static GenMessage GetRootAsGenMessage(ByteBuffer _bb) { return GetRootAsGenMessage(_bb, new GenMessage()); }
  public static GenMessage GetRootAsGenMessage(ByteBuffer _bb, GenMessage obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public GenMessage __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public MessageType MsgType { get { int o = __offset(4); return o != 0 ? (MessageType)bb.GetInt(o + bb_pos) : MessageType.Rpc; } }
  public string PId { get { int o = __offset(6); return o != 0 ? __string(o + bb_pos) : null; } }
  public ArraySegment<byte>? GetPIdBytes() { return __vector_as_arraysegment(6); }
  public int MsgId { get { int o = __offset(8); return o != 0 ? bb.GetInt(o + bb_pos) : (int)0; } }
  public byte GetBuf(int j) { int o = __offset(10); return o != 0 ? bb.Get(__vector(o) + j * 1) : (byte)0; }
  public int BufLength { get { int o = __offset(10); return o != 0 ? __vector_len(o) : 0; } }
  public ArraySegment<byte>? GetBufBytes() { return __vector_as_arraysegment(10); }
  public int Frame { get { int o = __offset(12); return o != 0 ? bb.GetInt(o + bb_pos) : (int)0; } }

  public static Offset<GenMessage> CreateGenMessage(FlatBufferBuilder builder,
      MessageType msgType = MessageType.Rpc,
      StringOffset pIdOffset = default(StringOffset),
      int msgId = 0,
      VectorOffset bufOffset = default(VectorOffset),
      int frame = 0) {
    builder.StartObject(5);
    GenMessage.AddFrame(builder, frame);
    GenMessage.AddBuf(builder, bufOffset);
    GenMessage.AddMsgId(builder, msgId);
    GenMessage.AddPId(builder, pIdOffset);
    GenMessage.AddMsgType(builder, msgType);
    return GenMessage.EndGenMessage(builder);
  }

  public static void StartGenMessage(FlatBufferBuilder builder) { builder.StartObject(5); }
  public static void AddMsgType(FlatBufferBuilder builder, MessageType msgType) { builder.AddInt(0, (int)msgType, 1); }
  public static void AddPId(FlatBufferBuilder builder, StringOffset pIdOffset) { builder.AddOffset(1, pIdOffset.Value, 0); }
  public static void AddMsgId(FlatBufferBuilder builder, int msgId) { builder.AddInt(2, msgId, 0); }
  public static void AddBuf(FlatBufferBuilder builder, VectorOffset bufOffset) { builder.AddOffset(3, bufOffset.Value, 0); }
  public static VectorOffset CreateBufVector(FlatBufferBuilder builder, byte[] data) { builder.StartVector(1, data.Length, 1); for (int i = data.Length - 1; i >= 0; i--) builder.AddByte(data[i]); return builder.EndVector(); }
  public static void StartBufVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(1, numElems, 1); }
  public static void AddFrame(FlatBufferBuilder builder, int frame) { builder.AddInt(4, frame, 0); }
  public static Offset<GenMessage> EndGenMessage(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<GenMessage>(o);
  }
};


}
