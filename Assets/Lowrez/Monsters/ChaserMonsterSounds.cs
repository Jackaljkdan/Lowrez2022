using DG.Tweening;
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
    public class ChaserMonsterSounds : EndgameListener
    {
        #region Inspector

        public ChaserMonster monster;

        public List<AudioClip> contactClips;

        public float secondsBetweenSteps = 0.5f;


        public List<AudioClip> stepClips;

        public AudioClip hugClip;

        public AudioSource contactAudioSource;
        public AudioSource stepAudioSource;
        public AudioSource hugAudioSource;

        private void Reset()
        {
            monster = GetComponentInParent<ChaserMonster>();
            contactAudioSource = GetComponentInChildren<AudioSource>();
        }

        #endregion

        private float hugReferenceVolume;

        protected override void Start()
        {
            base.Start();
            monster.State.onChange.AddListener(OnStateChanged);
            hugReferenceVolume = hugAudioSource.volume;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            monster.State.onChange.RemoveListener(OnStateChanged);
        }

        private void OnStateChanged(ObservableProperty<ChaserMonsterState>.Changed arg)
        {
            if (arg.updated == ChaserMonsterState.Chasing)
                contactAudioSource.PlayRandomClip(contactClips, oneShot: true);

            if (arg.updated == ChaserMonsterState.Grabbing)
            {
                hugAudioSource.time = 0;
                hugAudioSource.volume = hugReferenceVolume;
                hugAudioSource.Play();
            }
            else if (arg.old == ChaserMonsterState.Grabbing)
            {
                hugAudioSource.DOFade(0, 0.5f).onComplete += () => hugAudioSource.Stop();
            }
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

        protected override void OnEndGame(bool win)
        {
            contactAudioSource.DOFade(0, 0.5f);
            stepAudioSource.DOFade(0, 0.5f);
            hugAudioSource.DOFade(0, 0.5f);
        }
    }
}