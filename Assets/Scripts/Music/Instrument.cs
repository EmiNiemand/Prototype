using System;
using System.Collections.Generic;
using System.Linq;
using Music.Helpers;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

namespace Music
{
    //Genre value is its BPM 
    public enum Genre
    {
        Default = 60,
        Jazz = 80,
        Pop = 120,
        Rock = 140
    }

    public class Instrument : MonoBehaviour
    {
        public Genre genre;
        public List<Pattern> patterns;
        public List<Sample> samples;
        public int defaultPatternIndex;

        private AudioSource[] sources;

        public void Setup()
        {
            sources = GetComponentsInChildren<AudioSource>();
            for (int i = 0; i < samples.Count; i++)
            {
                sources[i].clip = samples[i].clip;
            }
        }

        public bool PlaySample(int index)
        {
            if (sources.Length <= index) return false;
            
            sources[index].Play();
            return true;
        }
    }
}