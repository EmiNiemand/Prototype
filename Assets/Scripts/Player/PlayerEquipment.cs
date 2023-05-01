using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Music;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    public int cash;
    public int rep = 100;

    [SerializeField] private int maxCashReward = 5;
    [SerializeField] private int maxRepReward = 10;

    private HashSet<Music.Helpers.Pattern> patterns;
    private HashSet<Instrument> instruments;

    // Start is called before the first frame update
    public void Setup(Instrument firstInstrument)
    {
        patterns = new HashSet<Music.Helpers.Pattern>();
        instruments = new HashSet<Instrument>();
        BuyInstrument(0, firstInstrument);
    }

    public bool BuyInstrument(int price, Instrument instrument)
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

    public void AddReward(float crowdSatisfaction)
    {
        cash += (int)(crowdSatisfaction * maxCashReward);
        rep += (int)(crowdSatisfaction * maxRepReward);
    }
    
    public HashSet<Instrument> GetInstruments()
    {
        return instruments;
    }
}
