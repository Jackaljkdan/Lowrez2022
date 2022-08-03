using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    public static class ComponentUtils
    {
        public static bool TryGetComponentInParent<T>(this Component self, out T component)
        {
            component = self.GetComponentInParent<T>();
            return component != null;
        }
        
    }
}