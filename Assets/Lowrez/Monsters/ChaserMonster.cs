using JK.Actuators;
using JK.Actuators.Input;
using JK.Injection;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Lowrez.Monsters
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(IMovementActuator))]
    public class ChaserMonster : MonoBehaviour
    {
        #region Inspector

        public float maxDistanceToChase = 2;
        public float maxDistanceToGrab = 0.1f;

        public float grabSecondsToKill = 5;

        [Header("Runtime")]

        public ChaserMonsterState state;

        #endregion
        
        private Transform playerTransform;
        private MovementActuatorInputBehaviour playerMovement;
        private RotationTowardsTargetActuatorInputBehaviour playerRotation;
        private Collider2D playerCollider;

        private void Awake()
        {
            playerTransform = Context.Find(this).Get<Transform>("player");
            playerMovement = Context.Find(this).Get<MovementActuatorInputBehaviour>("player");
            playerRotation = Context.Find(this).Get<RotationTowardsTargetActuatorInputBehaviour>("player");
            playerCollider = Context.Find(this).Get<Collider2D>("player");
        }

        private void Start()
        {
            state = ChaserMonsterState.Idle;
        }

        private bool IsPlayerTooFar()
        {
            return Vector3.Distance(playerTransform.position, transform.position) > maxDistanceToChase;
        }

        private void Update()
        {
            state = StateUpdate();
        }

        private ChaserMonsterState StateUpdate()
        {
            switch (state)
            {
                case ChaserMonsterState.Idle:
                default:
                    return IdleUpdate();
                case ChaserMonsterState.Chasing:
                    return ChaseUpdate();
                case ChaserMonsterState.Grabbing:
                    return GrabUpdate();
            }
        }

        private ChaserMonsterState IdleUpdate()
        {
            if (IsPlayerTooFar())
                return ChaserMonsterState.Idle;
            else
                return ChaserMonsterState.Chasing;
        }

        private ChaserMonsterState ChaseUpdate()
        {
            var movement = GetComponent<IMovementActuator>();
            float distance = Vector3.Distance(playerTransform.position, transform.position);

            if (distance > maxDistanceToGrab && distance <= maxDistanceToChase)
            {
                Vector3 direction = (playerTransform.position - transform.position).normalized;
                direction.z = direction.y;
                direction.y = 0;

                movement.Input = Vector3.Lerp(movement.Input, direction, 0.2f);

                return ChaserMonsterState.Chasing;
            }
            else
            {
                movement.Input = Vector3.zero;

                if (distance <= maxDistanceToGrab)
                    return ChaserMonsterState.Grabbing;
                else
                    return ChaserMonsterState.Idle;
            }
        }

        private float grabSeconds;
        private Vector2 lastInput;

        private ChaserMonsterState GrabUpdate()
        {
            if (playerMovement.enabled)
            {
                playerMovement.enabled = false;
                playerMovement.GetComponent<IMovementActuator>().Input = Vector3.zero;
                playerRotation.enabled = false;

                playerCollider.enabled = false;
                GetComponent<Collider2D>().enabled = false;

                grabSeconds = 0;
                lastInput = Vector2.zero;

                StartCoroutine(BeginGrabCoroutine());
            }

            Vector2 input = new Vector2(
                Input.GetAxisRaw("Horizontal"),
                Input.GetAxisRaw("Vertical")
            );

            if (input.sqrMagnitude != 0)
            {

            }

            return ChaserMonsterState.Grabbing;
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