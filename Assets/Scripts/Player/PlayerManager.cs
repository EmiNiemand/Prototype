using System.Collections;
using System.Collections.Generic;
using Music;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject sessionStarterPrefab;
    [SerializeField] private GameObject sessionPrefab;
    private Session session;
    private SessionStarter sessionStarter;

    private CrowdManager crowdManager;
    
    private PlayerMovement playerMovement;
    private PlayerEquipment playerEquipment;
    private PlayerCollider playerCollider;
    private PlayerCamera playerCamera;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerEquipment = GetComponent<PlayerEquipment>();
        playerCollider = GetComponent<PlayerCollider>();
        playerCamera = GetComponent<PlayerCamera>();

        crowdManager = FindObjectOfType<CrowdManager>();
    }
    
    #region Equipment methods

    public bool BuyInstrument(int price, Instrument instrument)
    {
        return playerEquipment.BuyInstrument(price, instrument);
    }

    public bool AddPattern(Music.Helpers.Pattern newPattern)
    {
        crowdManager.PlayedPattern(newPattern);
        
        return playerEquipment.AddPattern(newPattern);
    }

    public HashSet<Instrument> GetInstruments()
    {
        return playerEquipment.GetInstruments();
    }
    #endregion

    #region Session Methods

    public void StartSession(Instrument instrument)
    {
        Debug.Log("Started session with "+instrument.name+"!");
        GetComponent<PlayerInput>().SwitchCurrentActionMap(ActionMaps.Session.ToString());
        
        
        session = Instantiate(sessionPrefab, transform).GetComponent<Session>();
        session.Setup(this, instrument);
        crowdManager.SessionStarted(instrument.name, instrument.genre);
    }

    public void EndSession()
    {
        Debug.Log("Finished session!");
        crowdManager.SessionEnded();
        Destroy(session.gameObject);
        session = null;
        GetComponent<PlayerInput>().SwitchCurrentActionMap(ActionMaps.Normal.ToString());
    }
    #endregion

    #region Input Methods [NORMAL]
    public void OnMove(InputAction.CallbackContext context)
    {
        playerMovement.Move(context.ReadValue<Vector2>());
    }
    
    public void OnToggleSession(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if(playerEquipment.GetInstruments().Count == 0) return;
        if(sessionStarter)
        {
            Destroy(sessionStarter.gameObject);
            sessionStarter = null;
            return;
        }
        if(session) { EndSession(); return; }

        sessionStarter = Instantiate(sessionStarterPrefab, transform)
            .GetComponent<SessionStarter>();
        sessionStarter.Setup(this);
    }
    
    public void OnUse(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        
        playerCollider.OnUse();
    }
    #endregion
    
    #region Input Methods [SESSION]
    
    //TODO: dis stupid, improve somehow?
    public void OnSound1(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (!session) return;
        
        session.PlaySample(0);
    }
        
    public void OnSound2(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (!session) return;
        
        session.PlaySample(1);
    }
        
    public void OnSound3(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (!session) return;
        
        session.PlaySample(2);
    }
        
    public void OnSound4(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (!session) return;
        
        session.PlaySample(3);
    }
    #endregion
}
