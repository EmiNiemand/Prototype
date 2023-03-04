using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class PathPoint
{
    public int number;
    public GameObject point;
}

public class Enemy : MonoBehaviour
{
    [SerializeField] private List<PathPoint> pathPoints;

    private UnityEvent<GameObject> startBeingAlarmedEvent;
    private UnityEvent stopBeingAlarmedEvent;
    public Dictionary<int, Collider> hitColliders;
    private Dictionary<int, Vector3> normalPath;

    private Rigidbody rb;
    
    private bool isAlarmed = false;

    private int pathPointNumber = 0;
    private CapsuleCollider bCollider;
    private GameObject target;

    [SerializeField][Range(0, 5)] private float timeBetweenPathCalculations = 2;
    [SerializeField] private float timeToStopSearching = 5;
    [SerializeField] private float moveSpeed;
    
    private float timer = 0;
    private int iterationCounter = 0;
    
    private List<Vector3> path;

    // Start is called before the first frame update
    void Start()
    {
        normalPath = new Dictionary<int, Vector3>();
        hitColliders = new Dictionary<int, Collider>();
        path = new List<Vector3>();

        if (startBeingAlarmedEvent == null) startBeingAlarmedEvent = new UnityEvent<GameObject>();
        startBeingAlarmedEvent.AddListener(StartBeingAlarmed);
        
        if (stopBeingAlarmedEvent == null) stopBeingAlarmedEvent = new UnityEvent();
        stopBeingAlarmedEvent.AddListener(StopBeingAlarmed);

        bCollider = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
        foreach (var pathPoint in pathPoints)
        {
            var pos = pathPoint.point.transform.position;
            normalPath.Add(pathPoint.number, new Vector3(pos.x, 0, pos.z));
        }
        
        path.Add(normalPath.GetValueOrDefault(pathPointNumber));
    }

    // Update is called once per frame
    void Update()
    {
        var pos = transform.position;
        if (Vector3.Distance(new Vector3(pos.x, 0, pos.z), path[0]) < 0.5f)
        {
            if (!isAlarmed)
            {
                pathPointNumber++;
                if (pathPointNumber >= normalPath.Count) pathPointNumber = 0;
                path.RemoveAt(0);
                path.Add(normalPath.GetValueOrDefault(pathPointNumber));
            }

            else if (isAlarmed)
            {
                path.RemoveAt(0);
                if (path == null)
                {
                    //TODO: czy to oznacza że doszedł to miejsca i nikogo nie jebnął?
                }
            }
        }
        
        rb.MovePosition(transform.position + (path[0] - new Vector3(transform.position.x, 0, transform.position.z)).normalized * moveSpeed);
        
        if (!isAlarmed) return;
        if (timer > timeToStopSearching) stopBeingAlarmedEvent.Invoke();
        if (timer > timeBetweenPathCalculations * iterationCounter) SetNewPathWithPathFinding();
        timer += Time.deltaTime;
    }

    private void StartBeingAlarmed(GameObject targetObject)
    {
        target = targetObject;
        isAlarmed = true;
        path.Clear();
        SetNewPathWithPathFinding();
    }
    
    private void StopBeingAlarmed()
    {
        hitColliders.Clear();
        target = null;
        timer = 0;
        iterationCounter = 0;
        isAlarmed = false;
        path.Clear();
        SetNewPath();
    }

    void SetNewPath()
    {
        float minDistance = Vector3.Distance(transform.position, normalPath.GetValueOrDefault(0));
        Vector3 newTarget = normalPath.GetValueOrDefault(0);
        foreach (var position in normalPath)
        {
            if (Vector3.Distance(transform.position, position.Value) <= minDistance)
            {
                newTarget = position.Value;
                pathPointNumber = position.Key;
            }
        }
        path.Add(newTarget);
    }
    
    void SetNewPathWithPathFinding()
    {
        for (int i = 0; i < 8; i++)
        {
            Vector3 direction = Quaternion.AngleAxis(45 * i, Vector3.up) * Vector3.right;
            Vector3 position = transform.position;
            Vector3 origin = new Vector3(position.x, position.y + bCollider.height / 2, position.z);
            RaycastHit[] hits = Physics.RaycastAll(origin, direction);
            Debug.DrawRay(origin, direction, Color.red, 0.5f);
            foreach (var hit in hits)
            {
                hitColliders.Add(hit.colliderInstanceID, hit.collider);
            }
            origin = new Vector3(position.x, position.y - bCollider.height / 2 + 0.1f, position.z);
            hits = Physics.RaycastAll(origin, direction);
            Debug.DrawRay(origin, direction, Color.red, 0.5f);
            foreach (var hit in hits)
            {
                hitColliders.Add(hit.colliderInstanceID, hit.collider);
            }
        }
        var pathV2 = GetComponent<AI.Pathfinding>().FindPath(new Vector2(transform.position.x, transform.position.z),
            new Vector2(target.transform.position.x, target.transform.position.z));
        foreach (var pV2 in pathV2)
        {
            path.Add(new Vector3(pV2.x, 0, pV2.y));
        }
        iterationCounter++;
    }
}
