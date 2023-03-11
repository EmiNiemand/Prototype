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
                Vector3 position = new Vector3(Random.Range(-100, 100) + transform.position.x, transform.position.y,
                    Random.Range(-100, 100) + transform.position.z);
                if (listenerPrefabs != null)
                {
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
                            var random = Random.Range(0, 100);
                            var x = 100 / listenerPrefabs.Count * 9 / 10;
                            for (int j = 0; j < listenerPrefabs.Count; j++)
                            {
                                if (listenerPrefabs[j].GetComponent<PersonLogic>().favGenre)
                                {
                                    
                                }
                            }
                        }
                    }
                }
            }
            return;
        }
    }
}
