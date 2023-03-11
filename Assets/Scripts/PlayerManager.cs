using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerShop playerShop;
    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerShop = GetComponent<PlayerShop>();
    }

    void Update()
    {
        PollMouse();
    }
    
    
    #region Input methods
    public void OnMove(InputAction.CallbackContext context)
    {
        playerMovement.Move(context.ReadValue<Vector2>());
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        return;
    }
    private void PollMouse()
    {
        return;
    }

    public void OnShop(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        playerShop.ShowShop();
    }
    #endregion
}
