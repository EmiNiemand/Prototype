using System.Collections;
using System.Collections.Generic;
using Music;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject sessionStarter;
    private PlayerMovement playerMovement;
    private PlayerEquipment playerEquipment;
    private PlayerCollider playerCollider;
    
    private PlayerShop playerShop;
    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerEquipment = GetComponent<PlayerEquipment>();
        playerCollider = GetComponent<PlayerCollider>();
        
        playerShop = GetComponent<PlayerShop>();
    }
    
    #region Equipment methods

    public bool BuyInstrument(int price, Instrument instrument)
    {
        return playerEquipment.BuyInstrument(price, instrument);
    }

    public bool AddPattern(Music.Helpers.Pattern newPattern)
    {
        return playerEquipment.AddPattern(newPattern);
    }

    public HashSet<Instrument> GetInstruments()
    {
        return playerEquipment.GetInstruments();
    }
    #endregion

    #region Input methods
    public void OnMove(InputAction.CallbackContext context)
    {
        playerMovement.Move(context.ReadValue<Vector2>());
    }

    // Assuming that player can start session anywhere
    public void OnJump(InputAction.CallbackContext context)
    {
        return;
    }
    
    public void OnShop(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        playerCollider.OnUse();
    }
    #endregion
}
