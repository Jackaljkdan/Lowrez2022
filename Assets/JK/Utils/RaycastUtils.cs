using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JK.Utils
{
    public static class RaycastUtils
    {
        public static bool Cast(Vector3 from, Transform to, out RaycastHit hit, float maxDistance = float.PositiveInfinity)
        {
            LayerMask layerMask = to.gameObject.layer;
            return Physics.Raycast(from, to.position - from, out hit, maxDistance, layerMask);
        }

        public static bool Cast(Vector3 from, Transform to, out RaycastHit hit, LayerMask layerMask, float maxDistance = float.PositiveInfinity)
        {
            return Physics.Raycast(from, to.position - from, out hit, maxDistance, layerMask);
        }

        public static bool IsHit<T>(Vector3 from, T target, out RaycastHit hit, float maxDistance = float.PositiveInfinity) where T : MonoBehaviour
        {
            LayerMask layerMask = target.gameObject.layer;
            return IsHit(from, target, out hit, layerMask, maxDistance);
        }

        public static bool IsHit<T>(Vector3 from, T target) where T : MonoBehaviour
        {
            return IsHit(from, target, out RaycastHit _);
        }

        public static bool IsHit<T>(Vector3 from, T target, LayerMask layerMask, float maxDistance = float.PositiveInfinity) where T : MonoBehaviour
        {
            return IsHit(from, target, out RaycastHit _, layerMask, maxDistance);
        }

        public static bool IsHit<T>(Vector3 from, T target, out RaycastHit hit, LayerMask layerMask, float maxDistance = float.PositiveInfinity) where T : MonoBehaviour
        {
            if (Cast(from, target.transform, out hit, layerMask, maxDistance))
            {
                if (hit.collider.TryGetComponent(out T hitComponent))
                    if (hitComponent == target)
                        return true;

                if (hit.collider.TryGetComponent(out IRef<T> hitRef))
                    return hitRef.Ref == target;
            }

            return false;
        }

    }
}