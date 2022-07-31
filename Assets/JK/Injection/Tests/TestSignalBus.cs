using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Injection.Tests
{
    [DisallowMultipleComponent]
    public class TestSignalBus : MonoBehaviour
    {
        #region Inspector



        #endregion

        private struct TestSignal
        {
            public int x;
        }

        private static event Action<TestSignal> testCsEvent;

        private int expected = 42;

        private int listenedCount = 0;

        [SerializeReference, HideInInspector]
        private UnityEventBase testEvent;

        //private SignalBus bus;

        //[ContextMenu("Test")]
        //private void Test()
        //{
        //    bus = new SignalBus();
        //    listenedCount = 0;

        //    bus.AddListener<TestSignal>(ListenerTest);
        //    bus.Invoke(new TestSignal { x = expected });
        //    bus.RemoveListener<TestSignal>(ListenerTest);
        //    bus.Invoke(new TestSignal { x = expected });

        //    Debug.Log($"listened count: {listenedCount} {listenedCount == 1}");
        //}

        //[ContextMenu("SetupBeforeReload")]
        //private void SetupBeforeReload()
        //{
        //    bus = new SignalBus();
        //    bus.AddListener<TestSignal>(ListenerTest);
        //    bus.Invoke(new TestSignal { x = 314 });
        //}

        //[ContextMenu("TestAfterReload")]
        //private void TestAfterReload()
        //{
        //    bus.Invoke(new TestSignal { x = 314 });
        //}

        [ContextMenu("TestContext")]
        private void Test()
        {
            SignalBus bus = Context.Find(this).Get<SignalBus>();
            listenedCount = 0;

            bus.AddListener<TestSignal>(ListenerTest);
            bus.Invoke(new TestSignal { x = expected });
            bus.RemoveListener<TestSignal>(ListenerTest);
            bus.Invoke(new TestSignal { x = expected });

            Debug.Log($"listened count: {listenedCount} {listenedCount == 1}");
        }

        private void ListenerTest(TestSignal signal)
        {
            listenedCount += 1;
            Debug.Log($"x: {signal.x} {signal.x == expected}");
        }

        [ContextMenu("FieldSetupBeforeReload")]
        private void FieldSetupBeforeReload()
        {
            testEvent = new UnityEvent<TestSignal>();
            ((UnityEvent<TestSignal>)testEvent).AddListener(ListenerTest);
            ((UnityEvent<TestSignal>)testEvent).Invoke(new TestSignal { x = 42 });
        }

        [ContextMenu("FieldTestAfterReload")]
        private void FieldTestAfterReload()
        {
            ((UnityEvent<TestSignal>)testEvent).Invoke(new TestSignal { x = 42 });
        }

        //[ContextMenu("StaticSetupBeforeReload")]
        //private void StaticSetupBeforeReload()
        //{
        //    SignalBus.Instance.AddListener<TestSignal>(ListenerTest);
        //    SignalBus.Instance.Invoke(new TestSignal { x = 42 });
        //}

        //[ContextMenu("StaticTestAfterReload")]
        //private void StaticTestAfterReload()
        //{
        //    SignalBus.Instance.Invoke(new TestSignal { x = 42 });
        //}

        [ContextMenu("StaticCsSetupBeforeReload")]
        private void StaticCsSetupBeforeReload()
        {
            testCsEvent += ListenerTest;
            testCsEvent.Invoke(new TestSignal { x = 42 });
        }

        [ContextMenu("StaticCsTestAfterReload")]
        private void StaticCsTestAfterReload()
        {
            testCsEvent.Invoke(new TestSignal { x = 42 });
        }
    }
}