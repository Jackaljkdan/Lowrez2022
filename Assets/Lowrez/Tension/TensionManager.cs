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
        private TensionMusic tensionMusic;
        private SignalBus signalBus;
        
        private Tween tensionTween;

        private void Awake()
        {
            var context = Context.Find(this);
            
            tension = context.Get<ObservableProperty<float>>("tension");
            tensionMusic = context.Get<TensionMusic>();
            signalBus = context.Get<SignalBus>();
        }

        private void Start()
        {
            signalBus.AddListener<GrabbedSignal>(OnGrabbed);
            signalBus.AddListener<UngrabbedSignal>(OnUngrabbed);

            signalBus.AddListener<ChasingSignal>(OnChasing);
            signalBus.AddListener<StopChasingSignal>(OnStopChasing);
        }

        private void OnDestroy()
        {
            signalBus.RemoveListener<GrabbedSignal>(OnGrabbed);
            signalBus.RemoveListener<UngrabbedSignal>(OnUngrabbed);

            signalBus.RemoveListener<ChasingSignal>(OnChasing);
            signalBus.RemoveListener<StopChasingSignal>(OnStopChasing);
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

        private bool isChasing = false;

        private void OnChasing(ChasingSignal signal)
        {
            if (!isChasing)
            {
                isChasing = true;
                DOTension(tensionMusic.chaseThreshold + 0.01f, 0.2f);
            }
        }

        private void OnStopChasing(StopChasingSignal signal)
        {
            if (isChasing)
            {
                isChasing = false;
                DOTension(0, backToZeroSeconds);
            }
        }
    }
}