using System;
using System.Collections;
using System.Collections.Generic;
using Music;
using UnityEngine;

public class CrowdManager : MonoBehaviour
{
    private PersonLogic[] crowd;

    public void SessionStarted(InstrumentName ins, Genre gen)
    {
        crowd = FindObjectsOfType<PersonLogic>();

        foreach (var obj in crowd)
        {
            obj.SetPlayerInstrumentAndGenre(ins, gen);
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
    
    
}
