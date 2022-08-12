using JK.Actuators;
using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Sounds
{
    public class FootstepsOnMovementActuator : MonoBehaviour
    {
        #region Inspector

        [SerializeField]
        private MovementActuatorBehaviour movementActuator;

        public float secondsBetweenSteps = 0.5f;

        public List<AudioClip> clips = null;

        public List<AudioSource> audioSources;

        private void Reset()
        {
            audioSources = new List<AudioSource>();
            GetComponentsInChildren(audioSources);
        }

        #endregion

        private int lastSourceIndex = 0;
        private bool wasMoving = false;
        private float secondsSinceFootstep = 0;
        private float referenceSpeed;

        private void Awake()
        {
            referenceSpeed = movementActuator.Speed;
        }

        private void Start()
        {
            movementActuator.onMovement.AddListener(OnMovement);
        }

        private void OnMovement(Vector3 velocity)
        {
            if (velocity.sqrMagnitude > 0)
            {
                float speedFactor = movementActuator.Speed / referenceSpeed;
                float proportionalSecondsBetweenSteps = secondsBetweenSteps / speedFactor;

                if (!wasMoving)
                {
                    float halfSecondsBetween = proportionalSecondsBetweenSteps / 2;
                    if (secondsSinceFootstep < halfSecondsBetween)
                        secondsSinceFootstep = halfSecondsBetween;

                    wasMoving = true;
                }

                secondsSinceFootstep += Time.deltaTime;

                if (secondsSinceFootstep >= proportionalSecondsBetweenSteps)
                {
                    secondsSinceFootstep = 0;
                    PlayFootstep();
                }
            }
            else if (wasMoving)
            {
                wasMoving = false;
            }
        }

        private void PlayFootstep()
        {
            lastSourceIndex = (lastSourceIndex + 1) % audioSources.Count;
            audioSources[lastSourceIndex].PlayRandomClip(clips, oneShot: true);
        }

        private void OnDestroy()
        {
            movementActuator.onMovement.RemoveListener(OnMovement);
        }
    }
}