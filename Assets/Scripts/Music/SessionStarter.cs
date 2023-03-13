using System;
using System.Collections;
using System.Collections.Generic;
using Music;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SessionStarter : MonoBehaviour
{
    public GameObject genreButton;
    public GameObject instrumentButton;

    private PlayerManager playerManager;
    private List<GameObject> createdButtons;
    private Dictionary<Genre, List<Instrument>> instrumentsByGenre;
    
    public void Setup(PlayerManager manager)
    {
        playerManager = manager;
        createdButtons = new List<GameObject>();
        instrumentsByGenre = new Dictionary<Genre, List<Instrument>>();
        
        var instruments = manager.GetInstruments();
        var offset = 0;
        var genres = new HashSet<Genre>();
        foreach (var instrument in instruments)
        {
            if(genres.Add(instrument.genre)) 
                AddGenreButton(instrument.genre);
            
            if(!instrumentsByGenre.ContainsKey(instrument.genre)) 
                instrumentsByGenre.Add(instrument.genre, new List<Instrument>());
            instrumentsByGenre[instrument.genre].Add(instrument);
        }
    }

    private void AddGenreButton(Genre genre)
    {
        var xPos = 300 * createdButtons.Count * 
                   (createdButtons.Count % 2 == 0 ? -1 : 1);
        
        createdButtons.Add(Instantiate(genreButton, transform));
        createdButtons[^1].transform.Translate(xPos, 0, 0);
        createdButtons[^1].GetComponent<Button>()
            .onClick.AddListener(()=> ChooseGenre(genre));
        createdButtons[^1].GetComponentInChildren<TextMeshProUGUI>()
            .text = genre.ToString();
    }

    private void ChooseGenre(Genre genre)
    {
        createdButtons.ForEach(Destroy);
        createdButtons.Clear();
        
        ShowInstrumentButtons(genre);
    }

    private void ShowInstrumentButtons(Genre genre)
    {
        instrumentsByGenre[genre].ForEach(AddInstrumentButton);
    }
    
    private void AddInstrumentButton(Instrument instrument)
    {
        var xPos = 300 * createdButtons.Count * 
                   (createdButtons.Count % 2 == 0 ? -1 : 1);
        
        createdButtons.Add(Instantiate(instrumentButton, transform));
        createdButtons[^1].transform.Translate(xPos, 0, 0);
        createdButtons[^1].GetComponent<Image>().sprite = instrument.icon;
        createdButtons[^1].GetComponent<Button>()
            .onClick.AddListener(()=> ChooseInstrument(instrument));
        createdButtons[^1].GetComponentInChildren<TextMeshProUGUI>()
            .text = instrument.name.ToString();
    }

    private void ChooseInstrument(Instrument instrument)
    {
        foreach (var button in createdButtons) { Destroy(button); }
        createdButtons.Clear();
        
        playerManager.StartSession(instrument);
    }
}
