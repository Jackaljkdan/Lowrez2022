using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    public static class PlatformUtils
    {
#if UNITY_EDITOR
        public const bool IsEditor = true;
#else
        public const bool IsEditor = false;
#endif

        public static bool IsMobile => Application.isMobilePlatform;

        public static bool IsDesktop => !IsMobile;

#if UNITY_WEBGL
        public const bool IsWebGL = true;
#else
        public const bool IsWebGL = false;
#endif
    }
}