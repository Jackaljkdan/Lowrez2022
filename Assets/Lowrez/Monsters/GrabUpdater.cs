using JK.Actuators;
using JK.Actuators.Input;
using JK.Utils;
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
        #region Inspector

        public float grabSecondsToKill = 5;

        public float inputBreakAmount = 0.1f;
        public float refillBreakAmount = 0.01f;

        public float breakPushLerp = 0.2f;

        [Header("Runtime")]

        public float breakValue;

        [field: SerializeField]
        public bool IsGrabbing { get; private set; }

        [field: SerializeField]
        public bool IsBroken { get; private set; }

        #endregion


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
        private Vector3 grabDirection;

        public void Start()
        {
            playerMovement.enabled = false;
            playerMovement.GetComponent<IMovementActuator>().Input = Vector3.zero;
            playerRotation.enabled = false;

            playerCollider.enabled = false;
            grabber.GetComponent<Collider2D>().enabled = false;

            grabSeconds = 0;  // TODO: punto di partenza sulla base di salute del giocatore
            breakValue = 0;
            lastInput = Vector2.zero;

            grabDirection = (playerTransform.position - transform.position).normalized;
            grabber.StartCoroutine(BeginGrabCoroutine());

            IsGrabbing = true;
            IsBroken = false;
        }

        private IEnumerator BeginGrabCoroutine()
        {
            while (Vector3.Distance(transform.position, playerTransform.position) > 0.01f)
            {
                transform.position = Vector3.Lerp(transform.position, playerTransform.position, TimeUtils.AdjustToFrameRate(0.2f));
                yield return null;
            }
        }

        public void Update()
        {
            if (IsBroken)
                return;

            Vector2 input = new Vector2(
                Input.GetAxisRaw("Horizontal"),
                Input.GetAxisRaw("Vertical")
            );

            grabSeconds += Time.deltaTime;
            breakValue = Mathf.Max(0, breakValue - TimeUtils.AdjustToFrameRate(refillBreakAmount));

            //if (grabSeconds >= grabSecondsToKill)
            //    return;  // TODO: morte

            if (input.sqrMagnitude == 0)
                return;

            if (input == lastInput)
                return;

            lastInput = input;
            breakValue += inputBreakAmount;

            if (breakValue >= 1)
                Stop();
        }

        public void Stop()
        {
            IsBroken = true;

            grabber.StartCoroutine(StopGrabCoroutine());
            grabber.StartCoroutine(ReEnableStuffCoroutine());
        }

        private IEnumerator ReEnableStuffCoroutine()
        {
            playerRotation.enabled = true;

            yield return new WaitForSeconds(0.1f);

            playerMovement.enabled = true;
            playerCollider.enabled = true;
            grabber.GetComponent<Collider2D>().enabled = true;
        }

        private IEnumerator StopGrabCoroutine()
        {
            var movement = grabber.GetComponent<IMovementActuator>();
            var formerSpeed = movement.Speed;
            movement.Speed *= 4;
            movement.Input = -grabDirection;

            float t = 1;

            while (t > 0.01f)
            {
                yield return null;
                t = Mathf.Lerp(t, 0, TimeUtils.AdjustToFrameRate(breakPushLerp));
            }

            movement.Input = Vector3.zero;
            movement.Speed = formerSpeed;
            IsGrabbing = false;
            IsBroken = false;
        }
    }
}