using System;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using Messages;
using Utility;
class DataPaser
{
    class TypeNotRegisteredException : System.Exception
    {
        string typeName;
        public TypeNotRegisteredException(string typeName)
        {
            this.typeName = typeName;
        }

        public override string Message
        {
            get
            {
                return typeName;
            }
        }
    }
    struct ParseMethod : IBufferRW
    {
        public Func<ArraySegment<byte>, object[]> desMethod;
        public Func<object[], ArraySegment<byte>> serMethod;

        public ArraySegment<byte> Serilize(object[] args)
        {
            return serMethod(args);
        }

        public object[] Deserilize(ArraySegment<byte> buffSegment)
        {
            return desMethod(buffSegment);
        }
    }
    Dictionary<string, IBufferRW> paramDict = new Dictionary<string, IBufferRW>();
    Dictionary<string, System.Func<ArraySegment<byte>, object>> deserializeDict = new Dictionary<string, System.Func<ArraySegment<byte>, object>>();

    static DataPaser m_instance;

    public static DataPaser Instance
    {
        get {
            if (m_instance == null)
                m_instance = new DataPaser();
            return m_instance;
        }
    }

    public void RegisterType<Type>(System.Func<ArraySegment<byte>, object> des)
    {
        var typeName = typeof(Type).FullName;
        deserializeDict[typeName] = des;
    }

    public T Deserialize<T>(ArraySegment<byte> bytes)
    {
        var typeName = typeof(T).FullName;
        System.Func<ArraySegment<byte>, object> fun;
        if (deserializeDict.TryGetValue(typeName, out fun)) {
            return (T)fun(bytes);
        }
        throw new TypeNotRegisteredException(typeName);
    }
    public ArraySegment<byte> SerializeParams(Type type, object[] objs)
    {
        IBufferRW rw;
        if (paramDict.TryGetValue(type.Name, out rw))
        {
            return rw.Serilize(objs);
        }
        throw new TypeNotRegisteredException(type.Name);
    }

    public object[] DeserializeToParams(System.Type type, ArraySegment<byte> buffSegment)
    {
        IBufferRW rw;
        if (paramDict.TryGetValue(type.Name, out rw))
        {
            return rw.Deserilize(buffSegment);
        }
        throw new TypeNotRegisteredException(type.FullName);
    }

    public object[] DeserializeToParams<T>(ArraySegment<byte> buffSegment)
    {
        return DeserializeToParams(typeof(T), buffSegment);
    }

    void BindStructParam<T>() where T : struct
    {
        paramDict[typeof(T).Name] = new ParseMethod() { serMethod = SerStruct<T>, desMethod = DesStruct<T>};
    }

    public void RegisterParam<T>(System.Func<object[], ArraySegment<byte>> ser, System.Func<ArraySegment<byte>, object[]> deser)
    {
        paramDict[typeof(T).Name] = new ParseMethod() { serMethod = ser, desMethod = deser};
    }
    private DataPaser()
    {
        BindStructParam<int>();
        BindStructParam<float>();
        BindStructParam<double>();
        BindStructParam<Vector3>();
        BindStructParam<Quaternion>();
        BindStructParam<Color>();
        RegisterParam<string>((args) => new ArraySegment<byte>(System.Text.Encoding.UTF8.GetBytes(args[0] as string)), (seg) => new object[1]{System.Text.Encoding.UTF8.GetString(seg.Array, seg.Offset, seg.Count)});
        RegisterType<string>((bytes) => System.Text.Encoding.UTF8.GetString(bytes.Array, bytes.Offset, bytes.Count));
        RegisterType<WorldMessages.CreateRoomReply>(bytes => WorldMessages.CreateRoomReply.GetRootAsCreateRoomReply(new FlatBuffers.ByteBuffer(bytes.Array, bytes.Offset)));
        RegisterType<WorldMessages.GetRoomListReply>(bytes => WorldMessages.GetRoomListReply.GetRootAsGetRoomListReply(new FlatBuffers.ByteBuffer(bytes.Array, bytes.Offset)));
    }

    static object[] DesStruct<T>(ArraySegment<byte> seg) where T : struct
    {
        T o = (T)Utility.DataUtility.BytesToStruct(typeof(T), seg.Array, seg.Offset);
        return new object[] { o };
    }


    static ArraySegment<byte> SerStruct<T>(object[] args)
    {
        return new ArraySegment<byte>(StructToBytes(typeof(T), args[0]));
    }
    static byte[] StructToBytes(System.Type type, object o)
    {
        int size = Marshal.SizeOf(type);
        byte[] ret = new byte[size];
        Utility.DataUtility.WriteStructToBytes(o, ret, 0);
        return ret;
    }

    public static byte[] ParamObjectsToBytes(object[] args)
    {
        int length = 0;
        for (int i = 0; i < args.Length; i++)
        {
            var item = args[i];
            if (item is ValueType)
            {
                length += Marshal.SizeOf(item.GetType());
            }else if(item is string){
                var str = item as string;
                length += System.Text.Encoding.UTF8.GetByteCount(str) + sizeof(int);
            }
            else if (item is Array)
            {
                var elemType = item.GetType().GetElementType();
                if (elemType.IsValueType) {
                    int size = Marshal.SizeOf(elemType);
                    length += sizeof(int) + size * (item as Array).Length;
                }
            }
        }
        byte[] ret = new byte[length];
        length = 0;
        for (int i = 0; i < args.Length; i++)
        {
            var item = args[i];
            if (item is ValueType)
            {
                length += Utility.DataUtility.WriteStructToBytes(item, ret, length);
            }
            else if (item is string)
            {
                var str = item as string;
                int strBytelen = System.Text.Encoding.UTF8.GetBytes(str, 0, str.Length, ret, length + sizeof(int));
                Utility.DataUtility.WriteStructToBytes(strBytelen, ret, length, sizeof(int));
                length += strBytelen + sizeof(int);
            }
            else {
                var elemType = item.GetType().GetElementType();
                int size = Marshal.SizeOf(elemType);
                Utility.DataUtility.WriteStructToBytes((item as Array).Length, ret, length, sizeof(int));
                Utility.DataUtility.WriteStructArrayToBytes(item as Array, ret, length + sizeof(int), elemType, size);
                length += size * (item as Array).Length + sizeof(int);
            }
        }
        return ret;
    }

    public static object[] BytesToParams(byte[] bytes, int _offset, System.Reflection.MethodInfo methodInfo)
    {
        var parameters = methodInfo.GetParameters();
        object[] ret = new object[parameters.Length];
		int offset = _offset;
        for (int i = 0; i < parameters.Length; i++)
        {
            var type = parameters[i].ParameterType;
            if (type.IsValueType)
            {
                ret[i] = Utility.DataUtility.BytesToStruct(type, bytes, offset);
                offset += Marshal.SizeOf(type);
            }
            else if (type == typeof(string))
            {
                int strByteLen = Utility.DataUtility.ReadInt(bytes, offset);
                string str = System.Text.Encoding.UTF8.GetString(bytes, offset + sizeof(int), strByteLen);
                ret[i] = str;
                offset += strByteLen + sizeof(int);
            }
            else { //Array of ValueType
                int length = Utility.DataUtility.ReadInt(bytes, offset);
                var elemType = type.GetElementType();
                var array = Array.CreateInstance(elemType, length);
                int elemSize = Marshal.SizeOf(elemType);
                var handle = GCHandle.Alloc(array, GCHandleType.Pinned);
                IntPtr ptr = handle.AddrOfPinnedObject();
                Marshal.Copy(bytes, offset + sizeof(int), ptr, elemSize * length);
                handle.Free();
                ret[i] = array;
                offset += sizeof(int) + elemSize * length;
            }
			if(offset >= bytes.Length)
				throw new ArgumentOutOfRangeException();
        }
        return ret;
    }
}
