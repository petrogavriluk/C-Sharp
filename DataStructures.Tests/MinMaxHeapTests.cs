﻿using System;
using System.Collections.Generic;
using System.Linq;
using DataStructures;
using NUnit.Framework;

namespace DataStructures.Tests
{
    [TestFixture()]
    public static class MinMaxHeapTests
    {
        private static readonly object[] collectionsSource = new object[] {
            new int[] { 5, 10, -2, 0, 3, 13, 5, -8, 41, -5, -7, -60, -12 },
            new char[] {'e', '4', 'x', 'D', '!', '$', '-', '_', '2', ')', 'Z', 'q'},
            new string[] { "abc", "abc", "xyz", "bcd", "klm", "opq", "ijk" }
        };


        [Test()]
        public static void CustomComparerTest()
        {
            var arr = new string[] { "aaaa", "c", "dd", "bbb" };
            var comparer = Comparer<string>.Create((a, b) => Comparer<int>.Default.Compare(a.Length, b.Length));

            var mmh = new MinMaxHeap<string>(comparer);
            foreach (var s in arr)
            {
                mmh.Add(s);
            }

            Assert.AreEqual(comparer, mmh.Comparer);
            Assert.AreEqual("c", mmh.GetMin());
            Assert.AreEqual("aaaa", mmh.GetMax());
        }

        [Test()]
        [TestCaseSource("collectionsSource")]
        public static void AddTest<T>(IEnumerable<T> collection)
        {
            var mmh = new MinMaxHeap<T>();
            foreach (var item in collection)
            {
                mmh.Add(item);
            }
            T minValue = mmh.GetMin();
            T maxValue = mmh.GetMax();

            Assert.AreEqual(collection.Min(), minValue);
            Assert.AreEqual(collection.Max(), maxValue);
            Assert.AreEqual(collection.Count(), mmh.Count);
        }

        [Test()]
        [TestCaseSource("collectionsSource")]
        public static void RemoveMaxTest<T>(IEnumerable<T> collection)
        {
            var ordered = collection.OrderByDescending(x => x);
            var mmh = new MinMaxHeap<T>();
            bool res1 = mmh.RemoveMax();
            foreach (var item in collection)
            {
                mmh.Add(item);
            }
            T first = mmh.GetMax();
            bool res2 = mmh.RemoveMax();
            T second = mmh.GetMax();

            Assert.AreEqual(false, res1);
            Assert.AreEqual(true, res2);
            Assert.AreEqual(ordered.ElementAt(0), first);
            Assert.AreEqual(ordered.ElementAt(1), second);
            Assert.AreEqual(collection.Count() - 1, mmh.Count);
        }

        [Test()]
        [TestCaseSource("collectionsSource")]
        public static void RemoveMinTest<T>(IEnumerable<T> collection)
        {
            var ordered = collection.OrderBy(x => x);
            var mmh = new MinMaxHeap<T>();

            bool res1 = mmh.RemoveMin();
            foreach (T item in collection)
            {
                mmh.Add(item);
            }
            T first = mmh.GetMin();
            bool res2 = mmh.RemoveMin();
            T second = mmh.GetMin();

            Assert.AreEqual(false, res1);
            Assert.AreEqual(true, res2);
            Assert.AreEqual(ordered.ElementAt(0), first);
            Assert.AreEqual(ordered.ElementAt(1), second);
            Assert.AreEqual(collection.Count() - 1, mmh.Count);
        }

        [Test()]
        [TestCaseSource("collectionsSource")]
        public static void ExtractMaxTest<T>(IEnumerable<T> collection)
        {
            var ordered = collection.OrderByDescending(x => x);
            var mmh = new MinMaxHeap<T>(collection);
            var emptyHeap = new MinMaxHeap<T>();

            T first = mmh.ExtractMax();
            T second = mmh.GetMax();

            Assert.Throws<InvalidOperationException>(() => emptyHeap.ExtractMax());
            Assert.AreEqual(ordered.ElementAt(0), first);
            Assert.AreEqual(ordered.ElementAt(1), second);
            Assert.AreEqual(collection.Count() - 1, mmh.Count);
        }

        [Test()]
        [TestCaseSource("collectionsSource")]
        public static void ExtractMinTest<T>(IEnumerable<T> collection)
        {
            var ordered = collection.OrderBy(x => x);
            var mmh = new MinMaxHeap<T>(collection);
            var emptyHeap = new MinMaxHeap<T>();

            T first = mmh.ExtractMin();
            T second = mmh.GetMin();

            Assert.Throws<InvalidOperationException>(() => emptyHeap.ExtractMin());
            Assert.AreEqual(ordered.ElementAt(0), first);
            Assert.AreEqual(ordered.ElementAt(1), second);
            Assert.AreEqual(collection.Count() - 1, mmh.Count);
        }


        [Test()]
        [TestCaseSource("collectionsSource")]
        public static void GetMaxTest<T>(IEnumerable<T> collection)
        {
            var emptyHeap = new MinMaxHeap<int>();
            var mmh = new MinMaxHeap<T>(collection);

            T maxValue = mmh.GetMax();

            Assert.Throws<InvalidOperationException>(() => emptyHeap.GetMax());
            Assert.AreEqual(collection.Max(), maxValue);
        }

        [Test()]
        [TestCaseSource("collectionsSource")]
        public static void GetMinTest<T>(IEnumerable<T> collection)
        {
            var emptyHeap = new MinMaxHeap<int>();
            var mmh = new MinMaxHeap<T>(collection);

            T minValue = mmh.GetMin();

            Assert.Throws<InvalidOperationException>(() => emptyHeap.GetMin());
            Assert.AreEqual(collection.Min(), minValue);
        }

        [Test()]
        public static void HeapSortUsingGetAndRemove<T>([ValueSource("collectionsSource")]IEnumerable<T> collection, [Values]bool ascending)
        {
            var ordered = ascending ? collection.OrderBy(x => x) : collection.OrderByDescending(x => x);
            var mmh = new MinMaxHeap<T>(collection);
            var extracted = new List<T>();

            while (mmh.Count > 0)
            {
                T value;
                if (ascending)
                {
                    value = mmh.GetMin();
                    mmh.RemoveMin();
                }
                else
                {
                    value = mmh.GetMax();
                    mmh.RemoveMax();
                }
                extracted.Add(value);
            }

            Assert.IsTrue(ordered.SequenceEqual(extracted));
        }

        [Test()]
        public static void HeapSortUsingExtract<T>([ValueSource("collectionsSource")]IEnumerable<T> collection, [Values]bool ascending)
        {
            var ordered = ascending ? collection.OrderBy(x => x) : collection.OrderByDescending(x => x);
            var mmh = new MinMaxHeap<T>(collection);
            var extracted = new List<T>();

            while (mmh.Count > 0)
            {
                T value = ascending ? mmh.ExtractMin() : mmh.ExtractMax();
                extracted.Add(value);
            }

            Assert.IsTrue(ordered.SequenceEqual(extracted));
        }
    }
}
