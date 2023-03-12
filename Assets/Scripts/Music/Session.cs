using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Music
{
    public class Session : MonoBehaviour
    {
        public Instrument instrument;
        
        private int bpm;
        private SessionUI sessionUI;
        
        //TODO: move to player
        private HashSet<Helpers.Pattern> knownPatterns;
        
        private List<Helpers.Sound> recordedSounds;
        private List<Helpers.Pattern> potentialPatterns;
        private float lastTime = 0;
        
        public void Start()
        {
            knownPatterns = new HashSet<Helpers.Pattern>();
            
            recordedSounds = new List<Helpers.Sound>();
            potentialPatterns = new List<Helpers.Pattern>();
            
            instrument = FindObjectOfType<Instrument>();
            instrument.Setup();
            
            bpm = (int)instrument.genre;
            
            sessionUI = FindObjectOfType<SessionUI>();
            sessionUI.Setup(bpm);
        }

        #region Playing sound & detecting pattern
        public void PlaySample(int index)
        {
            if (!instrument.PlaySample(index)) return; 
            
            sessionUI.PlaySound(index);
            
            float currentTime = Time.time;
            float rhythmDiff = GetRhythmValue(currentTime - lastTime);
            // Reset patterns and sounds if player waited for too long
            // -------------------------------------------------------
            if (rhythmDiff > 2)
            {
                recordedSounds.Clear(); potentialPatterns.Clear(); PatternFail();
            }
            
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
            //TODO: implement needed functions
            // if (player.AddPattern(pattern)) sessionUI.DiscoveredPattern();
            if(knownPatterns.Add(pattern))
                sessionUI.DiscoveredPattern();
            Debug.Log(knownPatterns.Count);
            
            sessionUI.UpdateAccuracy(accuracy);
        }

        private void PatternFail()
        {
            sessionUI.UpdateAccuracy(0);
        }
        #endregion
        
        #region Helper methods
        private float GetRhythmValue(float currentNoteLength)
        {
            return currentNoteLength * (bpm/60.0f);
        }
        #endregion
        
        #region Inputs
        //TODO: dis stupid, improve somehow?
        public void OnSound1(InputAction.CallbackContext context)
        {
            if (!context.started) return;
            PlaySample(0);
        }
        
        public void OnSound2(InputAction.CallbackContext context)
        {
            if (!context.started) return;
            PlaySample(1);
        }
        
        public void OnSound3(InputAction.CallbackContext context)
        {
            if (!context.started) return;
            PlaySample(2);
        }
        
        public void OnSound4(InputAction.CallbackContext context)
        {
            if (!context.started) return;
            PlaySample(3);
        }
        #endregion
    }
}
