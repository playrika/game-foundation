using System;
using System.Collections.Generic;
using System.Linq;

namespace Playrika.GameFoundation.Extensions
{
    public static class CollectionExtensions
    {
        private static readonly Random _random = new Random();


        public static T GetRandom<T>(this List<T> items)
        {
            return items[_random.Next(0, items.Count)];
        }

        public static List<T> GetRandom<T>(this List<T> items, int size)
        {
            var randomItems = new List<T>(size);
            var itemsCopy = items.ToList();

            while (itemsCopy.Count > 0 && randomItems.Count < size)
            {
                var randomItem = itemsCopy[_random.Next(0, itemsCopy.Count)];
                itemsCopy.Remove(randomItem);
                randomItems.Add(randomItem);
            }

            if (randomItems.Count >= size)
                return randomItems;

            while (items.Count > 0 && randomItems.Count < size)
            {
                var randomItem = items[_random.Next(0, items.Count)];
                randomItems.Add(randomItem);
            }

            return randomItems;
        }


        public static KeyValuePair<T, T2> GetRandom<T, T2>(this Dictionary<T, T2> items)
        {
            return items.ToList()[_random.Next(0, items.Count)];
        }

        public static Dictionary<T, T2> GetRandom<T, T2>(this Dictionary<T, T2> items, int size)
        {
            var randomItems = new Dictionary<T, T2>(size);
            var itemsCopy = items.ToList();

            while (itemsCopy.Count > 0 && randomItems.Count < size)
            {
                var randomItem = itemsCopy[_random.Next(0, itemsCopy.Count)];
                itemsCopy.Remove(randomItem);
                randomItems.Add(randomItem.Key, randomItem.Value);
            }

            return randomItems;
        }
        

        public static void Shuffle<T>(this List<T> items)
        {
            var itemsCount = items.Count;

            while (itemsCount > 1)
            {
                itemsCount--;
                var index = _random.Next(itemsCount + 1);
                var value = items[index];
                items[index] = items[itemsCount];
                items[itemsCount] = value;
            }
        }
    }
}