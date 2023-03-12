using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    public int cash;
    public int rep = 100;

    private HashSet<Music.Helpers.Pattern> patterns;
    private HashSet<InstrumentName> instruments;

    // Start is called before the first frame update
    void Start()
    {
        patterns = new HashSet<Music.Helpers.Pattern>();
        instruments = new HashSet<InstrumentName>();
    }

    public bool BuyInstrument(int price, InstrumentName instrument)
    {
        if (price > cash) return false;
        if (!instruments.Add(instrument)) return false;
        
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
}
