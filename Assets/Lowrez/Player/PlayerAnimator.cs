using DG.Tweening;
using JK.Actuators;
using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Lowrez.Player
{
    [DisallowMultipleComponent]
    public class PlayerAnimator : MonoBehaviour
    {
        #region Inspector

        public Animator animator;

        public MovementActuatorBehaviour movementActuator;

        public List<AudioClip> stepClips;

        public AudioSource leftFootSource;
        public AudioSource rightFootSource;

        //public UnityEvent onLeftStep = new UnityEvent();
        //public UnityEvent onRightStep = new UnityEvent();

        [Header("Runtime")]

        public bool isWalking;

        private void Reset()
        {
            animator = GetComponent<Animator>();
            movementActuator = GetComponentInParent<MovementActuatorBehaviour>();
        }

        #endregion

        private Tween tween;


        private void Start()
        {
            isWalking = false;
        }


        private void LateUpdate()
        {
            bool isMoving = movementActuator.Input.sqrMagnitude > 0;

            if (isMoving && !isWalking)
            {
                animator.Play("Walk", 0, 0);
                isWalking = true;
            }
            else if (!isMoving && isWalking)
            {
                animator.Play("Idle", 0, 0);
                isWalking = false;
            }
        }

        public void OnLeftStep()
        {
            leftFootSource.PlayRandomClip(stepClips, oneShot: true);
        }

        public void OnRightStep()
        {
            rightFootSource.PlayRandomClip(stepClips, oneShot: true);
        }
    }
}