using System;

/// <summary>
/// Heap data structure for faster A* open set calculations
/// </summary>
/// <typeparam name="T"></typeparam>
public class Heap<T> where T : IHeapItem<T>
{
    private T[] _items;
    private int _currentItemCount;
    public int Count => _currentItemCount;

    // init the heap
    public Heap(int maxHeapSize)
    {
        _items = new T[maxHeapSize];
    }

    // adding item and highest priority item sorting it to the top
    // in our case lowest f cost should move up
    public void Add(T item)
    {
        item.HeapIndex = _currentItemCount;
        _items[_currentItemCount] = item;
        SortUp(item);
        _currentItemCount++;
    }

    public bool Contains(T item)
    {
        return Equals(_items[item.HeapIndex], item);
    }

    // Removing the highest priority item and finding next higher priority node
    // so node from end moves to top, and then we check in child nodes to get the highest priority node 
    public T RemoveFirst()
    {
        T firstItem = _items[0];
        _currentItemCount--;
        _items[0] = _items[_currentItemCount];
        _items[0].HeapIndex = 0;
        SortDown(_items[0]);
        return firstItem;
    }

    public void UpdateItem(T item)
    {
        SortUp(item);
    }

    private void SortDown(T item)
    {
        while (true)
        {
            int childLeft = 2 * item.HeapIndex + 1;
            int childRight = 2 * item.HeapIndex + 2;
            int swapIndex = 0;

            if (childLeft < _currentItemCount)
            {
                swapIndex = childLeft;

                if (childRight < _currentItemCount)
                {
                    if (_items[childLeft].CompareTo(_items[childRight]) < 0)
                    {
                        swapIndex = childRight;
                    }
                }

                if (item.CompareTo(_items[swapIndex]) < 0)
                {
                    Swap(item, _items[swapIndex]);
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }
    }
    
    private void SortUp(T item)
    {
        int parentIndex = (item.HeapIndex - 1) / 2;
        while (true)
        {
            T parentItem = _items[parentIndex];
            if (item.CompareTo(parentItem) > 0)
            {
                Swap(item, parentItem);
            }
            else
            {
                break;
            }
            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }

    private void Swap(T itemA, T itemB)
    { 
        _items[itemA.HeapIndex] = itemB;
        _items[itemB.HeapIndex] = itemA;

        int itemIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemIndex;
    }
}
// this is the interface used for comparision and storing of its index inside heap
public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex { get; set; }
}
