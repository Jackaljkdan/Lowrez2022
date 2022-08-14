using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

namespace JK.Utils.DGTweening
{
    public static class Light2DTweening
    {
        public static Tween DOIntensity(this Light2D light2D, float intensity, float seconds)
        {
            return DOTween.To(
                () => light2D.intensity,
                val => light2D.intensity = val,
                intensity,
                seconds
            );
        }
    }
}