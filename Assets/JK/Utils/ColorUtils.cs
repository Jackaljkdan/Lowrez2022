using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    public static class ColorUtils
    {
        public static Color WithAlpha(this Color color, float alpha)
        {
            Color copy = color;
            copy.a = alpha;
            return copy;
        }
    }
}