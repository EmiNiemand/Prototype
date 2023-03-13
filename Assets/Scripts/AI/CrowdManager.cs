using UnityEngine;

public class CrowdManager : MonoBehaviour
{
    private PersonLogic[] crowd;
    private InstrumentName currPlayerInstrument;
    private Genre currPlayerGenre;

    public void SessionStarted(InstrumentName ins, Genre gen)
    {
        currPlayerInstrument = ins;
        currPlayerGenre = gen;
        
        crowd = FindObjectsOfType<PersonLogic>();

        foreach (var obj in crowd)
        {
            obj.SetPlayerInstrumentAndGenre(ins, gen);
            obj.SetPlayerStatus();
        }
    }
    
    public void PlayedPattern(Music.Helpers.Pattern pat)
    {
        crowd = FindObjectsOfType<PersonLogic>();

        foreach (var obj in crowd)
        {
            obj.SetPlayerPattern(pat);
        }
    }
    
    public void SessionEnded()
    {
        crowd = FindObjectsOfType<PersonLogic>();

        foreach (var obj in crowd)
        {
            obj.SetPlayerStatus();
        }
    }
    
    public float GetCrowdSatisfaction()
    {
        crowd = FindObjectsOfType<PersonLogic>();

        float satisfaction = 0.0f;

        foreach (var obj in crowd)
        {
            satisfaction += obj.GetCurrentSatisfaction();
        }

        return satisfaction;
    }

    public InstrumentName GetCurrentPlayerInstrument()
    {
        return currPlayerInstrument;
    }
    
    public Genre GetCurrentPlayerGenre()
    {
        return currPlayerGenre;
    }

}
