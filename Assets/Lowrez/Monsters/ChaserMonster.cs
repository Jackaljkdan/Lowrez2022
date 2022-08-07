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

        public float aggroRange = 2;
        public float stopAggroRangeMultiplier = 3;
        public float grabRange = 0.15f;

        public GrabUpdater grabUpdater = new GrabUpdater();

        public float recoverSeconds = 1;

        [Header("Runtime")]

        public ChaserMonsterState state;

        #endregion

        private SignalBus signalBus;

        private Transform playerTransform;

        private void Awake()
        {
            var context = Context.Find(this);
            signalBus = context.Get<SignalBus>();

            playerTransform = context.Get<Transform>("player");

            grabUpdater.Inject(
                this,
                playerTransform,
                context.Get<MovementActuatorInputBehaviour>("player"),
                context.Get<RotationTowardsTargetActuatorInputBehaviour>("player"),
                context.Get<Collider2D>("player")
            );
        }

        private void Start()
        {
            state = ChaserMonsterState.Idle;
        }

        private bool IsPlayerTooFar()
        {
            return Vector3.Distance(playerTransform.position, transform.position) > aggroRange;
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
                case ChaserMonsterState.Recovering:
                    return RecoverUpdate();
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

            if (distance <= aggroRange * stopAggroRangeMultiplier && distance > grabRange)
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

                if (distance > grabRange)
                    return ChaserMonsterState.Idle;

                signalBus.Invoke(new GrabbedSignal());
                grabUpdater.Start();
                return ChaserMonsterState.Grabbing;
            }
        }

        private ChaserMonsterState GrabUpdate()
        {
            grabUpdater.Update();

            if (grabUpdater.IsGrabbing)
                return ChaserMonsterState.Grabbing;

            signalBus.Invoke(new UngrabbedSignal());
            return ChaserMonsterState.Recovering;
        }

        private float elapsedRecoverSeconds = 0;

        private ChaserMonsterState RecoverUpdate()
        {
            elapsedRecoverSeconds += Time.deltaTime;

            if (elapsedRecoverSeconds < recoverSeconds)
                return ChaserMonsterState.Recovering;

            elapsedRecoverSeconds = 0;
            return IdleUpdate();
        }
    }
}