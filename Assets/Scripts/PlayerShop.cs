using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerShop : MonoBehaviour, IUsable
{
    private bool isActive = false;
    private GameObject canvas;

    private void Start()
    {
        canvas = GameObject.Find("Shop");
        canvas.SetActive(false);
    }
    
    public void OnEnter(GameObject player)
    {
        Debug.Log("Entered shop interaction zone!");
        //TODO: show interaction indicator
    }

    public void OnUse(GameObject player)
    {
        //TODO: hide interaction indicator
        //TODO: show (toggle?) shop interface
    }

    public void OnExit(GameObject player)
    {
        Debug.Log("Left shop interaction zone!");
        //TODO: hide interaction indicator
    }
    

    public void ShowShop()
    {
        if (isActive)
        {
            isActive = false;
            Cursor.lockState = CursorLockMode.Locked;
            canvas.SetActive(false);
        }
        else
        {
            isActive = true;
            Cursor.lockState = CursorLockMode.Confined;
            canvas.SetActive(true);
        }
    }
}
