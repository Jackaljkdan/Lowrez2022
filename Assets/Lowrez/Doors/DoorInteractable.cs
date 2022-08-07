using JK.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Lowrez.Doors
{
    [DisallowMultipleComponent]
    public class DoorInteractable : InteractableBehaviour
    {
        #region Inspector

        [SerializeField]
        private bool startsOpen = false;

        public bool isLocked = false;

        public Animator animator;
        public string openAnimationName = "OpenDoor";
        public string closeAnimationName = "CloseDoor";

        public AudioSource audioSource = null;
        public AudioClip openClip = null;
        public AudioClip closeClip = null;
        public AudioClip lockedClip = null;

        public new Collider2D collider;

        public UnityEvent onInteraction = new UnityEvent();

        private void Reset()
        {
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            collider = GetComponentInChildren<Collider2D>();
        }

        #endregion

        public bool IsAnimating { get; private set; }

        public bool IsOpening => animator.GetCurrentAnimatorStateInfo(0).shortNameHash == Animator.StringToHash(openAnimationName);

        private void Start()
        {
            animator.Play(startsOpen ? openAnimationName : closeAnimationName, 0, 1);
            if (startsOpen)
                OnDoorOpened();
            else
                OnDoorClosed();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
                PerformInteraction(default);
        }

        private void PlayAudioClip(AudioClip clip)
        {
            if (audioSource != null && clip != null)
                audioSource.PlayOneShot(clip);
        }

        protected override void PerformInteraction(RaycastHit hit)
        {
            if (isLocked)
            {
                PlayAudioClip(lockedClip);
                onInteraction.Invoke();
                return;
            }

            float currentNormalizedTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            float targetNormalizedTime = IsAnimating ? 1 - currentNormalizedTime : 0;

            //Debug.Log($"door interaction: opening = {IsOpening} anim = {IsAnimating} tnt = {targetNormalizedTime}");

            if (!IsOpening)
            {
                animator.Play(openAnimationName, 0, targetNormalizedTime);
                PlayAudioClip(openClip);
            }
            else
            {
                animator.Play(closeAnimationName, 0, targetNormalizedTime);
                PlayAudioClip(closeClip);
                collider.enabled = true;
            }

            IsAnimating = true;

            onInteraction.Invoke();
        }

        public void OnDoorOpened()
        {
            IsAnimating = false;
            collider.enabled = false;
        }

        public void OnDoorClosed()
        {
            IsAnimating = false;
            collider.enabled = true;
        }
    }
}