using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Zoom(bool zoomIn = true)
    {
        if (zoomIn)
        {
            //TODO: need to move camera closer when zoomIn = true
            // for exaple, when he started playing on is in shop (maybe)
            //
            // Useful link:
            // https://christopherhilton88.medium.com/building-a-custom-cinemachine-virtual-camera-zoom-feature-with-one-button-in-unity-5a74a2a0a831
            // (just use FindObjectOfType<> instead of GetComponent<>)
        }
        else
        {
            // zoom out when player finished playing
        }
    }
}
