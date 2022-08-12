using JK.Injection;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Lowrez.Monsters
{
    [DisallowMultipleComponent]
    public class DisablerOnBrainDeath : MonoBehaviour
    {
        #region Inspector

        public Behaviour target;

        #endregion

        private SignalBus signalBus;

        private void Awake()
        {
            signalBus = Context.Find(this).Get<SignalBus>();
        }

        private void Start()
        {
            signalBus.AddListener<BrainDeathSignal>(OnBrainDeathSignal);
        }

        private void OnDestroy()
        {
            signalBus.RemoveListener<BrainDeathSignal>(OnBrainDeathSignal);
        }

        private void OnBrainDeathSignal(BrainDeathSignal signal)
        {
            target.enabled = false;
        }
    }
}