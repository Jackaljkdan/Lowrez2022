using System.Reflection;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

using System;

namespace JK.Lighting
{
    // https://www.youtube.com/watch?v=l8V90A4zKBg
    // https://pastebin.com/QkZFy2qi
    public class ShadowCaster2DFromComposite : MonoBehaviour
    {
        public bool castsShadows = true;
        public bool selfShadows = false;

        static readonly FieldInfo _meshField;
        static readonly FieldInfo _shapePathField;
        static readonly MethodInfo _generateShadowMeshMethod;

        UnityEngine.Rendering.Universal.ShadowCaster2D[] _shadowCasters;

        Tilemap _tilemap;
        CompositeCollider2D _compositeCollider;
        List<Vector2> _compositeVerts = new List<Vector2>();

        /// <summary>
        /// Using reflection to expose required properties in ShadowCaster2D
        /// </summary>
        static ShadowCaster2DFromComposite()
        {
            _meshField = typeof(UnityEngine.Rendering.Universal.ShadowCaster2D).GetField("m_Mesh", BindingFlags.NonPublic | BindingFlags.Instance);
            _shapePathField = typeof(UnityEngine.Rendering.Universal.ShadowCaster2D).GetField("m_ShapePath", BindingFlags.NonPublic | BindingFlags.Instance);

            _generateShadowMeshMethod = typeof(UnityEngine.Experimental.Rendering.Universal.PixelPerfectCamera)
                                        .Assembly
                                        .GetType("UnityEngine.Rendering.Universal.ShadowUtility")
                                        .GetMethod("GenerateShadowMesh", BindingFlags.Public | BindingFlags.Static);
        }

        [ContextMenu("Rebuild")]
        private void RebuildInEditMode()
        {
            bool hadCollider = TryGetComponent(out CompositeCollider2D collider);
            bool hadRigidBody = TryGetComponent(out Rigidbody2D rigidbody2D);

            if (!hadCollider)
            {
                collider = gameObject.AddComponent<CompositeCollider2D>();
                collider.geometryType = CompositeCollider2D.GeometryType.Polygons;

                if (TryGetComponent(out TilemapCollider2D tilemapCollider))
                    tilemapCollider.usedByComposite = true;
            }
            else
            {
                if (collider.geometryType != CompositeCollider2D.GeometryType.Polygons)
                {
                    Debug.LogError("composite collider does not have polygons geometry type");
                    return;
                }
            }


            Rebuild();

            if (!hadCollider)
            {
                DestroyImmediate(collider);

                if (!hadRigidBody)
                    DestroyImmediate(GetComponent<Rigidbody2D>());

                if (TryGetComponent(out TilemapCollider2D tilemapCollider))
                    tilemapCollider.usedByComposite = false;
            }
        }

        /// <summary>
        /// Rebuilds this specific ShadowCaster2DFromComposite
        /// </summary>
        private void Rebuild()
        {
            _compositeCollider = GetComponent<CompositeCollider2D>();
            CreateShadowGameObjects();
            _shadowCasters = GetComponentsInChildren<UnityEngine.Rendering.Universal.ShadowCaster2D>();
            for (int i = 0; i < _compositeCollider.pathCount; i++)
            {
                GetCompositeVerts(i);
            }
        }

        /// <summary>
        /// Automatically creates holder gameobjects for each needed ShadowCaster2D, depending on complexity of tilemap
        /// </summary>
        private void CreateShadowGameObjects()
        {
            //Delete all old objects
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                if (transform.GetChild(i).name.Contains("ShadowCaster"))
                    DestroyImmediate(transform.GetChild(i).gameObject);
            }
            //Generate new ones
            for (int i = 0; i < _compositeCollider.pathCount; i++)
            {
                GameObject newShadowCaster = new GameObject("ShadowCaster");
                newShadowCaster.transform.parent = transform;
                newShadowCaster.AddComponent<UnityEngine.Rendering.Universal.ShadowCaster2D>();
            }
        }

        /// <summary>
        /// Gathers all the verts of a given path shape in a CompositeCollider2D
        /// </summary>
        /// <param name="path">The path index to fetch verts from</param>
        private void GetCompositeVerts(int path)
        {
            _compositeVerts = new List<Vector2>();

            Vector2[] pathVerts = new Vector2[_compositeCollider.GetPathPointCount(path)];
            _compositeCollider.GetPath(path, pathVerts);
            _compositeVerts.AddRange(pathVerts);

            UpdateCompositeShadow(_shadowCasters[path]);
        }

        /// <summary>
        /// Sets the verts of each ShadowCaster2D to match their corresponding
        /// verts in CompositeCollider2D and then generates the mesh
        /// </summary>
        /// <param name="caster"></param>
        private void UpdateCompositeShadow(UnityEngine.Rendering.Universal.ShadowCaster2D caster)
        {
            caster.castsShadows = castsShadows;
            caster.selfShadows = selfShadows;

            Vector2[] points = _compositeVerts.ToArray();
            var threes = ConvertArray(points);

            _shapePathField.SetValue(caster, threes);
            _meshField.SetValue(caster, new Mesh());
            _generateShadowMeshMethod.Invoke(caster,
                new object[] { _meshField.GetValue(caster),
                _shapePathField.GetValue(caster) });
        }

        //Quick method for converting a Vector2 array into a Vector3 array
        Vector3[] ConvertArray(Vector2[] v2)
        {
            Vector3[] v3 = new Vector3[v2.Length];
            for (int i = 0; i < v3.Length; i++)
            {
                Vector2 tempV2 = v2[i];
                v3[i] = new Vector3(tempV2.x, tempV2.y);
            }
            return v3;
        }


    }
}