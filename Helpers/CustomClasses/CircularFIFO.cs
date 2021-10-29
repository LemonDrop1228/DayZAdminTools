using System;
using System.Collections;
using System.Collections.Generic;

namespace DayZTediratorToolz.Helpers
{
    public class CircularFIFO<T> : ICollection<T> , IDisposable
{
    public Queue<T> CircularBuffer;

    /// <summary>
    /// The default initial capacity.
    /// </summary>
    private int capacity = 32;

    /// <summary>
    /// Gets the actual capacity of the FIFO.
    /// </summary>
    public int Capacity
    {
        get { return capacity; }
    }

    /// <summary>
    ///  Initialize a new instance of FIFO class that is empty and has the default initial capacity.
    /// </summary>
    public CircularFIFO()
    {
        CircularBuffer = new();
    }

    /// <summary>
    /// Initialize a new instance of FIFO class that is empty and has the specified initial capacity.
    /// </summary>
    /// <param name="size"> Initial capacity of the FIFO. </param>
    public CircularFIFO(int size)
    {
        capacity = size;
        CircularBuffer = new (capacity);
    }

    /// <summary>
    /// Adds an item to the end of the FIFO.
    /// </summary>
    /// <param name="item"> The item to add to the end of the FIFO. </param>
    public void Add(T item)
    {
        if (this.Count >= this.Capacity)
            Remove();

        CircularBuffer.Enqueue(item);
    }

    /// <summary>
    /// Adds array of items to the end of the FIFO.
    /// </summary>
    /// <param name="item"> The array of items to add to the end of the FIFO. </param>
     public void Add(T[] item)
    {
        int enqueuedSize = 0;
        int remainEnqueueSize = this.Capacity - this.Count;

        for (; (enqueuedSize < item.Length && enqueuedSize < remainEnqueueSize); enqueuedSize++)
            CircularBuffer.Enqueue(item[enqueuedSize]);

        if ((item.Length - enqueuedSize) != 0)
        {
            Remove((item.Length - enqueuedSize));//remaining item size

            for (; enqueuedSize < item.Length; enqueuedSize++)
                CircularBuffer.Enqueue(item[enqueuedSize]);
        }
    }

    /// <summary>
    /// Removes and Returns an item from the FIFO.
    /// </summary>
    /// <returns> Item removed. </returns>
    public T Remove()
    {
        T removedItem = CircularBuffer.Peek();
        CircularBuffer.Dequeue();

        return removedItem;
    }

    /// <summary>
    /// Removes and Returns the array of items form the FIFO.
    /// </summary>
    /// <param name="size"> The size of item to be removed from the FIFO. </param>
    /// <returns> Removed array of items </returns>
    public T[] Remove(int size)
    {
        if (size > CircularBuffer.Count)
            size = CircularBuffer.Count;

        T[] removedItems = new T[size];

        for (int i = 0; i < size; i++)
        {
            removedItems[i] = CircularBuffer.Peek();
            CircularBuffer.Dequeue();
        }

        return removedItems;
    }

    /// <summary>
    /// Returns the item at the beginning of the FIFO with out removing it.
    /// </summary>
    /// <returns> Item Peeked. </returns>
    public T Peek()
    {
        return CircularBuffer.Peek();
    }

    /// <summary>
    /// Returns the array of item at the beginning of the FIFO with out removing it.
    /// </summary>
    /// <param name="size"> The size of the array items. </param>
    /// <returns> Array of peeked items. </returns>
    public T[] Peek(int size)
    {
        T[] arrayItems = new T[CircularBuffer.Count];
        CircularBuffer.CopyTo(arrayItems, 0);

        if (size > CircularBuffer.Count)
            size = CircularBuffer.Count;

        T[] peekedItems = new T[size];

        Array.Copy(arrayItems, 0, peekedItems, 0, size);

        return peekedItems;
    }

    /// <summary>
    /// Gets the actual number of items presented in the FIFO.
    /// </summary>
    public int Count
    {
        get
        {
            return CircularBuffer.Count;
        }
    }

    /// <summary>
    /// Removes all the contents of the FIFO.
    /// </summary>
    public void Clear()
    {
        CircularBuffer.Clear();
    }

    /// <summary>
    /// Resets and Initialize the instance of FIFO class that is empty and has the default initial capacity.
    /// </summary>
    public void Reset()
    {
        Dispose();
        CircularBuffer = new(capacity);
    }

    #region ICollection<T> Members

    /// <summary>
    /// Determines whether an element is in the FIFO.
    /// </summary>
    /// <param name="item"> The item to locate in the FIFO. </param>
    /// <returns></returns>
    public bool Contains(T item)
    {
        return CircularBuffer.Contains(item);
    }

    /// <summary>
    /// Copies the FIFO elements to an existing one-dimensional array.
    /// </summary>
    /// <param name="array"> The one-dimensional array that have at list a size of the FIFO </param>
    /// <param name="arrayIndex"></param>
    public void CopyTo(T[] array, int arrayIndex)
    {
        if (array.Length >= CircularBuffer.Count)
            CircularBuffer.CopyTo(array, 0);
    }

    public bool IsReadOnly
    {
        get { return false; }
    }

    public bool Remove(T item)
    {
        return false;
    }

    #endregion

    #region IEnumerable<T> Members

    public IEnumerator<T> GetEnumerator()
    {
       return CircularBuffer.GetEnumerator();
    }

    #endregion

    #region IEnumerable Members

    IEnumerator IEnumerable.GetEnumerator()
    {
        return CircularBuffer.GetEnumerator();
    }

    #endregion

    #region IDisposable Members

    /// <summary>
    /// Releases all the resource used by the FIFO.
    /// </summary>
    public void Dispose()
    {
        CircularBuffer.Clear();
        CircularBuffer = null;
        GC.Collect();
    }

    #endregion
}
}