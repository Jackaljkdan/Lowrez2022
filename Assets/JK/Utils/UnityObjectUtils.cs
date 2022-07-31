using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    public static class UnityObjectUtils
    {
        public static string GetName(this UnityEngine.Object obj)
        {
            if (obj != null)
                return obj.name;
            else
                return "\"null\"";
        }
    }
}