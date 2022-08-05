using JK.Actuators;
using JK.Actuators.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Lowrez.Monsters
{
    [Serializable]
    public class GrabUpdater
    {
        public float grabSecondsToKill = 5;

        public bool IsGrabbing { get; private set; }

        private MonoBehaviour grabber;
        private Transform transform;
        private Transform playerTransform;
        private MovementActuatorInputBehaviour playerMovement;
        private RotationTowardsTargetActuatorInputBehaviour playerRotation;
        private Collider2D playerCollider;

        public void Inject(
            MonoBehaviour grabber,
            Transform playerTransform,
            MovementActuatorInputBehaviour playerMovement,
            RotationTowardsTargetActuatorInputBehaviour playerRotation,
            Collider2D playerCollider
        )
        {
            this.grabber = grabber;
            transform = grabber.transform;
            this.playerTransform = playerTransform;
            this.playerMovement = playerMovement;
            this.playerRotation = playerRotation;
            this.playerCollider = playerCollider;
        }

        private float grabSeconds;
        private Vector2 lastInput;

        public void Start()
        {
            playerMovement.enabled = false;
            playerMovement.GetComponent<IMovementActuator>().Input = Vector3.zero;
            playerRotation.enabled = false;

            playerCollider.enabled = false;
            transform.GetComponent<Collider2D>().enabled = false;

            grabSeconds = 0;
            lastInput = Vector2.zero;

            grabber.StartCoroutine(BeginGrabCoroutine());

            IsGrabbing = true;
        }

        public void Update()
        {
            Vector2 input = new Vector2(
                Input.GetAxisRaw("Horizontal"),
                Input.GetAxisRaw("Vertical")
            );

            if (input.sqrMagnitude != 0)
            {

            }
        }

        private IEnumerator BeginGrabCoroutine()
        {
            while (Vector3.Distance(transform.position, playerTransform.position) > 0.01f)
            {
                transform.position = Vector3.Lerp(transform.position, playerTransform.position, 0.2f);
                yield return null;
            }
        }
    }
}