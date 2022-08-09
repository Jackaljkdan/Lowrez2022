using DG.Tweening;
using JK.Injection;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Sounds
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RandomClipsPlayer))]
    public class SurroundingAudioSource : MonoBehaviour
    {
        #region Inspector



        #endregion

        private Transform firstAnchor;
        private Transform secondAnchor;

        private Tween tween;

        private void Awake()
        {
            var context = Context.Find(this);

            firstAnchor = context.Get<Transform>("anchor.0");
            secondAnchor = context.Get<Transform>("anchor.1");
        }

        private void OnEnable()
        {
            
        }
    }
}