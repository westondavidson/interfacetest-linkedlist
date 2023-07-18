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
            internal Node(T data,  Node? n)
            {
                d = data;
                next = n;
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
            int indexer = 0;
            Node currentNode = First;
            while (indexer < index)
            {
                currentNode = currentNode.next;
                indexer++;
            }
            while(count > 0)
            {
                if (match(currentNode.d))
                {
                    return indexer;
                }
                currentNode = currentNode.next;
                count--;
                indexer++;
            }
            return -1;
        }

        public T FindLast(Predicate<T> match)
        {
            T item = default(T);
            Node currentNode = First;
            while(currentNode != null)
            {
                if (match(currentNode.d))
                {
                    item = currentNode.d;
                }
                currentNode = currentNode.next;
            }
            return item;

        }

        public int FindLastIndex(Predicate<T> match)
        {

            Node currentNode = First;
            int indexer = 0;
            int indexOfLast = -1;
            while (currentNode != null)
            {
                if (match(currentNode.d))
                {
                    indexOfLast = indexer;
                }
                currentNode = currentNode.next;
                indexer++;
            }
            return indexOfLast;
        }

        public int FindLastIndex(int endIndex, Predicate<T> match)
        {
            Node currentNode = First;
            int indexer = 0;
            int indexOfLast = -1;
            while (indexer <= endIndex)
            {
                if (match(currentNode.d))
                {
                    indexOfLast = indexer;
                }
                currentNode = currentNode.next;
                indexer++;
            }
            return indexOfLast;
        }

        public int FindLastIndex(int endIndex, int count, Predicate<T> match)
        {
            Node currentNode = First;
            int indexer = 0;
            int indexOfLast = -1;
            while (indexer < endIndex - count + 1)
            {
                currentNode = currentNode.next;
                indexer++;
            }
            while (indexer <= endIndex)
            {
                if (match(currentNode.d))
                {
                    indexOfLast = indexer;
                }
                currentNode = currentNode.next;
                indexer++;
            }
            return indexOfLast;
        }

        public void ForEach(Action<T> action)
        {
            Node currentNode = First;
            while(currentNode != null)
            {
                action(currentNode.d);
                currentNode = currentNode.next;
            }
        }

        public ICustomList<T> GetRange(int index, int count)
        {
            CustomLinkedList<T> list = new CustomLinkedList<T>();
            Node currentNode = First;
            int indexer = 0;
            while(indexer < index)
            {
                currentNode = currentNode.next;
                indexer++;
            }
            while(count > 0)
            {
                list.Add(currentNode.d);
                currentNode = currentNode.next;
                count--;
            }
            return list;
        }

        public int IndexOf(T item)
        {
            int indexer = 0;
            Node currentNode = First;
            while(currentNode != null)
            {
                if (currentNode.d.Equals(item))
                {
                    return indexer;
                }
                currentNode = currentNode.next;
                indexer++;
            }
            return -1;
        }

        public int IndexOf(T item, int index)
        {
            int indexer = 0;
            Node currentNode = First;
            while(indexer < index)
            {
                currentNode = currentNode.next;
                indexer++;
            }
            while (currentNode != null)
            {
                if (currentNode.d.Equals(item))
                {
                    return indexer;
                }
                currentNode = currentNode.next;
                indexer++;
            }
            return -1;
        }

        public int IndexOf(T item, int index, int count)
        {
            int indexer = 0;
            Node currentNode = First;
            while (indexer < index)
            {
                currentNode = currentNode.next;
                indexer++;
            }
            while (count > 0)
            {
                if (currentNode.d.Equals(item))
                {
                    return indexer;
                }
                currentNode = currentNode.next;
                indexer++;
                count--;
            }
            return -1;
        }

        public void Insert(T item, int index)
        {
            int indexer = 0;
            Node currentNode = First;
            Node nodeStore = null;
            if(index == 0)
            {
                Node node = new Node(item);
                node.next = currentNode;
                root = node;
                return;
            }
            while(indexer < index-1)
            {
                currentNode = currentNode.next;
                indexer++;
            }
            nodeStore = currentNode.next;
            currentNode.next = new Node(item);
            currentNode.next.next = nodeStore;
        }

        public void InsertRange(T[] inputArray, int startIndex)
        {
            //need to make sure we're assigning in the right scope here.
            int indexer = 0;
            Node currentNode = First;
            //store the current root/beginning node in a nodestore
            Node nodeStore = root;
            //create a new node given the starting value of the input array
            Node newNode = new Node(inputArray[0]);
            if(startIndex == 0)
            {
                //if the provided start index is at 0, assign the root value to the new node.
                root = newNode;
                //for each of the other values in the array, link them to the new node.
                for (int i = 1; i < inputArray.Length; i++)
                {
                    newNode.next = new Node(inputArray[i]);
                    newNode = newNode.next;
                }
                //after all the new nodes have been linked, connnect the last node to the beginning of the old node store.
                Last.next = nodeStore;
                return;
            }
            //we need to store the first values up to when it breaks, and then link them to the new values at the start index
            //then, after the input array finishes, we need to link the last value to the previous node that was stored there.

            while(indexer < startIndex-1)
            {
                currentNode = currentNode.next;
                indexer++;
            }
            nodeStore = currentNode.next;
            currentNode.next = new Node(inputArray[0]);
            currentNode = currentNode.next;
            for(int i = 1; i < inputArray.Length; i++)
            {
                currentNode.next = new Node(inputArray[i]);
                currentNode = currentNode.next;
            }
            Last.next = nodeStore;

        }

        public bool Remove(T item)
        {

            Node currentNode = First;
            if (First.d.Equals(item))
            {
                root = currentNode.next;
                return true;
            }
            while(currentNode != null)
            {
                if (currentNode.next.d.Equals(item))
                {
                    //remove it and return;
                    currentNode.next = currentNode.next.next;

                    return true;
                }
                currentNode = currentNode.next;

            }
            return false;
        }

        public int RemoveAll(T item)
        {
            int counter = 0;
            Node currentNode = First;

            while (currentNode != null)
            {
                if (First.d.Equals(item))
                {
                    root = currentNode.next;
                    counter++;
                }
                if (currentNode.next.d.Equals(item))
                {

                    //remove it, and also check if the next item is also the same value;
                    //if a null reference is thrown, that means we've reached the end of the list anyways, so it's fine. We'll just return the counter.
                    try
                    {
                        while (currentNode.next.d.Equals(item))
                        {
                            currentNode.next = currentNode.next.next;
                            counter++;
                        }
                    } catch(NullReferenceException e)
                    {
                        return counter;
                    }

                }
                currentNode = currentNode.next;

            }
            return counter;
        }

        public int RemoveAll(Predicate<T> match)
        {
            int counter = 0;
            Node currentNode = First;

            while (currentNode.next != null)
            {
                if (match(First.d))
                {
                    root = currentNode.next;
                    counter++;
                }
                if (match(currentNode.next.d))
                {

                    //remove it, and also check if the next item is also the same value;
                    //if a null reference is thrown, that means we've reached the end of the list anyways, so it's fine. We'll just return the counter.
                    try
                    {
                        while (match(currentNode.next.d))
                        {
                            currentNode.next = currentNode.next.next;
                            counter++;
                        }
                    }
                    catch (NullReferenceException e)
                    {
                        return counter;
                    }

                }
                currentNode = currentNode.next;

            }
            return counter;
        }

        public void RemoveAt(int index)
        {
            int counter = 0;
            Node currentNode = First;
            if(index == 0)
            {
                root = root.next;
            }
            while(counter < index-1)
            {
                currentNode = currentNode.next;
                counter++;
            }
            try
            {
                currentNode.next = currentNode.next.next;
            } catch(NullReferenceException e)
            {
                currentNode.next = null;
            }
        }

        public void RemoveFirst()
        {
            root = root.next;
        }

        public void RemoveLast()
        {
            Node currentNode = First;
            while(currentNode.next.next != null)
            {
                currentNode = currentNode.next;
            }
            currentNode.next = null;
        }

        public ICustomList<T> RemoveRange(int index)
        {
            Node currentNode = First;
            int counter = 0;
            if(index == 0)
            {
                this.Clear();
                return this;
            }
            while(counter < index - 1)
            {
                currentNode = currentNode.next;
                counter++;
            }
            currentNode.next = null;
            return this;
        }

        public ICustomList<T> RemoveRange(int index, int count)
        {
            CustomLinkedList<T> list = new CustomLinkedList<T>();
            //store the further iterating node in another node object
            Node currentNode = First;
            Node storedNode = currentNode;
            int counter = 0;
            if (index == 0)
            {
                while(count > 0)
                {
                    list.Add(currentNode.d);
                    currentNode = currentNode.next;
                    count--;

                }
                root = currentNode;
                return list;
            }
            while (counter < index-1)
            {
                currentNode = currentNode.next;
                counter++;
            }
            storedNode = currentNode;
            while(count >= 0)
            {
 
                storedNode = storedNode.next;
                count--;
                try
                {

                    list.Add(storedNode.d);

                }catch (NullReferenceException e)
                {
                    break;
                }
            }
            currentNode.next = storedNode;
            return list;
        }

        public void Reverse()
        {
            Node currentNode = First;
            Node previousNode = null;
            Node nextNode = null;
            while(currentNode != null)
            {
                //store the next node in the list undernextNode
                //we want to store this somewhere so we can access it after the next node is assigned to previous node
                nextNode = currentNode.next;
                //assign the next value of this current node to whatever is stored in the
                //previous node
                //by making the reference in this direction, we reverse the list
                currentNode.next = previousNode;
                //assign the old current node to previous node for next loop
                previousNode = currentNode;
                //move on to the next node in the original list to continue reversal
                currentNode = nextNode;

            }
            //once all links have been updated, assign the final previouNode to root
            //at this point, all nodes have now had their tails pointed to the
            //previous node in the list, reversing flow.
            root = previousNode;
        }

        public void Reverse(int startIndex, int count)
        {
            //here, we do something similar but just traverse and assign normally outside of the
            //window of startindex through the count.
            //actually I'm going to try a recursive method and see if that's easier
            Node currentNode = First;
            Node nextNode = null;
            Node beginningPositionStore = null;
            Node endPositionStore = null;
            Stack<Node> stack = new Stack<Node>();
            int countTracker = count;
            int indexer = 0;
            //find the starting index of the linked list
            while (indexer < startIndex-1)
            {
                currentNode = currentNode.next;
                indexer++;
            }
            //store the beginning of the linked list in a tracker node
            beginningPositionStore = currentNode;

            //find the end of the total count, or where the list will need to resume.
            while (countTracker != 0)
            {
                currentNode = currentNode.next;
                countTracker--;
            }
            //store the end position/resume position in endpositionstore
            endPositionStore = currentNode.next;
            //reassign the current node to beginning position
            currentNode = beginningPositionStore;

            countTracker = count;
            //push all nodes into a stack
            while(countTracker > 0)
            {
                stack.Push(currentNode.next);
                currentNode = currentNode.next;
                countTracker--;
            }
            countTracker = count;
            //once all nodes are stored, pop the values back into the list
            //from the starting position
            currentNode = beginningPositionStore;
            while(countTracker > 0)
            {
                currentNode.next = stack.Pop();
                currentNode = currentNode.next;
                countTracker--;
            }
            //assign the final next to null so there aren't any circular references. If you don't, it will try to point to one of the earlier nodes in the list.
            currentNode.next = null;
            //finally, assign the final node next to the end position that was stored.
            //if the endposition is null, just don't assign anything (?)
            if(endPositionStore !=null)
            {
                currentNode.next = endPositionStore;
            }

            

            
        }

        public bool TrueForAll(Predicate<T> match)
        {
            throw new NotImplementedException();
        }
    }
}
