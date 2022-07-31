using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Injection
{
    [Serializable]
    public class SceneParametersBus
    {
#if UNITY_EDITOR
        [SerializeField]
#endif
        private InjectionDictionary dict = new InjectionDictionary();

        public void Set<T>(T value, string id = Context.EmptyId)
        {
            SetUnsafe(value, typeof(T), id);
        }

        /// <summary>
        /// This is unsafe because if you pass a <paramref name="value"/> that isn't
        /// of type <paramref name="type"/> then you will get an <see cref="InvalidCastException"/>
        /// when you try to <see cref="Get{T}(string, T)"/> or <see cref="Consume{T}(string, T)"/> it
        /// </summary>
        public void SetUnsafe(object value, Type type, string id = Context.EmptyId)
        {
            InjectionUtils.ThrowInEditorForInvalidId(id);
            dict[dict.GetKey(type, id)] = value;
        }

        public void Remove<T>(string id = Context.EmptyId)
        {
            Remove(typeof(T), id);
        }

        public void Remove(Type type, string id = Context.EmptyId)
        {
            dict.Remove(dict.GetKey(type, id));
        }

        public T Consume<T>(string id = Context.EmptyId, T defaultValue = default)
        {
            return (T)Consume(typeof(T), id, defaultValue);
        }

        public object Consume(Type type, string id = Context.EmptyId, object defaultValue = null)
        {
            try
            {
                var key = dict.GetKey(type, id);
                object value = dict[key];
                dict.Remove(key);
                return value;
            }
            catch (KeyNotFoundException)
            {
                return defaultValue;
            }
        }

        public T Get<T>(string id = Context.EmptyId, T defaultValue = default)
        {
            return (T)Get(typeof(T), id, defaultValue);
        }

        public object Get(Type type, string id = Context.EmptyId, object defaultValue = null)
        {
            try
            {
                return dict[dict.GetKey(type, id)];
            }
            catch (KeyNotFoundException)
            {
                return defaultValue;
            }
        }
    }
}