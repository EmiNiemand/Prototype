using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PersonLogic : MonoBehaviour
{
    private float minSatisfaction;
    private float currSatisfaction = 0;
    private PersonMovement _personMovement;
    
    [HideInInspector] public Genre favGenre;
    [HideInInspector] public Instrument favInstrument;
    [HideInInspector] public Pattern favPattern;
    
    private GameObject Player;
    private Genre playerGenre;
    private Instrument playerInstrument;
    private Pattern playerPattern;
    private float playerSkill;

    private bool isAlreadyPlaying = false;
    
    private UnityEvent playerStartedPlaying;
    private UnityEvent playerStoppedPlaying;
    
    // Start is called before the first frame update
    void Start()
    {
        minSatisfaction = UnityEngine.Random.Range(40, 60);
        favGenre = (Genre)UnityEngine.Random.Range(0, 4);
        favInstrument = (Instrument)UnityEngine.Random.Range(0, 2);
        favPattern = (Pattern)UnityEngine.Random.Range(0, 2);

        Player = GameObject.FindGameObjectWithTag("Player");
        
        playerStartedPlaying.AddListener(SetPathToPlayer);
        playerStoppedPlaying.AddListener(ReturnToPreviousPath);
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: Pobierz stan gracza i sprawdz czy aktualnie rozpoczal gre na instrumencie
        // Jezeli tak, sprawdz czy spelnia warunki
        //
        // if (Player. cos tam cos tam stan bool istnienia)
        // TODO: Pobrac ostatnie patterny, sprawdzic czy wypadl akurat ten ulubiony
        // TODO: Pobrac instrument na ktorym gra gracz
        // TODO: Pobrac gatunek muzyki

        isAlreadyPlaying = true;
        
        if (playerGenre == favGenre)
        {
            currSatisfaction += 30;
        }
        
        if (playerInstrument == favInstrument)
        {
            currSatisfaction += 20;
        }
        
        if (playerPattern == favPattern)
        {
            currSatisfaction += 25;
        }

        currSatisfaction += playerSkill;

        if (currSatisfaction > minSatisfaction)
        {
            playerStartedPlaying.Invoke();
        }

        // else
        // {
        //     isAlreadyPlaying = false;
        //     playerStoppedPlaying.Invoke();
        // }
        

    }

    void SetPathToPlayer()
    {
        _personMovement.SetNewPathToPlayer(Player.transform.position);
    }
    
    void ReturnToPreviousPath()
    {
        _personMovement.ReturnToPreviousPath();
    }
}
