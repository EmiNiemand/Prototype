using System;
using UnityEngine;

namespace Music.Helpers
{
    [Serializable]
    [CreateAssetMenu(fileName = "Sample", menuName = "Music/Sample", order = 1)]
    public class Sample : ScriptableObject
    {
        public AudioClip clip;
    }
}
