using System;
using System.Collections.Generic;
using UnityEngine;

namespace Music.Helpers
{
    [Serializable]
    public struct Sound
    {
        //TODO: maybe instead of this make it a list 
        public Sample sample;
        // Delay from the previously played sound; 1 = 1 metronome tact
        public float delay;

        public Sound(Sample nSample, float nDelay) { sample = nSample; delay = nDelay; }
    }
    
    [CreateAssetMenu(fileName = "Pattern", menuName = "Music/Pattern", order = 1)]
    public class Pattern : ScriptableObject
    {
        public List<Sound> sounds;
    }
}
