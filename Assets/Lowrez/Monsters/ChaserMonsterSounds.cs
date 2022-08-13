using JK.Observables;
using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Lowrez.Monsters
{
    [DisallowMultipleComponent]
    public class ChaserMonsterSounds : MonoBehaviour
    {
        #region Inspector

        public ChaserMonster monster;

        public AudioSource audioSource;

        public List<AudioClip> contactClips;

        private void Reset()
        {
            monster = GetComponentInParent<ChaserMonster>();
            audioSource = GetComponentInChildren<AudioSource>();
        }

        #endregion

        private void Start()
        {
            monster.State.onChange.AddListener(OnStateChanged);
        }

        private void OnDestroy()
        {
            monster.State.onChange.RemoveListener(OnStateChanged);
        }

        private void OnStateChanged(ObservableProperty<ChaserMonsterState>.Changed arg)
        {
            if (arg.updated == ChaserMonsterState.Chasing)
                audioSource.PlayRandomClip(contactClips, oneShot: true);
        }
    }
}