using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

namespace Music
{
    public class Session : MonoBehaviour
    {
        public Instrument instrument;
        private int bpm;
        
        private SessionUI sessionUI;
        private PlayerManager playerManager;

        private List<Helpers.Sound> recordedSounds;
        private List<Helpers.Pattern> potentialPatterns;
        private float lastTime = 0;

        public void Setup(PlayerManager manager, Instrument playerInstrument)
        {
            recordedSounds = new List<Helpers.Sound>();
            potentialPatterns = new List<Helpers.Pattern>();
            
            instrument = playerInstrument;
            instrument.Setup();
            
            bpm = (int)instrument.genre;
            
            playerManager = manager;
            
            sessionUI = FindObjectOfType<SessionUI>();
            sessionUI.Setup(bpm, instrument.samples);
        }

        private float timeoutCounter = 0.0f;
        private float timeout = 1.0f;
        private void FixedUpdate()
        {
            timeoutCounter += Time.fixedDeltaTime;
            
            if (recordedSounds.Count > 0 && timeoutCounter > timeout)
                PatternTimeout();
        }

        #region Playing sound & detecting pattern
        public void PlaySample(int index)
        {
            if (instrument.samples.Count-1 < index) return;
            
            sessionUI.PlaySound(index);
            
            timeoutCounter = 0;
            
            float currentTime = Time.time;
            float rhythmDiff = GetRhythmValue(currentTime - lastTime);
            
            recordedSounds.Add(new Helpers.Sound(instrument.samples[index], rhythmDiff));
            lastTime = currentTime;

            DetectPattern();
        }

        private void DetectPattern()
        {
            // Initialize potential patterns list
            // ----------------------------------
            if(potentialPatterns.Count == 0)
            {
                var lastItem = recordedSounds[^1];
                recordedSounds.Clear();
                recordedSounds.Add(lastItem);
                
                foreach (var pattern in instrument.patterns.Where(
                             pattern => recordedSounds[0].sample == pattern.sounds[0].sample))
                {
                    potentialPatterns.Add(pattern);
                }
                return;
            }
            
            // Remove patterns that don't match anymore
            // ----------------------------------------
            int lastIndex = recordedSounds.Count - 1;
            potentialPatterns.RemoveAll(pattern => 
                pattern.sounds[lastIndex].sample != recordedSounds[lastIndex].sample);

            // User played sequence that doesn't match any patterns
            // ----------------------------------------------------
            if (potentialPatterns.Count == 0)
            {
                Debug.Log("No patterns match");
                PatternFail();
                return;
            }
            // User played correct pattern ^^
            // ------------------------------
            if (potentialPatterns.Exists(item =>
                    item.sounds.Count == recordedSounds.Count))
                CalcAccuracyAndReset();
        }

        private void CalcAccuracyAndReset()
        {
            float accuracy = 0;
            var goodPattern = potentialPatterns.Find(item =>
                item.sounds.Count == recordedSounds.Count);
            for (int i = 0; i < recordedSounds.Count; i++)
            {
                float recordedDelay = i == 0 ? 0 : recordedSounds[i].delay;
                accuracy += Mathf.Abs(goodPattern.sounds[i].delay - recordedDelay);
            }

            accuracy /= (float)recordedSounds.Count;
            Debug.Log("Accuracy: " + (1 - accuracy) * 100 + "%");
            PatternSuccess(goodPattern, 1 - accuracy);
            
            recordedSounds.Clear();
            potentialPatterns.Clear();
        }
        
        #endregion

        #region Session logic
        private void PatternSuccess(Helpers.Pattern pattern, float accuracy)
        {
            playerManager.PlayedPattern(pattern);
            //TODO: play some kind of success sound
                // sessionUI.DiscoveredPattern();
            
            sessionUI.UpdateAccuracy(accuracy);
        }

        private void PatternTimeout()
        {
            recordedSounds.Clear(); 
            potentialPatterns.Clear();
            
            playerManager.PlayedPattern(null);
            //TODO: play timeout sound 
        }

        private void PatternFail()
        {
            sessionUI.UpdateAccuracy(0);
            playerManager.PlayedPattern(null);
            //TODO: play some kind of failure sound (record scratch)
        }
        #endregion
        
        #region Helper methods

        private float GetRhythmValue(float currentNoteLength)
        {
            return currentNoteLength * (bpm/60.0f);
        }
        #endregion
    }
}
