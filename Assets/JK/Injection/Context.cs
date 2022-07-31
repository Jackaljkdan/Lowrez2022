using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Injection
{
    [Serializable]
    public class Context
    {
        public const string EmptyId = "";

        public string name;

        private InjectionDictionary dict = new InjectionDictionary();

#if UNITY_EDITOR
        [SerializeReference]
#endif
        private Context container;

        public Context() { }

        public Context(Context container, string name)
        {
            this.container = container;
            this.name = name;
        }

        public T Get<T>(string id = EmptyId)
        {
            return (T)Get(typeof(T), id);
        }

        public object Get(Type type, string id = EmptyId)
        {
            var key = dict.GetKey(type, id);

            if (dict.TryGetValue(key, out object value))
                return value;

            if (container != null)
                return container.Get(type, id);

            throw new BindingNotFoundException(this, type, id);
        }

        public T GetOptional<T>(string id = EmptyId, object defaultValue = null)
        {
            return (T)GetOptional(typeof(T), id, defaultValue);
        }

        public object GetOptional(Type type, string id = EmptyId, object defaultValue = null)
        {
            var key = dict.GetKey(type, id);

            if (dict.TryGetValue(key, out object value))
                return value;

            if (container != null)
                return container.Get(type, id);

            return defaultValue;
        }

        public bool TryGetOptional<T>(out T value, string id = EmptyId, T defaultValue = default)
        {
            bool found = TryGetOptional(typeof(T), out object obj, id, defaultValue);
            value = (T)obj;
            return found;
        }

        public bool TryGetOptional(Type type, out object value, string id = EmptyId, object defaultValue = null)
        {
            value = GetOptional(type, id, defaultValue: null);

            if (value != null)
                return true;

            value = defaultValue;
            return false;
        }

        public void Bind<T>(T value, string id = EmptyId)
        {
            BindUnsafe(value, typeof(T), id);
        }

        public void BindAs<TValue, TBinding>(TValue value, string id = EmptyId) where TValue : TBinding
        {
            BindUnsafe(value, typeof(TBinding), id);
        }

        /// <summary>
        /// This is unsafe because if you pass a <paramref name="value"/> that isn't
        /// of type <paramref name="type"/> then you will get an <see cref="InvalidCastException"/>
        /// when you try to <see cref="Get{T}(string)"/> it
        /// </summary>
        public void BindUnsafe(object value, Type type, string id = EmptyId)
        {
            InjectionUtils.ThrowInEditorForInvalidId(id);

            var key = dict.GetKey(type, id);

            if (PlatformUtils.IsEditor)
            {
                if (dict.ContainsKey(key))
                    Debug.LogError($"Context {name} already had a binding for type {type} with id {id} and it will be overwritten");
            }

            dict[key] = value;
        }

        public override string ToString()
        {
            return $"{name} (Context)";
        }

        public static Context Find(Component component)
        {
            MonoContext monoContext = component.GetComponentInParent<MonoContext>();

            if (monoContext != null)
                return monoContext.context;

            component.gameObject.scene.GetRootGameObjects(list);

            foreach (var go in list)
            {
                if (go.TryGetComponent(out monoContext))
                    return monoContext.context;
            }

            monoContext = ProjectContext.Get();

            if (monoContext != null)
                return monoContext.context;

            return null;
        }

        private static List<GameObject> list = new List<GameObject>(16);

        public static bool TryFind(Component component, out Context context)
        {
            context = Find(component);
            return context != null;
        }
    }
}