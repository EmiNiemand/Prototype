using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    private List<GameObject> usables;
    
    // Start is called before the first frame update
    void Start()
    {
        usables = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == (int)Layers.Usable)
        {
            other.GetComponent<IUsable>().OnEnter(gameObject);
            usables.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == (int)Layers.Usable)
        {
            other.GetComponent<IUsable>().OnExit(gameObject);
            usables.Remove(other.gameObject);
        }
    }

    public void OnUse()
    {
        if (usables.Count == 0) return;
        usables[^1].GetComponent<IUsable>().OnUse(gameObject);
    }
}
