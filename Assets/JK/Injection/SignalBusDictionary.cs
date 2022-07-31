using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Injection
{
#if UNITY_EDITOR
    using SignalBusKey = System.String;

    public static class SignalBusKeyUtils
    {
        public static SignalBusKey GetKey(this SignalBusDictionary dict, Type type)
        {
            return type.ToString();
        }
    }
#else
    using SignalBusKey = System.Type;

    public static class SignalBusKeyUtils
    {
        public static SignalBusKey GetKey(this SignalBusDictionary dict, Type type)
        {
            return type;
        }
    }
#endif

    [Serializable]
    public class SignalBusDictionary : EditorSerliazedDictionary<SignalBusKey>
    {

    }
}