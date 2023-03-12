using System.Collections;
using System.Collections.Generic;
using Music;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SessionStarter : MonoBehaviour
{
    public GameObject genreButton;
    private Genre chosenGenre;
    private List<GameObject> createdButtons;

    // Start is called before the first frame update
    public void Setup(PlayerManager manager)
    {
        createdButtons = new List<GameObject>();
        
        var instruments = manager.GetInstruments();
        var offset = 0;
        var genres = new HashSet<Genre>();
        foreach (var instrument in instruments)
        {
            if(genres.Add(instrument.genre)) 
                AddGenreButton(instrument.genre); 
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

    public void ChooseGenre(Genre genre)
    {
        foreach (var button in createdButtons) { Destroy(button); }
        chosenGenre = genre;
    }
}
