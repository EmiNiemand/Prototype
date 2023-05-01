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
    [SerializeField] private float moveSpeed;
    [SerializeField] private float lerpSpeed;
    
    private new Rigidbody rigidbody;

    private Vector2 moveVector;
    
    // Start is called before the first frame update
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rigidbody.MovePosition(Vector3.Lerp(rigidbody.position, 
            rigidbody.position + rigidbody.rotation * new Vector3(moveVector.x, 0, moveVector.y), lerpSpeed));
    }

    public void Move(Vector2 newMoveVector)
    {
        moveVector = newMoveVector * moveSpeed;
    }
}
