using JK.Injection;
using Lowrez.Monsters;
using Lowrez.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Lowrez
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
            signalBus.AddListener<PlayerDeathSignal>(OnPlayerDeathSignal);
        }

        protected virtual void OnDestroy()
        {
            RemoveListeners();
        }

        private void RemoveListeners()
        {
            signalBus.RemoveListener<BrainDeathSignal>(OnBrainDeathSignal);
            signalBus.RemoveListener<PlayerDeathSignal>(OnPlayerDeathSignal);
        }

        private void OnBrainDeathSignal(BrainDeathSignal arg)
        {
            OnPrivateEndGame(win: true);
        }

        private void OnPlayerDeathSignal(PlayerDeathSignal arg)
        {
            OnPrivateEndGame(win: false);
        }

        private void OnPrivateEndGame(bool win)
        {
            RemoveListeners();
            OnEndGame(win);
        }

        protected abstract void OnEndGame(bool win);
    }
}