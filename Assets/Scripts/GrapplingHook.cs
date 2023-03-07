using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    [SerializeField] private GameObject Camera;
    [SerializeField] private float HookRange;
    private LineRenderer _line;
    
    void Start()
    {
        _line = GetComponent<LineRenderer>();
    }
    
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Vector3 lineStart = _line.GetPosition(0);

            RaycastHit hit;

            if (Physics.Raycast(lineStart, Camera.transform.forward, out hit, HookRange))
            {
                _line.SetPosition(1, hit.point);
            }
            else
            {
                _line.SetPosition(1, lineStart + (Camera.transform.forward * HookRange));
            }
        }
    }
}
