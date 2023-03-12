using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Music;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    public int cash;
    public int rep = 100;

    private HashSet<Music.Helpers.Pattern> patterns;
    private List<Instrument> instruments;

    // Start is called before the first frame update
    void Start()
    {
        patterns = new HashSet<Music.Helpers.Pattern>();
        instruments = new List<Instrument>();
    }

    public bool BuyInstrument(int price, Instrument instrument)
    {
        if (price > cash) return false;
        
        if (instruments.Contains(instrument)) return false;
        instruments.Add(instrument);

        cash -= price;
        return true;
    }

    public bool AddPattern(Music.Helpers.Pattern newPattern)
    {
        return patterns.Add(newPattern);
    }

    public void UpdateCashAndRep(int playerCash, int playerRep)
    {
        cash += playerCash; rep += playerRep;
    }
    
    public List<Instrument> GetInstruments()
    {
        return instruments;
    }
}
