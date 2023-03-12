using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

/// <summary>
/// Responsible for basic player movement and camera control
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    
    private new Rigidbody rigidbody;

    private Vector2 moveVector;
    
    // Start is called before the first frame update
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        //TODO: improve this
        rigidbody.MovePosition(rigidbody.position + 
                               rigidbody.rotation * 
                               new Vector3(moveVector.x, 0, moveVector.y) * 
                               Time.deltaTime);
    }

    public void Move(Vector2 newMoveVector)
    {
        moveVector = newMoveVector * speed;
    }
}
