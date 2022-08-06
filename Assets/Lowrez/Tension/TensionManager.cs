using DG.Tweening;
using JK.Injection;
using JK.Observables;
using Lowrez.Monsters;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Lowrez.Tension
{
    [DisallowMultipleComponent]
    public class TensionManager : MonoBehaviour
    {
        #region Inspector

        public float backToZeroSeconds = 20;

        #endregion
        
        private ObservableProperty<float> tension;
        private SignalBus signalBus;
        
        private Tween tensionTween;

        private void Awake()
        {
            var context = Context.Find(this);
            
            tension = context.Get<ObservableProperty<float>>("tension");
            signalBus = context.Get<SignalBus>();
        }

        private void Start()
        {
            signalBus.AddListener<GrabbedSignal>(OnGrabbed);
            signalBus.AddListener<UngrabbedSignal>(OnUngrabbed);
        }

        private void OnDestroy()
        {
            signalBus.RemoveListener<GrabbedSignal>(OnGrabbed);
            signalBus.RemoveListener<UngrabbedSignal>(OnUngrabbed);
        }

        public Tween DOTension(float value, float seconds)
        {
            tensionTween?.Kill();

            tensionTween = DOTween.To(
                () => tension.Value,
                val => tension.Value = val,
                value,
                seconds
            );

            return tensionTween;
        }

        private void OnGrabbed(GrabbedSignal signal)
        {
            DOTension(1, 0.1f);
        }

        private void OnUngrabbed(UngrabbedSignal signal)
        {
            DOTension(0, backToZeroSeconds);
        }
    }
}