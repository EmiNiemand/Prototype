using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SessionUI : MonoBehaviour
{
    [SerializeField] private GameObject sampleSourcePrefab;
    
    private TextMeshProUGUI accuracyFeedback;
    private readonly Color[] accuracyColors = { Color.red, Color.cyan, Color.green, Color.yellow };
    private readonly string[] accuracyTexts = { "Poor", "Nice", "Great!", "PERFECT" };
    private readonly float[] accuracyThresholds = { /*0.0f,*/0.5f, 0.8f, 0.95f };
    
    private Animator animator;
    private AudioSource audioSource;
    private List<AudioSource> sampleSources;

    public void Setup(int bpm, List<Music.Helpers.Sample> samples)
    {
        accuracyFeedback = transform.Find("AccuracyFeedback").GetComponent<TextMeshProUGUI>();
        
        animator = GetComponent<Animator>();
        animator.SetFloat("MetronomeScale", bpm/60.0f);
        
        audioSource = GetComponent<AudioSource>();

        sampleSources = new List<AudioSource>();
        foreach (var sample in samples)
        {
            sampleSources.Add(Instantiate(sampleSourcePrefab, transform)
                .GetComponent<AudioSource>());
            sampleSources[^1].clip = sample.clip;
        }
    }

    public void PlaySound(int index)
    {
        animator.SetTrigger("Sound"+index);
        sampleSources[index].Play();
    }

    public void DiscoveredPattern()
    {
        animator.SetTrigger("DiscoveredPattern");
    }

    // Fraction values: <0, 1>
    public void UpdateAccuracy(float fraction)
    {
        ShowAccuracyFeedback(fraction);
        return;
    }
    
    private void ShowAccuracyFeedback(float fraction)
    {
        int index = accuracyThresholds.Count(threshold => fraction > threshold);
        accuracyFeedback.text = accuracyTexts[index];
        accuracyFeedback.color = accuracyColors[index];
        animator.SetTrigger("ShowAccuracy");
    }
    
    #region Animation Events
    public void AE_MetronomeTick()
    {
        audioSource.Play();
    }
    #endregion
}
