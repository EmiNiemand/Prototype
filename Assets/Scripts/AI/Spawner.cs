using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Spawner : MonoBehaviour
{
    //TODO: This probably should be a Dictionary<InstrumentName, Pattern> but for now it doesn't matter that much 
    [SerializeField] private List<Music.Helpers.Pattern> patterns;
    
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
    void Update()
    {
        timer += Time.deltaTime;
        if (numberOfSpawned == maxNumberOfSpawns) return;

        if (timer > timeBetweenSpawns)
        {
            Vector3 position = new Vector3(Random.Range(-spawnDistance, spawnDistance) + transform.position.x, transform.position.y,
                Random.Range(-spawnDistance, spawnDistance) + transform.position.z);
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
                GameObject closestBuilding = buildings[0];
                foreach (var building in buildings)
                {
                    if (Vector3.Distance(building.transform.position, position) < 
                        Vector3.Distance(closestBuilding.transform.position, position))
                    {
                        closestBuilding = building;
                    }
                }

                if (Vector3.Distance(closestBuilding.transform.position, position) < 25.0f)
                {
                    probability += 10;
                }
            }

            if (Random.Range(0, 100) < probability)
            {
                SpawnListener(position);
                numberOfSpawned++;
            }
            timer = 0;
        }
    }

    void SpawnListener(Vector3 position)
    {
        if (listenerPrefabs != null)
        {
            var random = Random.Range(0, 100);
            var x = 100 / listenerPrefabs.Count * 9 / 10;
            var y = 100 - listenerPrefabs.Count * x;
            var z = 100 / listenerPrefabs.Count;
            if (buildings != null)
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
                                var listener = Instantiate(listenerPrefabs[j], position, Quaternion.identity);
                                listener.transform.parent = transform;
                                // Custom code, while spawner itself seems to be rather generalized(?)
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
                                var listener = Instantiate(listenerPrefabs[j], position, Quaternion.identity);
                                listener.transform.parent = transform;
                                // Custom code, while spawner itself seems to be rather generalized(?)
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
                            var listener = Instantiate(listenerPrefabs[j], position, Quaternion.identity);
                            listener.transform.parent = transform;
                            // Custom code, while spawner itself seems to be rather generalized(?)
                            return;
                        }
                    }
                }
            }
        }
    }
}
