using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    public static class TimeUtils
    {
        public static float AdjustToFrameRate(float value)
        {
            return value * Application.targetFrameRate * Time.deltaTime;
        }
    }
}