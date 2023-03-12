using System.Collections;
using System.Collections.Generic;
using Music;
using UnityEngine;
using UnityEngine.UI;

public class SessionStarter : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Setup(PlayerManager manager)
    {
        var instruments = manager.GetInstruments();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
