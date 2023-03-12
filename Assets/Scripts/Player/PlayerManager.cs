using System.Collections;
using System.Collections.Generic;
using Music;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    //TODO: DELET
    [SerializeField] private Instrument instrument;
    
    [SerializeField] private GameObject sessionStarter;
    private PlayerMovement playerMovement;
    private PlayerEquipment playerEquipment;
    private PlayerCollider playerCollider;
    
    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerEquipment = GetComponent<PlayerEquipment>();
        playerCollider = GetComponent<PlayerCollider>();
        
        //TODO: DELET, mock
        playerEquipment.BuyInstrument(0, instrument);
    }
    
    #region Equipment methods

    public bool BuyInstrument(int price, Instrument instrument)
    {
        return playerEquipment.BuyInstrument(price, instrument);
    }

    public bool AddPattern(Music.Helpers.Pattern newPattern)
    {
        if (playerEquipment.AddPattern(newPattern))
        {
            //TODO: when everything's ready, move CrowdManager.NewPattern() here
            return true;
        }

        return false;
    }

    public List<Instrument> GetInstruments()
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
        if (!context.started) return;
        
        // CrowdManager.OnSessionStart(InstrumentName.Drums, Genre.Jazz);

        Instantiate(sessionStarter, transform)
            .GetComponent<SessionStarter>()
            .Setup(this);
    }
    
    public void OnShop(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        
        playerCollider.OnUse();
    }

    //TODO: DELET, mock methods
    public void OnTest1(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        // CrowdManager.OnSessionStart(InstrumentName.Drums, Genre.Jazz);
        
    }
    public void OnTest2(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        // CrowdManager.OnNewPattern(pattern);
        
    }
    public void OnTest3(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        // CrowdManager.OnSessionEnd();
        
    }
    #endregion
}
