using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PersonLogic : MonoBehaviour
{
    private float minSatisfaction;
    private float currSatisfaction = 0;
    private PersonMovement _personMovement;
    
    public Genre favGenre;
    public Instrument favInstrument;
    public Pattern favPattern;
    
    private GameObject Player;
    private Genre playerGenre;
    private Instrument playerInstrument;
    private Pattern playerPattern;
    private float playerSkill;

    private bool isAlreadyPlaying = false;
    
    public UnityEvent detectedPlayerPlaying;
    
    // Start is called before the first frame update
    void Start()
    {
        minSatisfaction = UnityEngine.Random.Range(40, 60);
        favGenre = (Genre)UnityEngine.Random.Range(0, 4);
        favInstrument = (Instrument)UnityEngine.Random.Range(0, 2);
        favPattern = (Pattern)UnityEngine.Random.Range(0, 2);

        Player = GameObject.FindGameObjectWithTag("Player");
        
        detectedPlayerPlaying.AddListener(SetPathToPlayer);
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
            detectedPlayerPlaying.Invoke();
        }

        // else
        // {
        //     isAlreadyPlaying = false;
        // }
        

    }

    void SetPathToPlayer()
    {
        _personMovement.SetNewPathWithPathFindingToPlayer(Player.transform.position);
    }
}
