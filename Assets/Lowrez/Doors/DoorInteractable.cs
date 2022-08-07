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

        public bool IsOpen { get; private set; }

        private void Start()
        {
            animator.Play(startsOpen ? openAnimationName : closeAnimationName, 0, 1);

            if (startsOpen)
                OnDoorOpened();
            else
                OnDoorClosed();
        }

        private void PlayAudioClip(AudioClip clip)
        {
            if (audioSource != null && clip != null)
                audioSource.PlayOneShot(clip);
        }

        protected override void PerformInteraction(RaycastHit hit)
        {
            if (!IsOpen)
                OpenAsInteraction();
            else
                CloseAsInteraction();
        }

        public void Open()
        {
            OpenOrClose(open: true, isInteraction: false);
        }

        public void OpenAsInteraction()
        {
            OpenOrClose(open: true, isInteraction: true);
        }

        public void Close()
        {
            OpenOrClose(open: false, isInteraction: false);
        }

        public void CloseAsInteraction()
        {
            OpenOrClose(open: false, isInteraction: true);
        }

        private float GetTargetNormalizedTime()
        {
            float currentNormalizedTime = Mathf.Clamp01(animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            return IsAnimating ? 1 - currentNormalizedTime : 0;
        }

        private void OpenOrClose(bool open, bool isInteraction)
        {
            if (isInteraction && isLocked)
            {
                PlayAudioClip(lockedClip);
                onInteraction.Invoke();
                return;
            }

            Debug.Log($"door action: isopen = {IsOpen} wantopen = {open} anim = {IsAnimating} tnt = {GetTargetNormalizedTime()}");

            if (IsOpen == open)
                return;

            animator.Play(open ? openAnimationName : closeAnimationName, 0, GetTargetNormalizedTime());
            PlayAudioClip(open ? openClip : closeClip);
            IsOpen = open;
            IsAnimating = true;

            if (!open)
                collider.enabled = true;

            if (isInteraction)
                onInteraction.Invoke();
        }

        public void OnDoorOpened()
        {
            IsAnimating = false;
            collider.enabled = false;
            IsOpen = true;
        }

        public void OnDoorClosed()
        {
            IsAnimating = false;
            collider.enabled = true;
            IsOpen = false;
        }
    }
}