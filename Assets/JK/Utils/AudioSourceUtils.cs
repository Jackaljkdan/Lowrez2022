using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    public static class AudioSourceUtils
    {
        public static void PlayOneShotSafely(this AudioSource source, AudioClip clip)
        {
            if (source != null && clip != null)
                source.PlayOneShot(clip);
        }

        public static AudioClip PlayRandomClip(this AudioSource source, List<AudioClip> clips, bool oneShot)
        {
            int randomIndex = UnityEngine.Random.Range(0, clips.Count);
            AudioClip randomClip = clips[randomIndex];

            if (oneShot)
            {
                source.PlayOneShot(randomClip);
            }
            else
            {
                source.clip = randomClip;
                source.Play();
            }

            return randomClip;
        }
    }
}