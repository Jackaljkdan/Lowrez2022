using JK.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Interaction
{
    [RequireComponent(typeof(Animator))]
    public class SlidingDoorInteractable : InteractableBehaviour
    {
        #region Inspector

        [SerializeField]
        private bool _isOpen = false;

        [SerializeField]
        private bool _isLocked = false;

        [SerializeField]
        private AudioSource audioSource = null;

        [SerializeField]
        private AudioClip openClip = null;

        [SerializeField]
        private AudioClip closeClip = null;

        [SerializeField]
        private AudioClip lockedClip = null;

        public UnityEvent onInteraction = new UnityEvent();

        #endregion

        public bool IsOpen
        {
            get => _isOpen;
            private set => _isOpen = value;
        }

        public bool IsLocked
        {
            get => _isLocked;
            set => _isLocked = value;
        }

        public bool IsAnimating { get; private set; }

        protected override void PerformInteraction(RaycastHit hit)
        {
            if (IsAnimating)
                return;

            if (IsLocked)
            {
                if (audioSource != null && !audioSource.isPlaying)
                    audioSource.PlayOneShot(lockedClip);

                onInteraction.Invoke();

                return;
            }

            if (!IsOpen)
            {
                GetComponent<Animator>().Play("DoorOpen");
                if (audioSource != null)
                    audioSource.PlayOneShot(openClip);
            }
            else
            {
                GetComponent<Animator>().Play("DoorClose");
                if (audioSource != null)
                    audioSource.PlayOneShot(closeClip);
            }

            IsAnimating = true;

            onInteraction.Invoke();
        }

        public void ForceClose()
        {
            if (IsAnimating)
            {
                if (IsOpen)
                    return;
            }
            else if (!IsOpen)
            {
                return;
            }

            GetComponent<Animator>().Play("DoorClose");
            if (audioSource != null)
                audioSource.PlayOneShot(closeClip);
        }

        public void OnDoorOpened()
        {
            IsAnimating = false;
            IsOpen = true;
        }

        public void OnDoorClosed()
        {
            IsAnimating = false;
            IsOpen = false;
        }
    }
}