using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PersonLogic : MonoBehaviour
{
    private float minSatisfaction = 0;
    private float currSatisfaction = 0;
    private PersonMovement _personMovement;

    private Image satisfactionIndicator;
    [SerializeField] private Gradient indicatorGradient;
    [SerializeField] public List<InstrumentName> favInstrumentsNames;
    [SerializeField] public List<Genre> favGenres;
    
    [HideInInspector] public Music.Helpers.Pattern favPattern;
    
    private GameObject Player;
    private InstrumentName playerInstrumentName;
    private Genre playerGenre;
    private Music.Helpers.Pattern playerPattern;
    private float playerSkill;
    private bool playerIsPlaying = false;
    private bool onPathToPlayer = false;
    
    // Start is called before the first frame update
    void Start()
    {
        satisfactionIndicator = transform.Find("PersonUI").GetComponentInChildren<Image>();
        
        _personMovement = GetComponent<PersonMovement>();
        
        minSatisfaction = UnityEngine.Random.Range(30, 50);
        indicatorGradient.colorKeys[1].time = minSatisfaction;

        Player = GameObject.FindGameObjectWithTag("Player");

        if (Player.GetComponent<PlayerManager>().GetSessionStatus())
        {
            playerIsPlaying = true;

            // Niesamowicie nie podoba mi się odwołanie wstecz do 
            // menadżera ale jest 02:04 i lepiej pójść na skróty
            CrowdManager crowdMg = FindObjectOfType<CrowdManager>();

            playerInstrumentName = crowdMg.GetCurrentPlayerInstrument();
            playerGenre = crowdMg.GetCurrentPlayerGenre();
        }

        CalculateSatisfaction();
    }
    
    private void SetPathToPlayer()
    {
        Vector3 newPos = new Vector3(Player.transform.position.x - UnityEngine.Random.Range(0.5f, 2.0f), 0, 
            Player.transform.position.z - UnityEngine.Random.Range(0.5f, 2.0f));
        
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

        if (playerPattern == favPattern)
        {
            currSatisfaction += 15;
            
            if (currSatisfaction > 100)
            {
                currSatisfaction = 100;
            }
        }
        else
        {
            currSatisfaction -= 5;
            
            if (currSatisfaction < 0)
            {
                currSatisfaction = 0;
            }
        }

        if (currSatisfaction > minSatisfaction && !onPathToPlayer) 
        {
            onPathToPlayer = true;
            SetPathToPlayer();
        }
        else if (currSatisfaction < minSatisfaction && onPathToPlayer)
        {
            onPathToPlayer = false;
            ReturnToPreviousPath();
        }
        
        SetSatisfactionIndicator();
    }
    
    public void SetPlayerStatus()
    {
        playerIsPlaying = !playerIsPlaying;

        CalculateSatisfaction();
    }

    public float GetCurrentSatisfaction()
    {
        return currSatisfaction;
    }

    public void CalculateSatisfaction()
    {
        currSatisfaction = 0;
        
        if (playerIsPlaying)
        {
            if (favGenres.Contains(playerGenre))
            {
                currSatisfaction += 30;
            }

            if (favInstrumentsNames.Contains(playerInstrumentName))
            {
                currSatisfaction += 20;
            }

            if (currSatisfaction > minSatisfaction)
            {
                onPathToPlayer = true;
                SetPathToPlayer();
            }
        }
        else
        {
            onPathToPlayer = false;
            ReturnToPreviousPath();
        }

        SetSatisfactionIndicator();
    }

    private void SetSatisfactionIndicator()
    {
        satisfactionIndicator.color = indicatorGradient.Evaluate(currSatisfaction / 100);
    }

}
