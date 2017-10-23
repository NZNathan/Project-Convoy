using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Heap<T> where T: IHeapItem<T> {

    T[] items;
    int currentItemCount;


    public int Count
    {
        get
        {
            return currentItemCount;
        }
    }

    public Heap(int maxHeapSize)
    {
        items = new T[maxHeapSize];
    }

    private void SortUp(T item)
    {
        //Get parent
        int parentIndex = (item.HeapIndex - 1) / 2;

        //While item has a higher pirority than its parent keep moving it up the heap
        while (true)
        {
            T parentItem = items[parentIndex];

            //Compare item to parent item
            if(item.CompareTo(parentItem) > 0)
            {
                Swap(item, parentItem);
            }
            else
            {
                break;
            }
        }
    }

    private void SortDown(T item)
    {
        while (true)
        {
            //Get Children
            int childLeft = item.HeapIndex * 2 + 1;
            int childRight = item.HeapIndex * 2 + 2;

            int swapIndex = 0;

            //If item has children, check its in the right position, else it must be the right position so return
            if (childLeft < currentItemCount)
            {
                swapIndex = childLeft;

                //If item has a right child and it is of higher priority than the left child set it to the swap index
                if (childRight < currentItemCount && items[childLeft].CompareTo(items[childRight]) < 0)
                {
                    swapIndex = childRight;
                }

                if (item.CompareTo(items[swapIndex]) < 0)
                {
                    Swap(item, items[swapIndex]);
                }
                else
                    return;
            }
            else
                return;

        }

    }

    /// <summary>
    /// Updates the item, if its heap value as been changed
    /// </summary>
    /// <param name="item"></param>
    public void UpdateItem(T item)
    {
        SortUp(item);
    }

    public bool Contains(T item)
    {
        return Equals(items[item.HeapIndex], item);
    }

    private void Swap(T itemA, T itemB)
    {
        //Swap items in array
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;

        //Swap items index value
        int itemAIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;
    }

    public void Add(T item)
    {
        item.HeapIndex = currentItemCount;
        items[currentItemCount] = item;
        SortUp(item);
        currentItemCount++;
    }

    public T RemoveFirst()
    {
        T firstItem = items[0];
        currentItemCount--;

        //Take last item in heap and move it to the head
        items[0] = items[currentItemCount];
        items[0].HeapIndex = 0;

        //sort the new item
        SortDown(items[0]);

        //Return the removed item
        return firstItem;
    }
}

public interface IHeapItem<T> : IComparable<T> {

    int HeapIndex
    {
        get;
        set;
    }
}
