using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Utility
{
    class DataUtility
    {
        public static int ReadInt(byte[] buf, int offset)
        {
            unsafe {
                fixed (void* ptr = &buf[offset]) {
                    return ((int*)ptr)[0];
                }
            }
        }
        public static byte[] GetDataBuffer(int len, Func<int, byte> getMethod, int startIndex = 0)
        {
            byte[] ret = new byte[len];
            for (int i = startIndex; i < len + startIndex; i++) {
                ret[i - startIndex] = getMethod(i);
            }
            return ret;
        }

        public static int WriteStructToBytes(object obj, byte[] dest, int offset)
        {
            Type type = obj.GetType();
            if (type == typeof(Vector3))
            {
                Vector3 c = (Vector3)obj;
                unsafe
                {
                    fixed (void* vp = &dest[offset])
                    {
                        ((Vector3*)(vp))[0] = c;
                    }
                }
                return Marshal.SizeOf(type);
            }
            else
            {
                int size = Marshal.SizeOf(type);
                WriteStructToBytes(obj, dest, offset, size);
                return size;
            }
        }

        public static void WriteStructToBytes(object obj, byte[] dest, int offset, int size)
        {
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(obj, structPtr, true);
            Marshal.Copy(structPtr, dest, offset, size);
            Marshal.FreeHGlobal(structPtr);
        }

        public static void WriteStructArrayToBytes(Array obj, byte[] dest, int offset, Type elemType, int elemSize)
        {
            if (elemType == typeof(int)) {
                unsafe {
                    var arr = obj as int[];
                    fixed (void* p = arr)
                    {
                        IntPtr ptr = new IntPtr(p);
                        Marshal.Copy(ptr, dest, offset, elemSize * arr.Length);
                    }
                }
            }
            else if (elemType == typeof(float))
            {
                unsafe {
                    var arr = obj as int[];
                    fixed (void* p = arr)
                    {
                        IntPtr ptr = new IntPtr(p);
                        Marshal.Copy(ptr, dest, offset, elemSize * arr.Length);
                    }
                }
            }
            else if (elemType == typeof(byte))
            {
                unsafe
                {
                    var arr = obj as byte[];
                    fixed (void* p = arr)
                    {
                        IntPtr ptr = new IntPtr(p);
                        Marshal.Copy(ptr, dest, offset, elemSize * arr.Length);
                    }
                }
            }
            else if (elemType == typeof(Vector3))
            {
                unsafe
                {
                    var arr = obj as Vector3[];
                    fixed (void* p = arr)
                    {
                        IntPtr ptr = new IntPtr(p);
                        Marshal.Copy(ptr, dest, offset, elemSize * arr.Length);
                    }
                }
            }
            else if (elemType == typeof(Vector2))
            {
                unsafe
                {
                    var arr = obj as Vector2[];
                    fixed (void* p = arr)
                    {
                        IntPtr ptr = new IntPtr(p);
                        Marshal.Copy(ptr, dest, offset, elemSize * arr.Length);
                    }
                }
            }

            else if (elemType == typeof(Quaternion))
            {
                unsafe
                {
                    var arr = obj as Quaternion[];
                    fixed (void* p = arr)
                    {
                        IntPtr ptr = new IntPtr(p);
                        Marshal.Copy(ptr, dest, offset, elemSize * arr.Length);
                    }
                }
            }
            else {
                var handle = GCHandle.Alloc(obj, GCHandleType.Pinned);
                IntPtr ptr = handle.AddrOfPinnedObject();
                Marshal.Copy(ptr, dest, offset, elemSize * obj.Length);
                handle.Free();
            }
        }

        public static object BytesToStruct(System.Type type, byte[] buf, int offset = 0)
        {
            if (type == typeof(int))
            {
                unsafe { 
                    fixed (void* p = &buf[offset])
                    {
                        return ((int*)p)[0];
                    }
                }
            }
            else if(type == typeof(float)){
                unsafe {
                    fixed (void* p = &buf[offset])
                    {
                        return ((float*)p)[0];
                    }
                }
            }
            else if (type == typeof(UnityEngine.Vector3))
            {
                unsafe
                {
                    fixed (void* p = &buf[offset])
                    {
                        return ((UnityEngine.Vector3*)p)[0];
                    }
                }
            }
            else if (type == typeof(UnityEngine.Vector2))
            {
                unsafe
                {
                    fixed (void* p = &buf[offset])
                    {
                        return ((UnityEngine.Vector2*)p)[0];
                    }
                }
            }
            else if (type == typeof(UnityEngine.Quaternion))
            {
                unsafe
                {
                    fixed (void* p = &buf[offset])
                    {
                        return ((UnityEngine.Quaternion*)p)[0];
                    }
                }
            }
            int size = Marshal.SizeOf(type);
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.Copy(buf, offset, ptr, size);
            var ret = Marshal.PtrToStructure(ptr, type);
            Marshal.FreeHGlobal(ptr);
            return ret;
        }
    }
}
