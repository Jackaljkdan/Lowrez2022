using JK.Injection;
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
    public class ChaserMonsterAnimator : MonoBehaviour
    {
        #region Inspector

        public ChaserMonster monster;

        public Animator animator;

        public Transform sprite;

        public float directionLerp = 0.1f;

        //[Header("debug")]

        //public Vector3 dir;
        //public Vector3 lerp;
        //public float adjLerp;

        #endregion

        private Transform playerTransform;

        private void Awake()
        {
            playerTransform = Context.Find(this).Get<Transform>("player");
        }

        private void Start()
        {
            monster.State.onChange.AddListener(OnStateChanged);
            OnStateChanged(new ObservableProperty<ChaserMonsterState>.Changed() { updated = monster.State.Value });
        }

        private void OnDestroy()
        {
            monster.State.onChange.RemoveListener(OnStateChanged);
        }

        private void OnStateChanged(ObservableProperty<ChaserMonsterState>.Changed arg)
        {
            switch (arg.updated)
            {
                case ChaserMonsterState.Idle:
                case ChaserMonsterState.Recovering:
                    animator.Play("Idle", 0, 0);
                    enabled = false;
                    break;
                case ChaserMonsterState.Chasing:
                    animator.Play("Walk", 0, 0);
                    enabled = true;
                    break;
                case ChaserMonsterState.Grabbing:
                    animator.Play("Grab", 0, 0);
                    enabled = true;
                    break;
            }
        }

        private void Update()
        {
            Vector3 movementInput = monster.movementActuator.Input;
            movementInput.y = movementInput.z;
            movementInput.z = 0;

            //dir = movementInput;
            //lerp = Vector3.Lerp(sprite.right, movementInput, TimeUtils.AdjustToFrameRate(directionLerp));
            //adjLerp = TimeUtils.AdjustToFrameRate(directionLerp);

            sprite.right = Vector3.Lerp(sprite.right, movementInput, TimeUtils.AdjustToFrameRate(directionLerp));
        }
    }
}