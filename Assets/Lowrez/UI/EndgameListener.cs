using JK.Injection;
using Lowrez.Monsters;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Lowrez.UI
{
    [DisallowMultipleComponent]
    public abstract class EndgameListener : MonoBehaviour
    {
        #region Inspector



        #endregion

        protected SignalBus signalBus;

        protected virtual void Awake()
        {
            signalBus = Context.Find(this).Get<SignalBus>();
        }

        protected virtual void Start()
        {
            signalBus.AddListener<BrainDeathSignal>(OnBrainDeathSignal);
            // TODO: ascoltare segnale morte giocatore
            //signalBus.AddListener<BrainDeathSignal>(OnBranDeathSignal);
        }

        protected virtual void OnDestroy()
        {
            RemoveListeners();
        }

        private void RemoveListeners()
        {
            signalBus.RemoveListener<BrainDeathSignal>(OnBrainDeathSignal);
            // TODO: togliere segnale morte giocatore
            //signalBus.RemoveListener<BrainDeathSignal>(OnBranDeathSignal);
        }

        private void OnBrainDeathSignal(BrainDeathSignal arg)
        {
            OnPrivateEndGame(win: true);
        }

        private void OnPrivateEndGame(bool win)
        {
            RemoveListeners();
            OnEndGame(win);
        }

        protected abstract void OnEndGame(bool win);
    }
}