using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    private GameObject shop;
    
    private void Start()
    {
        shop = transform.GetChild(0).gameObject;
    }

    
}
