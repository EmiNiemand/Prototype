using System;
using System.Collections.Generic;
using System.Linq;
using Music.Helpers;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

namespace Music
{
    public class Instrument : MonoBehaviour
    {
        public InstrumentName name;
        public Sprite icon;
        public Genre genre;
        public List<Helpers.Pattern> patterns;
        public List<Helpers.Sample> samples;
        public int defaultPatternIndex;

        private AudioSource[] sources;

        public void Setup()
        {
            // sources = GetComponentsInChildren<AudioSource>();
            // for (int i = 0; i < samples.Count; i++)
            // {
            //     sources[i].clip = samples[i].clip;
            // }
        }
    }
}
