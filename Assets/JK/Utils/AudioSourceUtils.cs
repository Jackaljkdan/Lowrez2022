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
    }
}