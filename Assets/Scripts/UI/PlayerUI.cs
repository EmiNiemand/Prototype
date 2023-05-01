using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    private TextMeshProUGUI cashText;
    private TextMeshProUGUI repText;
    
    // Start is called before the first frame update
    public void Setup()
    {
        cashText = transform.Find("Cash").GetComponent<TextMeshProUGUI>();
        repText = transform.Find("Rep").GetComponent<TextMeshProUGUI>();
    }

    public void UpdateCashAndRep(int cash, int rep)
    {
        cashText.text = "$$$:  " + cash;
        repText.text = "Rep: " + rep;
    }
}
