using JK.Actuators;
using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Lowrez.Monsters
{
    [Serializable]
    public class ChaseUpdater
    {
        private MovementActuatorBehaviour movementActuator;
        private Transform playerTransform;

        public void Inject(MovementActuatorBehaviour movementActuator, Transform playerTransform)
        {
            this.movementActuator = movementActuator;
            this.playerTransform = playerTransform;
        }

        private static RaycastHit2D[] buffer = new RaycastHit2D[2];

        public void Update()
        {
            Transform transform = movementActuator.transform;

            Vector3 worldDirectionToPlayer = (playerTransform.position - transform.position).normalized;

            float radius = 0.4f;

            int hitCount = Physics2D.CircleCast(
                origin: transform.position,
                radius: radius,
                direction: worldDirectionToPlayer,
                new ContactFilter2D()
                {
                    layerMask = 1,
                    useLayerMask = true,
                },
                buffer
            );

            //Debug.Log("hit c: " + hitCount + " name: " + (hitCount > 1 ? buffer[1].collider.GetName(): "mmffpp"));

            Vector3 targetDirection = worldDirectionToPlayer;

            if (hitCount > 1 && buffer[1].collider.transform != playerTransform)
            {
                Vector3 normalPerpendicular = Vector3.Cross(buffer[1].normal, Vector3.forward);

                //Debug.Log("perp: " + normalPerpendicular + "\n: to p " + worldDirectionToPlayer);

                normalPerpendicular = new Vector3(
                    Mathf.Abs(normalPerpendicular.x),
                    Mathf.Abs(normalPerpendicular.y),
                    Mathf.Abs(normalPerpendicular.z)
                );

                targetDirection = Vector3.Scale(normalPerpendicular, worldDirectionToPlayer).normalized;
            }

            Vector3 direction = transform.InverseTransformDirection(targetDirection);
            direction.z = direction.y;
            direction.y = 0;

            movementActuator.Input = Vector3.Lerp(movementActuator.Input, direction, TimeUtils.AdjustToFrameRate(0.2f));
        }
    }
}