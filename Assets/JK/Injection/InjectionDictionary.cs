using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Injection
{
#if UNITY_EDITOR
    // with this trick i ensure that when recompiling during playmode everything works
    // it's much slower than the non editor alternative because i'm calling Type.ToString()
    using InjectionKey = System.String;

    public static class InjectionKeyUtils
    {
        public static InjectionKey GetKey(this InjectionDictionary dict, Type type, string id)
        {
            return $"{type}-{id}";
        }
    }
#else
    // this is the fast runtime implementation where i don't need to recompile in play mode
    // this is not serializable (because of Type, it doesn't work even with [SerializeReference]) and will break on recompilation
    public struct InjectionKey : IEquatable<InjectionKey>
    {
        public Type type;
        public string id;

        public bool Equals(InjectionKey other)
        {
            return other.type == type && other.id == id;
        }

        public override int GetHashCode()
        {
            return type.GetHashCode() ^ id.GetHashCode();
        }

        public override string ToString()
        {
            return $"{type}-{id}";
        }
    }

    public static class InjectionKeyUtils
    {
        public static InjectionKey GetKey(this InjectionDictionary dict, Type type, string id)
        {
            return new InjectionKey()
            {
                type = type,
                id = id,
            };
        }
    }
#endif

    [Serializable]
    public class InjectionDictionary : EditorSerliazedDictionary<InjectionKey>
    {

    }
}