using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class StringVsIntComparisionTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void StringVsIntComparisionTestSimplePasses()
    {
        int iterations = 20000000;

        var stopwatch = new System.Diagnostics.Stopwatch();

        var str1 = "test";
        var str2 = "test2";

        var int1 = 1;
        var int2 = 2;

        var listOfStrings = new List<string>() { str2 };
        var listOfInts = new List<int>() { int2 };

        stopwatch.Start();
        for (int i = 0; i < iterations; i++)
        {
            var containesEitherOr = listOfStrings.Contains(str1) || listOfStrings.Contains(str2);
        }
        stopwatch.Stop();
        Debug.Log("Test1: " + stopwatch.ElapsedMilliseconds);

        stopwatch.Reset();
        stopwatch.Start();
        for (int i = 0; i < iterations; i++)
        {
            var containesEitherOr = listOfInts.Contains(int1) || listOfInts.Contains(int2);
        }

        stopwatch.Stop();
        Debug.Log("Test2: " + stopwatch.ElapsedMilliseconds);
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator StringVsIntComparisionTestWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
