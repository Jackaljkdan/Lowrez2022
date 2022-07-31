using System;
using System.Collections.Generic;
using UnityEngine;

namespace JK.Injection
{
    /// <summary>
    /// http://answers.unity.com/answers/809221/view.html
    /// </summary>
    [Serializable]
    public class EditorSerliazedDictionary<TKey> : Dictionary<TKey, object>, ISerializationCallbackReceiver
    {
        [SerializeField]
        private List<TKey> keys;

        [SerializeReference]
        private List<object> values;

        [SerializeField]
        private List<TKey> stringKeys;

        [SerializeField]
        private List<string> stringValues;

        [SerializeField]
        private List<TKey> unityObjectKeys;

        [SerializeField]
        private List<UnityEngine.Object> unityObjectValues;

        public void OnBeforeSerialize()
        {
            if (keys == null)
            {
                keys = new List<TKey>();
                values = new List<object>();
                stringKeys = new List<TKey>();
                stringValues = new List<string>();
                unityObjectKeys = new List<TKey>();
                unityObjectValues = new List<UnityEngine.Object>();
            }
            else
            {
                keys.Clear();
                values.Clear();
                stringKeys.Clear();
                stringValues.Clear();
                unityObjectKeys.Clear();
                unityObjectValues.Clear();
            }

            foreach (var pair in this)
            {
                if (pair.Value is string str)
                {
                    stringKeys.Add(pair.Key);
                    stringValues.Add(str);
                }
                else if (pair.Value is UnityEngine.Object obj)
                {
                    unityObjectKeys.Add(pair.Key);
                    unityObjectValues.Add(obj);
                }
                else
                {
                    keys.Add(pair.Key);
                    values.Add(pair.Value);
                }
            }
        }

        public void OnAfterDeserialize()
        {
            Clear();

            for (int i = 0; i < keys.Count; i++)
                Add(keys[i], values[i]);

            for (int i = 0; i < stringKeys.Count; i++)
                Add(stringKeys[i], stringValues[i]);

            for (int i = 0; i < unityObjectKeys.Count; i++)
                Add(unityObjectKeys[i], unityObjectValues[i]);
        }
    }
}