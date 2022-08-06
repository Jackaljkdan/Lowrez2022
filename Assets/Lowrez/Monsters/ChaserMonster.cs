using JK.Actuators;
using JK.Actuators.Input;
using JK.Injection;
using JK.Utils;
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

        public GrabUpdater grabUpdater = new GrabUpdater();

        [Header("Runtime")]

        public ChaserMonsterState state;

        #endregion
        
        private Transform playerTransform;

        private void Awake()
        {
            playerTransform = Context.Find(this).Get<Transform>("player");

            grabUpdater.Inject(
                this,
                playerTransform,
                Context.Find(this).Get<MovementActuatorInputBehaviour>("player"),
                Context.Find(this).Get<RotationTowardsTargetActuatorInputBehaviour>("player"),
                Context.Find(this).Get<Collider2D>("player")
            );
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

                movement.Input = Vector3.Lerp(movement.Input, direction, TimeUtils.AdjustToFrameRate(0.2f));

                return ChaserMonsterState.Chasing;
            }
            else
            {
                movement.Input = Vector3.zero;

                if (distance <= maxDistanceToGrab)
                {
                    grabUpdater.Start();
                    return ChaserMonsterState.Grabbing;
                }
                else
                {
                    return ChaserMonsterState.Idle;
                }
            }
        }

        private ChaserMonsterState GrabUpdate()
        {
            grabUpdater.Update();

            if (grabUpdater.IsGrabbing)
                return ChaserMonsterState.Grabbing;
            else
                return ChaserMonsterState.Chasing;
        }
    }
}