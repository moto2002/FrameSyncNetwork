// automatically generated, do not modify

namespace Messages
{

using System;
using FlatBuffers;

public sealed class RpcMsg : Table {
  public static RpcMsg GetRootAsRpcMsg(ByteBuffer _bb) { return GetRootAsRpcMsg(_bb, new RpcMsg()); }
  public static RpcMsg GetRootAsRpcMsg(ByteBuffer _bb, RpcMsg obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public RpcMsg __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public int NetId { get { int o = __offset(4); return o != 0 ? bb.GetInt(o + bb_pos) : (int)0; } }
  public string Method { get { int o = __offset(6); return o != 0 ? __string(o + bb_pos) : null; } }
  public ArraySegment<byte>? GetMethodBytes() { return __vector_as_arraysegment(6); }
  public byte GetArgbuf(int j) { int o = __offset(8); return o != 0 ? bb.Get(__vector(o) + j * 1) : (byte)0; }
  public int ArgbufLength { get { int o = __offset(8); return o != 0 ? __vector_len(o) : 0; } }
  public ArraySegment<byte>? GetArgbufBytes() { return __vector_as_arraysegment(8); }

  public static Offset<RpcMsg> CreateRpcMsg(FlatBufferBuilder builder,
      int netId = 0,
      StringOffset methodOffset = default(StringOffset),
      VectorOffset argbufOffset = default(VectorOffset)) {
    builder.StartObject(3);
    RpcMsg.AddArgbuf(builder, argbufOffset);
    RpcMsg.AddMethod(builder, methodOffset);
    RpcMsg.AddNetId(builder, netId);
    return RpcMsg.EndRpcMsg(builder);
  }

  public static void StartRpcMsg(FlatBufferBuilder builder) { builder.StartObject(3); }
  public static void AddNetId(FlatBufferBuilder builder, int netId) { builder.AddInt(0, netId, 0); }
  public static void AddMethod(FlatBufferBuilder builder, StringOffset methodOffset) { builder.AddOffset(1, methodOffset.Value, 0); }
  public static void AddArgbuf(FlatBufferBuilder builder, VectorOffset argbufOffset) { builder.AddOffset(2, argbufOffset.Value, 0); }
  public static VectorOffset CreateArgbufVector(FlatBufferBuilder builder, byte[] data) { builder.StartVector(1, data.Length, 1); for (int i = data.Length - 1; i >= 0; i--) builder.AddByte(data[i]); return builder.EndVector(); }
  public static void StartArgbufVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(1, numElems, 1); }
  public static Offset<RpcMsg> EndRpcMsg(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<RpcMsg>(o);
  }
};


}
