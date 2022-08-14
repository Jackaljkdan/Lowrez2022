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

        public List<AudioClip> contactClips;

        public float secondsBetweenSteps = 0.5f;

        public List<AudioClip> stepClips;

        public AudioSource contactAudioSource;
        public AudioSource stepAudioSource;

        private void Reset()
        {
            monster = GetComponentInParent<ChaserMonster>();
            contactAudioSource = GetComponentInChildren<AudioSource>();
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
                contactAudioSource.PlayRandomClip(contactClips, oneShot: true);
        }

        private float lastStepTime;

        private bool wasMoving = false;

        private void LateUpdate()
        {
            bool isMoving = monster.movementActuator.Input.sqrMagnitude > 0;
            
            if (!wasMoving && isMoving)
            {
                wasMoving = true;
                lastStepTime = Time.time;
            }

            if (!isMoving)
            {
                wasMoving = false;
                return;
            }

            float elapsed = Time.time - lastStepTime;

            if (elapsed >= secondsBetweenSteps)
            {
                lastStepTime = Time.time;
                stepAudioSource.PlayRandomClip(stepClips, oneShot: true);
            }
        }
    }
}