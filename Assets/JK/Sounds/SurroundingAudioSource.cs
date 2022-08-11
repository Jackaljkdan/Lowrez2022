using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Sounds
{
    [DisallowMultipleComponent]
    public class SurroundingAudioSource : MonoBehaviour
    {
        #region Inspector

        public float minSecondsBetweenSounds = 0;
        public float maxSecondsBetweenSounds = 1;

        public List<AudioClip> clips;
        public List<AudioClip> clipsBk;

        public AudioSource audioSource;

        [SerializeField]
        private List<Path> paths;

        private void Reset()
        {
            audioSource = GetComponent<AudioSource>();
        }

        #endregion

        [Serializable]
        struct Path
        {
            public Transform start;
            public Transform end;
        }

        private Coroutine coroutine;

        private void OnEnable()
        {
            coroutine = StartCoroutine(PlaySoundsCoroutine());
        }

        private void OnDisable()
        {
            if (coroutine != null)
                StopCoroutine(coroutine);
        }

        private void PlayRandom()
        {
            int randomIndex = UnityEngine.Random.Range(0, clips.Count);
            
            audioSource.clip = clips[randomIndex];
            audioSource.time = 0;
            audioSource.Play();
        }

        private void SelectStartAndEnd(out Transform start, out Transform end)
        {
            int randomPathIndex = UnityEngine.Random.Range(0, paths.Count);
            var path = paths[randomPathIndex];

            int random = UnityEngine.Random.Range(0, 2);
            start = random == 0 ? path.start : path.end;
            end = random == 0 ? path.end : path.start;
        }
        
        IEnumerator PlaySoundsCoroutine()
        {
            Transform tr = transform;

            while (true)
            {
                yield return new WaitForSeconds(UnityEngine.Random.Range(minSecondsBetweenSounds, maxSecondsBetweenSounds));

                SelectStartAndEnd(out Transform start, out Transform end);
                PlayRandom();

                while (audioSource.isPlaying)
                {
                    tr.position = Vector3.Lerp(start.position, end.position, audioSource.time / audioSource.clip.length);
                    yield return null;
                }
            }
        }
    }
}