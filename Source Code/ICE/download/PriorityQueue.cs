//-----------------------------------------------------------------------
// <copyright file="PriorityQueue.cs" company="International Monetary Fund">
//   This file is part of "Information Connections Engine". See more information at http://ICEdotNet.codeplex.com
//
//   "Information Connections Engine" is free software: you can redistribute it and/or modify
//   it under the terms of the GNU General Public License as published by
//   the Free Software Foundation, either version 2 of the License, or
//   (at your option) any later version.
//
//   "Information Connections Engine" is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//   GNU General Public License for more details.
//
//   You should have received a copy of the GNU General Public License
//   along with "Information Connections Engine".  If not, see http://www.gnu.org/license.
// </copyright>
// <authors>
//      Poirot Clément (Project Officer)
// </authors>
// <context>
//      for the benefit of the the Open Source Community
// </context>
// <supervisors>
//      Hervé Tourpe (Team Leader)
//      Jeffrey Hatton (Project Manager)
// </supervisors>
// <years>2008 - 2009</years>
// <contributors>
//      <!-- any contributors (except for authors) to this file should be listed here -->
// </contributors>
//-----------------------------------------------------------------------

namespace ICE.download
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Ink;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Windows.Shapes;
    
    /// <summary>
    /// This class is a queue with priority
    /// </summary>
    /// <typeparam name="T">IPrioritized type</typeparam>
    /// <remarks>This class must stay thread-safe</remarks>
    public class PriorityQueue<T> where T : ICE.download.IPrioritized
    {
        /// <summary>
        /// This is the different queues (one for each priority encountered)
        /// </summary>
        private List<KeyValuePair<ICE.download.Priority, Queue<T>>> queues = new List<KeyValuePair<Priority, Queue<T>>>();

        /// <summary>
        /// Gets the number of item in the queue
        /// </summary>
        public int Count
        {
            get
            {
                int result = 0;

                foreach (KeyValuePair<ICE.download.Priority, Queue<T>> queue in this.queues)
                {
                    result += queue.Value.Count;
                }

                return result;
            }
        }

        /// <summary>
        /// This function adds an item to the prioritized queue
        /// </summary>
        /// <param name="item">item to add</param>
        /// <remarks>This function must stay thread-safe</remarks>
        public void Enqueue(T item)
        {
            // create a new  sub queue if necessary
            bool queueExist = false;

            foreach (KeyValuePair<ICE.download.Priority, Queue<T>> queue in this.queues)
            {
                queueExist |= queue.Key == item.Priority;
            }

            if (!queueExist)
            {
                KeyValuePair<ICE.download.Priority, Queue<T>> newQueue = new KeyValuePair<Priority, Queue<T>>(item.Priority, new Queue<T>());
                this.queues.Add(newQueue);
            }

            // enqueue the object
            IEnumerable<KeyValuePair<ICE.download.Priority, Queue<T>>> sortedList = from queue in this.queues orderby queue.Key ascending select queue;
            bool itemIsInQueue = false;

            foreach (KeyValuePair<ICE.download.Priority, Queue<T>> queue in sortedList)
            {
                if (queue.Value.Contains(item))
                {
                    if (itemIsInQueue)
                    {
                        // the new item has been added with a more important priotity
                        lock (this)
                        {
                            this.RemoveItem(item, queue);
                        }
                    }
                    else
                    {
                        itemIsInQueue = true;
                    }
                }

                if (queue.Key == item.Priority && !itemIsInQueue)
                {
                    // no item with a better priority has been detected

                    // Queue<T> is not a thread-safe object
                    lock (this)
                    {
                        queue.Value.Enqueue(item);
                        itemIsInQueue = true;
                    }
                }
            }
        }

        /// <summary>
        /// This function returns the item on the top of the queue.
        /// </summary>
        /// <returns>the next item</returns>
        /// <remarks>This function must stay thread-safe</remarks>
        public T Dequeue()
        {
            IEnumerable<KeyValuePair<ICE.download.Priority, Queue<T>>> sortedList = from queue in this.queues orderby queue.Key ascending select queue;

            foreach (KeyValuePair<ICE.download.Priority, Queue<T>> queue in sortedList)
            {
                if (queue.Value.Count >= 1)
                {
                    T item;

                    // Queue<T> is not a thread-safe object
                    lock (this)
                    {
                        item = queue.Value.Dequeue();
                    }

                    return item;
                }
            }

            return default(T);
        }

        /// <summary>
        /// This funtion clean the queue
        /// </summary>
        public void Clear()
        {
            this.queues = new List<KeyValuePair<Priority, Queue<T>>>();
        }

        /// <summary>
        /// this function remove the item in argument from the queue in argument.
        /// </summary>
        /// <param name="item">the item to remove</param>
        /// <param name="queue">a queue of loader and its priority</param>
        private void RemoveItem(T item, KeyValuePair<ICE.download.Priority, Queue<T>> queue)
        {
            int numberOfItem = queue.Value.Count;
            for (int i = 0; i < numberOfItem; i++)
            {
                T otherItem = queue.Value.Dequeue();
                if (!item.Equals(otherItem))
                {
                    queue.Value.Enqueue(otherItem);
                }
            }
        }
    }
}
