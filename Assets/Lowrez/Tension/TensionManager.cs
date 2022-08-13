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
    public class TensionManager : EndgameListener
    {
        #region Inspector

        public float backToZeroSeconds = 20;

        #endregion
        
        private ObservableProperty<float> tension;
        private TensionMusic tensionMusic;
        
        private Tween tensionTween;

        protected override void Awake()
        {
            base.Awake();

            var context = Context.Find(this);
            
            tension = context.Get<ObservableProperty<float>>("tension");
            tensionMusic = context.Get<TensionMusic>();
            signalBus = context.Get<SignalBus>();
        }

        protected override void Start()
        {
            base.Start();

            signalBus.AddListener<GrabbedSignal>(OnGrabbed);
            signalBus.AddListener<UngrabbedSignal>(OnUngrabbed);

            signalBus.AddListener<ChasingSignal>(OnChasing);
            signalBus.AddListener<StopChasingSignal>(OnStopChasing);

            signalBus.AddListener<BrainPainSignal>(OnBrainPainSignal);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            signalBus.RemoveListener<GrabbedSignal>(OnGrabbed);
            signalBus.RemoveListener<UngrabbedSignal>(OnUngrabbed);

            signalBus.RemoveListener<ChasingSignal>(OnChasing);
            signalBus.RemoveListener<StopChasingSignal>(OnStopChasing);

            signalBus.RemoveListener<BrainPainSignal>(OnBrainPainSignal);
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
        private bool isBrainPain = false;

        private void OnChasing(ChasingSignal signal)
        {
            if (!isChasing )
            {
                isChasing = true;

                if (!isBrainPain)
                    DOTension(tensionMusic.chaseThreshold + 0.01f, 0.2f);
            }
        }

        private void OnStopChasing(StopChasingSignal signal)
        {
            if (isChasing)
            {
                isChasing = false;

                if (!isBrainPain)
                    DOTension(0, backToZeroSeconds);
            }
        }

        private void OnBrainPainSignal(BrainPainSignal signal)
        {
            isBrainPain = signal.isStart;

            if (isBrainPain)
                DOTension(1, 0.2f);
            else if (!isChasing)
                DOTension(0, backToZeroSeconds);
        }

        protected override void OnEndGame(bool win)
        {
            DOTension(0, 1);
            OnDestroy();
        }
    }
}