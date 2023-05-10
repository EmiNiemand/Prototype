using System;
using System.Collections;
using System.Collections.Generic;
using Music;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class SessionStarter : MonoBehaviour
{
    public GameObject genreButton;
    public GameObject instrumentButton;

    private PlayerManager playerManager;
    private List<GameObject> createdButtons;
    
    public void Setup(PlayerManager manager)
    {
        playerManager = manager;
        createdButtons = new List<GameObject>();
        
        var instruments = manager.GetInstruments();
        var offset = 0;
        foreach (var instrument in instruments)
        {
            AddInstrumentButton(instrument, instruments.Count);
        }
        createdButtons[0].GetComponent<Button>().Select();
    }
    
    private void AddInstrumentButton(Instrument instrument, int count)
    {
        var buttonOffset = 300;
        var xPosBegin = (-count / 2) * buttonOffset + (buttonOffset/2) * ((count+1)%2);
        var xPos = xPosBegin + buttonOffset * createdButtons.Count;
        
        createdButtons.Add(Instantiate(instrumentButton, transform));
        createdButtons[^1].transform.Translate(xPos, 0, 0);
        var button = createdButtons[^1].GetComponent<Button>();
        button.onClick.AddListener(()=> ChooseInstrument(instrument));
        button.transform.Find("Icon").GetComponent<Image>().sprite = instrument.icon;
        button.transform.Find("Name").GetComponent<TextMeshProUGUI>().
            text = instrument.name.ToString();
        button.transform.Find("Genre").GetComponent<TextMeshProUGUI>().
            text = "Genre: "+instrument.genre+" ("+(int)instrument.genre+")";
    }

    private void ChooseInstrument(Instrument instrument)
    {
        foreach (var button in createdButtons) { Destroy(button); }
        createdButtons.Clear();
        
        playerManager.StartSession(instrument);
    }
}
