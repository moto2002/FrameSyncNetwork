// automatically generated, do not modify

namespace Messages
{

using System;
using FlatBuffers;

public sealed class CreateObj : Table {
  public static CreateObj GetRootAsCreateObj(ByteBuffer _bb) { return GetRootAsCreateObj(_bb, new CreateObj()); }
  public static CreateObj GetRootAsCreateObj(ByteBuffer _bb, CreateObj obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public CreateObj __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public string Path { get { int o = __offset(4); return o != 0 ? __string(o + bb_pos) : null; } }
  public ArraySegment<byte>? GetPathBytes() { return __vector_as_arraysegment(4); }
  public Vec3 Pos { get { return GetPos(new Vec3()); } }
  public Vec3 GetPos(Vec3 obj) { int o = __offset(6); return o != 0 ? obj.__init(o + bb_pos, bb) : null; }
  public Quat Rot { get { return GetRot(new Quat()); } }
  public Quat GetRot(Quat obj) { int o = __offset(8); return o != 0 ? obj.__init(o + bb_pos, bb) : null; }

  public static void StartCreateObj(FlatBufferBuilder builder) { builder.StartObject(3); }
  public static void AddPath(FlatBufferBuilder builder, StringOffset pathOffset) { builder.AddOffset(0, pathOffset.Value, 0); }
  public static void AddPos(FlatBufferBuilder builder, Offset<Vec3> posOffset) { builder.AddStruct(1, posOffset.Value, 0); }
  public static void AddRot(FlatBufferBuilder builder, Offset<Quat> rotOffset) { builder.AddStruct(2, rotOffset.Value, 0); }
  public static Offset<CreateObj> EndCreateObj(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<CreateObj>(o);
  }
};


}
