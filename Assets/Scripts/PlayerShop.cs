using System;
using System.Collections.Generic;
using Music;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShop : MonoBehaviour, IUsable
{
    public List<Instrument> instruments;
    
    private bool isActive = false;
    private GameObject canvas;
    private PlayerManager playerManager;

    private void Start()
    {
        canvas = GameObject.Find("Interactive").transform.GetChild(0).GetChild(0).GameObject();
        canvas.SetActive(false);
        playerManager = FindObjectOfType<PlayerManager>();
    }
    
    public void OnEnter(GameObject player)
    {
        ShowShop();
    }

    public void OnUse(GameObject player)
    {
        //TODO: hide interaction indicator
        //TODO: show (toggle?) shop interface
    }

    public void OnExit(GameObject player)
    {
        HideShop();
    }
    

    void ShowShop()
    {
        isActive = true;
        Cursor.lockState = CursorLockMode.Confined;
        canvas.SetActive(true);
    }

    void HideShop()
    {
        isActive = false;
        Cursor.lockState = CursorLockMode.Locked;
        canvas.SetActive(false);
    }

    public void BuyInstrument()
    {
        if (playerManager.BuyInstrument(100, instruments[0]))
        {
            Debug.Log("Kupione");
        }
        else
        {
            Debug.Log("Nie masz hajsu");
        }
    }
}
