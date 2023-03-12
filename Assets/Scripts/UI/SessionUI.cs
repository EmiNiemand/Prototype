using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SessionUI : MonoBehaviour
{
    [SerializeField] private Gradient accuracyGradient;

    private Slider accuracy;
    private Image accuracyColor;
    
    private Animator animator;
    private AudioSource audioSource;

    public void Setup(int bpm)
    {
        accuracy = transform.Find("Accuracy").GetComponent<Slider>();
        accuracyColor = transform.Find("Accuracy").Find("Fill Area").Find("Fill").GetComponent<Image>();
        
        animator = GetComponent<Animator>();
        animator.SetFloat("MetronomeScale", bpm/60.0f);
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(int index)
    {
        animator.SetTrigger("Sound"+index);
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
