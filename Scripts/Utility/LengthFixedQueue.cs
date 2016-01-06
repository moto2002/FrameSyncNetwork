using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class LenthFixedQueue<T>
{
    private T[] _array;
    private int headPos;
    public int Count
    {
        get;
        private set;
    }
    public int Capacity
    {
        get;
        private set;
    }

    public LenthFixedQueue(int capacity)
    {
        Capacity = capacity;
        _array = new T[capacity];
        headPos = 0;
    }

    public void Clear()
    {
        headPos = 0;
        Count = 0;
    }

    public void Enqueue(T item)
    {
        if (Count < Capacity)
        {
            _array[(headPos + Count) % Capacity] = item;
            Count++;
        }
        else {
            _array[headPos] = item;
            headPos = (headPos + 1) % Capacity;
        }
    }

    public T Dequeue()
    {
        if (Count > 0)
        {
            T ret = _array[headPos];
            headPos = (headPos + 1) % Capacity;
            Count--;
            return ret;
        }
        else
            throw new IndexOutOfRangeException("Empty Queue");
    }

    public bool TryGetEnum(System.Predicate<T> match, out T value)
    {
        for (int i = 0; i < Count; i++) {
            int pos = (headPos + i) % Capacity;
            var item = _array[pos];
            if (match(item)) {
                value = item;
                return true;
            }
        }
        value = default(T);
        return false;
    }
}
