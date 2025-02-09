using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class BasicPerfFiberTests
{
    interface IPerformTest
    {
        void PerformTest(int b);
    }

    class Test1 : IPerformTest
    {
        int a = 0;
        public void PerformTest(int b)
        {
            a = 1 + b;
        }
    }

    class BaseTest
    {
        public virtual void PerformTest(int b)
        {
            Debug.Log("Test");
        }
    }

    class Test2 : BaseTest
    {
        int a = 0;
        public override void PerformTest(int b)
        {
            a = 1 + b;
        }
    }

    class Test3
    {
        int a = 0;
        public void PerformTest(int b)
        {
            a = 1 + b;
        }
    }

    // A Test behaves as an ordinary method
    [Test]
    public void BasicPerfFiberTestsSimplePasses()
    {
        int iterations = 100000000;

        IPerformTest test1 = new Test1();
        BaseTest test2 = new Test2();
        Test3 test3 = new Test3();

        var stopwatch = new System.Diagnostics.Stopwatch();

        stopwatch.Start();
        for (int i = 0; i < iterations; i++)
        {
            test1.PerformTest(i);
        }
        stopwatch.Stop();
        Debug.Log("Test1: " + stopwatch.ElapsedMilliseconds);

        stopwatch.Reset();
        stopwatch.Start();

        for (int i = 0; i < iterations; i++)
        {
            test2.PerformTest(i);
        }
        stopwatch.Stop();
        Debug.Log("Test2: " + stopwatch.ElapsedMilliseconds);

        stopwatch.Reset();
        stopwatch.Start();

        for (int i = 0; i < iterations; i++)
        {
            test3.PerformTest(i);
        }
        stopwatch.Stop();
        Debug.Log("Test3: " + stopwatch.ElapsedMilliseconds);
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator BasicPerfFiberTestsWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
