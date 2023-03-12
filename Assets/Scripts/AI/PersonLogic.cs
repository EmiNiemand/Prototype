using System.Collections;
using System.Collections.Generic;
using Music;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class PersonLogic : MonoBehaviour
{
    private float minSatisfaction;
    private float currSatisfaction = 0;
    private PersonMovement _personMovement;
    
    [HideInInspector] public Genre favGenre;
    [HideInInspector] public InstrumentName favInstrumentName;
    [HideInInspector] public Pattern favPattern;
    
    private GameObject Player;
    private Genre playerGenre;
    private Instrument playerInstrument;
    private Pattern playerPattern;
    private float playerSkill;
    private bool playerIsPlaying = false;
    private bool playerIsAlreadyPlaying = false;

    private UnityEvent playerStartedPlaying;
    private UnityEvent playerStoppedPlaying;
    
    // Start is called before the first frame update
    void Start()
    {
        _personMovement = GetComponent<PersonMovement>();
        
        minSatisfaction = UnityEngine.Random.Range(40, 60);
        favGenre = (Genre)UnityEngine.Random.Range(0, 5);
        favInstrumentName = (InstrumentName)UnityEngine.Random.Range(0, 2);
        favPattern = (Pattern)UnityEngine.Random.Range(0, 2);

        Player = GameObject.FindGameObjectWithTag("Player");

        playerStartedPlaying = new UnityEvent();
        playerStartedPlaying.AddListener(SetPathToPlayer);
        
        playerStoppedPlaying = new UnityEvent();
        playerStoppedPlaying.AddListener(ReturnToPreviousPath);
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    private void SetPathToPlayer()
    {
        Vector3 newPos = new Vector3(Player.transform.position.x - UnityEngine.Random.Range(0.5f, 1.5f), 0, 
            Player.transform.position.z - UnityEngine.Random.Range(0.5f, 1.5f));
        _personMovement.SetNewPathToPlayer(newPos);
    }
    
    private void ReturnToPreviousPath()
    {
        _personMovement.ReturnToPreviousPath();
    }

    public void SetPlayerInstrumentAndGenre(Instrument ins, Genre gen)
    {
        playerInstrument = ins;
        playerGenre = gen;
    }
    
    public void SetPlayerPattern(Pattern pat)
    {
        playerPattern = pat;
    }
    
    public void SetPlayerStatus()
    {
        playerIsPlaying = !playerIsPlaying;
        
        if (playerIsPlaying)
        {
            if (!playerIsAlreadyPlaying)
            {
                playerIsAlreadyPlaying = true;
                
                currSatisfaction = 0;
            
                if (playerGenre == favGenre)
                {
                    currSatisfaction += 30;
                }
        
                if (playerInstrument.name == favInstrumentName)
                {
                    currSatisfaction += 20;
                }
        
                if (playerPattern == favPattern)
                {
                    currSatisfaction += 25;
                }
        
                // currSatisfaction += playerSkill;
        
                if (currSatisfaction > minSatisfaction)
                {
                    playerStartedPlaying.Invoke();
                }
                
            }

        }
        else
        {
            playerIsAlreadyPlaying = false;
            playerStoppedPlaying.Invoke();
        }
        
    }
    
    public float GetCurrentSatisfaction()
    {
        return currSatisfaction;
    }

}
