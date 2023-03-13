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
    [HideInInspector] public Music.Helpers.Pattern favPattern;
    
    private GameObject Player;
    private Genre playerGenre;
    private InstrumentName playerInstrumentName;
    private Music.Helpers.Pattern playerPattern;
    private float playerSkill;
    private bool playerIsPlaying = false;
    private bool playerIsAlreadyPlaying = false;
    
    // Start is called before the first frame update
    void Start()
    {
        _personMovement = GetComponent<PersonMovement>();
        
        minSatisfaction = UnityEngine.Random.Range(40, 60);
        favGenre = (Genre)UnityEngine.Random.Range(0, 5);
        favInstrumentName = (InstrumentName)UnityEngine.Random.Range(0, 2);

        Player = GameObject.FindGameObjectWithTag("Player");
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

    public void SetPlayerInstrumentAndGenre(InstrumentName ins, Genre gen)
    {
        playerInstrumentName = ins;
        playerGenre = gen;
    }
    
    public void SetPlayerPattern(Music.Helpers.Pattern pat)
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
                    currSatisfaction += 30;

                if (playerInstrumentName == favInstrumentName) 
                    currSatisfaction += 20;

                if (playerPattern == favPattern) 
                    currSatisfaction += 25;

                //TODO: implement somehow
                // currSatisfaction += playerSkill;
        
                if (currSatisfaction > minSatisfaction)
                    SetPathToPlayer();
            }
        }
        else
        {
            playerIsAlreadyPlaying = false;
            ReturnToPreviousPath();
        }
        
    }
    
    public float GetCurrentSatisfaction()
    {
        return currSatisfaction;
    }

}
