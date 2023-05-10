using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Spawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> buildings;
    [SerializeField] private List<GameObject> listenerPrefabs;
    [SerializeField] private float spawnDistance;
    [SerializeField] private int minNumberOfSpawns;
    [SerializeField] private int maxNumberOfSpawns;
    [SerializeField] private float timeBetweenSpawns;

    private float timer = 0;
    private int numberOfSpawned = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < minNumberOfSpawns; i++)
        {
            Vector3 position = new Vector3(Random.Range(-spawnDistance, spawnDistance) + transform.position.x, transform.position.y,
                Random.Range(-spawnDistance, spawnDistance) + transform.position.z);
            SpawnListener(position);
            numberOfSpawned++;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (numberOfSpawned == maxNumberOfSpawns) return;
        
        timer += Time.fixedDeltaTime;
        if (timer > timeBetweenSpawns)
        {
            Vector3 position = new Vector3(Random.Range(-spawnDistance, spawnDistance) + transform.position.x, transform.position.y,
                Random.Range(-spawnDistance, spawnDistance) + transform.position.z);
            
            if(CheckPositionForListener(position))
            {
                SpawnListener(position);
                numberOfSpawned++;
            }
            timer = 0;
        }
    }

    bool CheckPositionForListener(Vector3 position)
    {
        var distance = Vector3.Distance(transform.position, position);
        float probability;
        if (distance != 0)
        {
            probability = distance / spawnDistance * 100 - 40;
            if (probability <= 0) probability = 0;
        }
        else
        {
            probability = 60;
        }

        if (buildings != null)
        {
            if (Vector3.Distance(GetClosestBuilding(position).transform.position, position) < 25.0f)
            {
                probability += 10;
            }
        }

        return Random.Range(0, 100) < probability;
    }

    void SpawnListener(Vector3 position)
    {
        if (listenerPrefabs == null) return;
        
        var random = Random.Range(0, 100);
        var x = 100 / listenerPrefabs.Count * 9 / 10;
        var y = 100 - listenerPrefabs.Count * x;
        var z = 100 / listenerPrefabs.Count;
        
        if (buildings == null) return;

        GameObject closestBuilding = GetClosestBuilding(position);
                
        if (Vector3.Distance(closestBuilding.transform.position, position) < 25.0f)
        {
            bool dupa = false;
            for (int j = 0; j < listenerPrefabs.Count; j++)
            {
                if (listenerPrefabs[j].GetComponent<PersonLogic>().favGenres.Contains(closestBuilding.GetComponent<Building>().genre))
                {
                    dupa = true;
                    var value = x * j + x + y;
                    if (random <= value)
                    {
                        Instantiate(listenerPrefabs[j], transform).transform.position = position;
                        return;
                    }
                }
                else
                {
                    float value;
                    if (!dupa) value = x * j + x;
                    else value = x * j + x + y;
                    if (random <= value)
                    {
                        Instantiate(listenerPrefabs[j], transform).transform.position = position;
                        return;
                    }
                }
            }
        }
        else
        {
            for (int j = 0; j < listenerPrefabs.Count; j++)
            {
                var value = z * j + z;
                if (random <= value)
                {
                    Instantiate(listenerPrefabs[j], transform).transform.position = position;
                    return;
                }
            }
        }
    }

    private GameObject GetClosestBuilding(Vector3 position)
    {
        GameObject closestBuilding = buildings[0];
        foreach (var building in buildings)
        {
            if (Vector3.Distance(building.transform.position, position) < 
                Vector3.Distance(closestBuilding.transform.position, position))
            {
                closestBuilding = building;
            }
        }

        return closestBuilding;
    }
}
