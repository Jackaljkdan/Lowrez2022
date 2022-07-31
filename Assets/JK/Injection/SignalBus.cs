using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Injection
{
    [Serializable]
    public class SignalBus
    {
        // N.B. an instance could never survive recompilation because
        // [SerializeReference] does not support inflated types, i.e. class<T>
        // not even a static instance works, nor even a static c# event

        private Dictionary<Type, UnityEventBase> dict = new Dictionary<Type, UnityEventBase>();

        private UnityEvent<T> Get<T>()
        {
            UnityEvent<T> ev;
            var key = typeof(T);

            try
            {
                ev = (UnityEvent<T>)dict[key];
            }
            catch (KeyNotFoundException)
            {
                ev = new UnityEvent<T>();
                dict[key] = ev;
            }

            return ev;
        }

        public void AddListener<T>(UnityAction<T> action) where T : struct
        {
            Get<T>().AddListener(action);
        }

        public void AddListenerOnce<T>(UnityAction<T> action) where T : struct
        {
            Get<T>().AddListenerOnce(action);
        }

        public void RemoveListener<T>(UnityAction<T> action) where T : struct
        {
            try
            {
                UnityEvent<T> ev = (UnityEvent<T>)dict[typeof(T)];
                ev.RemoveListener(action);
            }
            catch (KeyNotFoundException)
            {
                // there's no listener, but we don't care
            }
        }

        public void Invoke<T>(T parameter = default) where T : struct
        {
            try
            {
                UnityEvent<T> ev = (UnityEvent<T>)dict[typeof(T)];
                ev.Invoke(parameter);
            }
            catch (KeyNotFoundException)
            {
                // there's no listener, but we don't care
            }
        }
    }
}