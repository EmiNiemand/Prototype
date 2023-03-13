using System;
using System.Collections.Generic;
using Music;
using UnityEngine;
using TMPro;

public class PlayerShop : MonoBehaviour, IUsable
{
    public List<Instrument> instruments;
    
    private bool isActive = false;
    private GameObject canvas;
    //stupid workaround because disabling GameObject hogs performance
    private TextMeshProUGUI indicatorText;
    private string indicatorString;
    
    private PlayerManager playerManager;

    private void Start()
    {
        indicatorString = "[E] Buy instrument";
        
        canvas = transform.Find("Shop").gameObject;
        canvas.SetActive(false);

        indicatorText = transform.Find("Indicator").GetComponentInChildren<TextMeshProUGUI>();
        indicatorText.text = string.Empty;
        
        playerManager = FindObjectOfType<PlayerManager>();
    }
    
    public void OnEnter(GameObject player) { indicatorText.text = indicatorString; }

    public void OnUse(GameObject player) { ToggleShop(); }

    public void OnExit(GameObject player)
    {
        indicatorText.text = string.Empty;
        if(isActive) ToggleShop();
    }

    public void ToggleShop() {
        isActive = !isActive;
        canvas.SetActive(isActive);
    }

    public void BuyInstrument()
    {
        if (playerManager.BuyInstrument(100, instruments[0]))
        {
            Debug.Log("Kupione");
            //TODO: add some cash sound effect
        }
        else
        {
            Debug.Log("Nie masz hajsu");
        }
    }
}
