using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    public static class UnityEventUtils
    {
        public static void AddListenerOnce(this UnityEvent unityEvent, UnityAction call)
        {
            void wrapper()
            {
                if (unityEvent != null)
                    unityEvent.RemoveListener(wrapper);

                call();
            }

            unityEvent.AddListener(wrapper);
        }

        public static void AddListenerOnce<T>(this UnityEvent<T> unityEvent, UnityAction<T> call)
        {
            void wrapper(T arg0)
            {
                if (unityEvent != null)
                    unityEvent.RemoveListener(wrapper);

                call(arg0);
            }

            unityEvent.AddListener(wrapper);
        }

        public static void AddListenerOnce<T0, T1>(this UnityEvent<T0, T1> unityEvent, UnityAction<T0, T1> call)
        {
            void wrapper(T0 arg0, T1 arg1)
            {
                if (unityEvent != null)
                    unityEvent.RemoveListener(wrapper);

                call(arg0, arg1);
            }

            unityEvent.AddListener(wrapper);
        }
    }
}