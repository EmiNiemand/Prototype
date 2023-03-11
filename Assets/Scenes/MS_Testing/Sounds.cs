using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Sounds : MonoBehaviour
{
    public AudioClip[] soundClips;
    public Image[] images;

    private AudioSource source;
    private readonly Color[] colors = { Color.green, Color.magenta, Color.yellow};

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void OnSound1(InputAction.CallbackContext context)
    {
        if(!context.started) return;
        StartCoroutine(PlaySound(0));
    }
    
    public void OnSound2(InputAction.CallbackContext context)
    {
        if(!context.started) return;
        StartCoroutine(PlaySound(1));
    }
    
    public void OnSound3(InputAction.CallbackContext context)
    {
        if(!context.started) return;
        StartCoroutine(PlaySound(2));
    }

    private IEnumerator PlaySound(int index)
    {
        images[index].color = colors[index];
        source.clip = soundClips[index];
        source.Play();
        yield return new WaitUntil(() => !source.isPlaying);
        images[index].color = Color.white;
    }
}
