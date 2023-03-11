using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (enemies.Length == 0) return;
            foreach (var enemy in enemies)
            {
                if (Vector3.Distance(transform.position, enemy.transform.position) < 50)
                {
                    if (enemy.GetComponent<Enemy>()) enemy.GetComponent<Enemy>().startBeingAlarmedEvent.Invoke(gameObject);
                }
            }
        }
    }
}