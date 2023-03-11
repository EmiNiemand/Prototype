using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Spawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> buildings;
    [SerializeField] private List<GameObject> listenerPrefabs;
    [SerializeField] private List<GameObject> enemyPrefabs;
    [SerializeField] private float spawnDistance;
    [SerializeField] private float minNumberOfSpawns;
    [SerializeField] private float maxNumberOfSpawns;
    [SerializeField] private float timeBetweenSpawns;

    private float timer = 0;
    private float numberOfSpawned = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (numberOfSpawned == maxNumberOfSpawns) return;
        if (numberOfSpawned < minNumberOfSpawns)
        {
            for (int i = 0; i < minNumberOfSpawns - numberOfSpawned; i++)
            {
                Vector3 position = new Vector3(Random.Range(-spawnDistance, spawnDistance) + transform.position.x, transform.position.y,
                    Random.Range(-spawnDistance, spawnDistance) + transform.position.z);
                SpawnListener(position);
            }
            return;
        }

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
            }
            timer = 0;
            return;
        }
    }


    void SpawnListener(Vector3 position)
    {
        if (listenerPrefabs != null)
        {
            
            var random = Random.Range(0, 100);
            var x = 100 / listenerPrefabs.Count * 9 / 10;
            var y = 100 - listenerPrefabs.Count * x;
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
                    for (int j = 0; j < listenerPrefabs.Count; j++)
                    {
                        if (listenerPrefabs[j].GetComponent<PersonLogic>().favGenre == 
                            closestBuilding.GetComponent<Building>().genre)
                        {
                            var value = x * j + x + y;
                            if (value < random)
                            {
                                Instantiate(listenerPrefabs[j], position, Quaternion.identity);
                                return;
                            }
                        }
                        else
                        {
                            var value = x * j + x;
                            if (value < random)
                            {
                                Instantiate(listenerPrefabs[j], position, Quaternion.identity);
                                return;
                            }
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < listenerPrefabs.Count; j++)
                    {
                        var value = x * j + x;
                        if (value < random)
                        {
                            Instantiate(listenerPrefabs[j], position, Quaternion.identity);
                            return;
                        }
                    }
                }
            }
        }
    }
}
