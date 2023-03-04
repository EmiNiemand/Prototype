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
    [SerializeField] private Vector2 sensitivity;
    [SerializeField] private float jumpForce = 100;
    
    private new Rigidbody rigidbody;
    private Transform cameraPivot;

    private Vector2 moveVector;

    // Workaround to big mouse delta at beginning of game
    private bool freezeInput = true;
    private bool grounded = true;
    
    // Start is called before the first frame update
    void Awake()
    {
        freezeInput = true;
        rigidbody = GetComponent<Rigidbody>();
        cameraPivot = transform.Find("CameraPivot");
        Cursor.lockState = CursorLockMode.Locked;
        StartCoroutine(FreezeInput());
    }

    private void Update()
    {
        //TODO: improve this
        rigidbody.MovePosition(rigidbody.position + 
                               rigidbody.rotation * 
                               new Vector3(moveVector.x, 0, moveVector.y) * 
                               Time.deltaTime);
    }

    IEnumerator FreezeInput()
    {
        yield return new WaitForSeconds(0.1f);
        freezeInput = false;
    }

    public void RotateCamera(Vector2 delta)
    {
        if (freezeInput) return;
        delta *= sensitivity;
        
        transform.Rotate(Vector3.up, delta.x);
        //TODO: Block Y rotation to prevent overturn
        // var diffX = cameraPivot.localRotation.eulerAngles.x - delta.y;
        // Debug.Log(diffX);
        // if ((diffX > 180 && diffX < 275) ||
        //     (diffX < 180 && diffX > 85)) return;
        cameraPivot.Rotate(Vector3.right, -delta.y);
    }

    public void Move(Vector2 newMoveVector)
    {
        moveVector = newMoveVector * speed;
        Debug.Log(newMoveVector);
    }

    public void Jump()
    {
        //TODO: check grounded in PlayerCollisions
        if (!grounded) return;
        
        rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}
