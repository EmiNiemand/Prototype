using System.Collections;
using System.Collections.Generic;
using Music;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject sessionStarterPrefab;
    [SerializeField] private GameObject sessionPrefab;
    private Session session;
    private SessionStarter sessionStarter;
    private bool sessionStatus = false;
    
    private CrowdManager crowdManager;
    
    private PlayerMovement playerMovement;
    private PlayerEquipment playerEquipment;
    private PlayerCollider playerCollider;
    private PlayerCamera playerCamera;
    private PlayerUI playerUI;
    
    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerEquipment = GetComponent<PlayerEquipment>();
        playerCollider = GetComponent<PlayerCollider>();
        playerCamera = GetComponent<PlayerCamera>();
        
        playerUI = GetComponentInChildren<PlayerUI>();
        playerUI.Setup();
        playerUI.UpdateCashAndRep(playerEquipment.cash, playerEquipment.rep);

        crowdManager = FindObjectOfType<CrowdManager>();
    }
    
    #region Equipment methods

    public bool BuyInstrument(int price, Instrument instrument)
    {
        if (playerEquipment.BuyInstrument(price, instrument))
        {
            playerUI.UpdateCashAndRep(playerEquipment.cash, playerEquipment.rep);
            return true;
        }

        return false;
    }

    public HashSet<Instrument> GetInstruments()
    {
        return playerEquipment.GetInstruments();
    }
    #endregion

    #region Session Methods

    public void StartSession(Instrument instrument)
    {
        sessionStatus = true;
        GetComponent<PlayerInput>().SwitchCurrentActionMap(ActionMaps.Session.ToString());

        Destroy(sessionStarter.gameObject);
        sessionStarter = null;
        
        session = Instantiate(sessionPrefab, transform).GetComponent<Session>();
        session.Setup(this, instrument);
        crowdManager.SessionStarted(instrument.name, instrument.genre);
    }
    
    public bool GetSessionStatus() { return sessionStatus; }

    // Argument pat is null when player failed playing pattern
    public void PlayedPattern(Music.Helpers.Pattern pat)
    {
        crowdManager.PlayedPattern(pat);
        
        if (!pat) return;
        
        playerEquipment.AddPattern(pat);
        playerEquipment.AddReward(crowdManager.GetCrowdSatisfaction()/100);
        
        playerUI.UpdateCashAndRep(playerEquipment.cash, playerEquipment.rep);
    }

    public void EndSession()
    {
        sessionStatus = false;
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
        if(session) { EndSession(); return; }
        if(sessionStarter)
        {
            Destroy(sessionStarter.gameObject);
            sessionStarter = null;
            return;
        }

        sessionStarter = Instantiate(sessionStarterPrefab, transform)
            .GetComponent<SessionStarter>();
        sessionStarter.Setup(this);
    }
    
    public void OnUse(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        
        playerCollider.OnUse();
    }

    public void OnResetLevel(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        SceneManager.LoadScene(0);
    }
    #endregion
    
    #region Input Methods [SESSION]

    public void OnCheatSheet(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (!session) return;
        session.ToggleCheatSheet();
    }
    
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
