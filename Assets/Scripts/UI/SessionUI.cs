using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SessionUI : MonoBehaviour
{
    [SerializeField] private Gradient accuracyGradient;
    [SerializeField] private GameObject sampleSourcePrefab;

    private Slider accuracy;
    private Image accuracyColor;
    
    private Animator animator;
    private AudioSource audioSource;
    private List<AudioSource> sampleSources;

    public void Setup(int bpm, List<Music.Helpers.Sample> samples)
    {
        accuracy = transform.Find("Accuracy").GetComponent<Slider>();
        accuracyColor = accuracy.transform.Find("Fill Area").Find("Fill").GetComponent<Image>();
        
        animator = GetComponent<Animator>();
        animator.SetFloat("MetronomeScale", bpm/60.0f);
        
        audioSource = GetComponent<AudioSource>();

        sampleSources = new List<AudioSource>();
        foreach (var sample in samples)
        {
            sampleSources.Add(Instantiate(sampleSourcePrefab, transform).GetComponent<AudioSource>());
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
        Debug.Log(fraction);
        accuracy.value = fraction;
        accuracyColor.color = accuracyGradient.Evaluate(fraction);
    }
    
    #region Animation Events
    public void AE_MetronomeTick()
    {
        audioSource.Play();
    }
    #endregion
}
