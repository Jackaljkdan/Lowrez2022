using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Injection
{
    public static class InjectionUtils
    {
        public static void ThrowInEditorForInvalidId(string id)
        {
            if (PlatformUtils.IsEditor && id == null)
                throw new ArgumentException("Binding id cannot be null");
        }
    }
}