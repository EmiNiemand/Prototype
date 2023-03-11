using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerShop : MonoBehaviour
{
    private bool isActive = false;
    private GameObject canvas;

    private void Start()
    {
        canvas = GameObject.Find("Shop");
        canvas.SetActive(false);
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
