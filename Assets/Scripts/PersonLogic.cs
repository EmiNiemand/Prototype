using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PersonLogic : MonoBehaviour
{
    private int minSatisfaction;
    private int currSatisfaction = 0;
    
    public Genre favGenre;
    public Instrument favInstrument;
    public Pattern favPattern;
    
    private GameObject Player;
    private Genre playerGenre;
    private Instrument playerInstrument;
    private Pattern playerPattern;

    private bool isAlreadyPlaying = false;
    
    public UnityEvent detectedPlayerPlaying;
    
    // Start is called before the first frame update
    void Start()
    {
        minSatisfaction = UnityEngine.Random.Range(0, 25);
        favGenre = (Genre)UnityEngine.Random.Range(0, 4);
        favInstrument = (Instrument)UnityEngine.Random.Range(0, 2);
        favPattern = (Pattern)UnityEngine.Random.Range(0, 2);

        Player = GameObject.FindGameObjectWithTag("Player");
        
        detectedPlayerPlaying.AddListener(SetPathToPlayer);
    }

    // Update is called once per frame
    void Update()
    {
        // if (Player. cos tam cos tam stan bool istnienia)
        // Pobrac ostatnie patterny, sprawdzic czy wypadl akurat ten ulubiony
        // Pobrac instrument na ktorym gra gracz
        // Pobrac gatunek muzyki

        isAlreadyPlaying = true;
        
        if (playerGenre == favGenre)
        {
            currSatisfaction += 30;
        }
        
        if (playerInstrument == favInstrument)
        {
            currSatisfaction += 20;
        }
        
        if (playerGenre == favGenre)
        {
            currSatisfaction += 25;
        }
        // currSatisfaction = 
        // Pobierz stan gracza i sprawdz czy aktualnie rozpoczal gre na instrumencie
        // Jezeli tak, sprawdz czy spelnia warunki
            
    }

    void SetPathToPlayer()
    {
        
    }
}
