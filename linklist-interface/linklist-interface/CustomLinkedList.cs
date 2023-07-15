using System;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics.Eventing.Reader;

namespace GenericsConsoleApp
{
    public class CustomLinkedList<T> : ICustomList<T>
    {

        public override string ToString()
        {
            Node currentNode = First;
            String finishedString = "";
            if (currentNode != null)
            {
                while (currentNode.next != null)
                {
                    finishedString += currentNode.d.ToString() + ", ";
                    currentNode = currentNode.next;
                }
                finishedString += currentNode.d.ToString();
            }
            return finishedString;
        }

        //a singly linked node has two fields: A data object and a value pointing to the next node.
        internal class Node
        {
            internal T d;
            internal Node? next;
            internal Node(T data)
            {
                d = data;
                next = null;
            }
        }

        //by default, the list will not have anything in it, so the first node is null.
        internal Node? root = null;

        //we can retrieve the first node with First to make things easier to read.
        internal Node First { get { return root; } }

        //we retrieve the last node by checking each node until the next pointer is null. Then we return the node with a null next.
        internal Node Last
        {
            get
            {
                Node currentNode = First;
                if (currentNode == null)
                {
                    return root;
                }
                while(currentNode.next != null)
                {
                    currentNode = currentNode.next;
                }
                return currentNode;

            }
        }

        public T this[int i] { get
            {
                Node? currentNode = First;
                if (Count == 0)
                {
                    return default(T);
                }
                while(i > 0)
                {
                    currentNode = currentNode.next;
                    i--;
                }
                return currentNode.d;
            }

            set 
            {
                //if someone assigns a node to a big index value with null values in between, we should still account for that.
                Node? currentNode = First;
                if (i == 0)
                {
                    root = new Node(value);
                }

                while(i > 0)
                {

                    //if both the next node is null and i is still bigger than zero, we need to create a new empty node for that position.
                    if (currentNode.next == null && i > 0)
                    {
                        Node node = new Node(value);
                        node.d = default(T);
                        currentNode.next = node;
                        i--;
                    }

                    currentNode = currentNode.next;
                    i--;
                }
                currentNode.d = value;
            }
        
        }

        public int Count
        {
            
            get
            {
                int counter = 0;
                Node? currentNode = First;
                if (currentNode == null)
                {
                    return counter;
                }
                while(currentNode != Last)
                {
                    counter++;
                    currentNode = currentNode.next;
                }
                counter++;
                return counter;
            }
        }

        public void Add(T item)
        {
            if (First == null)
            {
                root = new Node(item);
                return;
            }
            else Last.next = new Node(item);
        }

        public void AddRange(T[] array)
        {
            Node currentNode = Last;
            if(currentNode == null)
            {
                root = new Node(array[0]);
                currentNode = root;
            }
            for (int i = 1; i < array.Length; i++)
            {
                currentNode.next = new Node(array[i]);
                currentNode = currentNode.next;
            }
        }

        public void Clear()
        {
            root = null;
        }

        public bool Contains(T item)
        {
            Node currentNode = First;
         
            while(currentNode != null)
            {
                if(currentNode.d.Equals(item))
                {
                    return true;
                }
                currentNode = currentNode.next;
            }
          
            return false;
        }

        public T[] CopyTo(T[] array)
        {
            T[] newArray = new T[Count];
            Node currentNode = First;
            for(int i = 0; i < Count; i++)
            {
                newArray[i] = currentNode.d;
                currentNode = currentNode.next;
            }
            return newArray;
        }

        public T[] CopyTo(T[] array, int arrayIndex)
        {
            Node currentNode = First;
            for (int i = 0; i < Count; i++)
            {
                array[arrayIndex] = currentNode.d;
                currentNode = currentNode.next;
                arrayIndex++;
            }
            return array;
        }

        public T[] CopyTo(int index, T[] array, int arrayIndex, int count)
        {
            Node currentNode = First;
            int indexer = 0;
            while(indexer < index)
            {
                currentNode = currentNode.next;
                indexer++;
            }
            for(int i = 0; i < count; i++)
            {
                array[arrayIndex] = currentNode.d;
                currentNode = currentNode.next;
                arrayIndex++;
            }
            return array;
        }

        public bool Exists(Predicate<T> match)
        {
            Node currentNode = First;
            while(currentNode != null)
            {
                if (match(currentNode.d))
                {
                    return true;
                }
                currentNode = currentNode.next;

            }
            return false;
        }

        public T? Find(Predicate<T> match)
        {
            Node currentNode = First;
            while (currentNode != null)
            {
                if (match(currentNode.d))
                {
                    return currentNode.d;
                }
                currentNode = currentNode.next;

            }
            return default(T);
        }

        public ICustomList<T> FindAll(Predicate<T> match)
        {
            CustomLinkedList<T> list = new CustomLinkedList<T>();

            Node currentNode = First;
            while (currentNode != null)
            {
                if (match(currentNode.d))
                {
                    list.Add(currentNode.d);
                }
                currentNode = currentNode.next;

            }
            return list;


        }

        public int FindIndex(Predicate<T> match)
        {
            int indexer = 0;

            Node currentNode = First;
            while (currentNode != null)
            {
                if (match(currentNode.d))
                {
                    return indexer;
                }
                currentNode = currentNode.next;
                indexer++;

            }
            return -1;

        }

        public int FindIndex(int startIndex, Predicate<T> match)
        {
            int indexer = 0;
            Node currentNode = First;
            while (indexer < startIndex)
            {
                currentNode = currentNode.next;
                indexer++;
            }
            while(currentNode != null)
            {
                if (match(currentNode.d))
                {
                    return indexer;
                }
                currentNode = currentNode.next;
                indexer++;
            }
            return -1;
        }

        public int FindIndex(int index, int count, Predicate<T> match)
        {
            throw new NotImplementedException();
        }

        public T FindLast(Predicate<T> match)
        {
            throw new NotImplementedException();
        }

        public int FindLastIndex(Predicate<T> match)
        {
            throw new NotImplementedException();
        }

        public int FindLastIndex(int endIndex, Predicate<T> match)
        {
            throw new NotImplementedException();
        }

        public int FindLastIndex(int endIndex, int count, Predicate<T> match)
        {
            throw new NotImplementedException();
        }

        public void ForEach(Action<T> action)
        {
            throw new NotImplementedException();
        }

        public ICustomList<T> GetRange(int index, int count)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(T item)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(T item, int index)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(T item, int index, int count)
        {
            throw new NotImplementedException();
        }

        public void Insert(T item, int index)
        {
            throw new NotImplementedException();
        }

        public void InsertRange(T[] inputArray, int startIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        public int RemoveAll(T item)
        {
            throw new NotImplementedException();
        }

        public int RemoveAll(Predicate<T> match)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public void RemoveFirst()
        {
            throw new NotImplementedException();
        }

        public void RemoveLast()
        {
            throw new NotImplementedException();
        }

        public ICustomList<T> RemoveRange(int index)
        {
            throw new NotImplementedException();
        }

        public ICustomList<T> RemoveRange(int index, int count)
        {
            throw new NotImplementedException();
        }

        public void Reverse()
        {
            throw new NotImplementedException();
        }

        public void Reverse(int startIndex, int count)
        {
            throw new NotImplementedException();
        }

        public bool TrueForAll(Predicate<T> match)
        {
            throw new NotImplementedException();
        }
    }
}
