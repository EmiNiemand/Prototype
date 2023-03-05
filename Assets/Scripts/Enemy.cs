using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


public class Enemy : MonoBehaviour
{
    [SerializeField] private List<GameObject> pathPoints;

    public UnityEvent<GameObject> startBeingAlarmedEvent;
    public UnityEvent stopBeingAlarmedEvent;
    public Dictionary<int, Collider> hitColliders;

    private Rigidbody rb;
    
    public bool isAlarmed = false;

    private int pathPointNumber = 0;
    private CapsuleCollider bCollider;
    private GameObject target;
    
    [SerializeField] private float timeToStopSearching = 5;
    [SerializeField] private float moveSpeed;
    
    private float timer = 0;
    private float setPathTimer = 0;

    private List<Vector3> path;
    private List<Vector3> backPath;

    // Start is called before the first frame update
    void Start()
    {
        hitColliders = new Dictionary<int, Collider>();
        backPath = new List<Vector3>();
        path = new List<Vector3>();

        if (startBeingAlarmedEvent == null) startBeingAlarmedEvent = new UnityEvent<GameObject>();
        startBeingAlarmedEvent.AddListener(StartBeingAlarmed);
        
        if (stopBeingAlarmedEvent == null) stopBeingAlarmedEvent = new UnityEvent();
        stopBeingAlarmedEvent.AddListener(StopBeingAlarmed);

        bCollider = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();

        path.Add(new Vector3(pathPoints[pathPointNumber].transform.position.x, 0, pathPoints[pathPointNumber].transform.position.z));
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlarmed)
        {
            if (timer > timeToStopSearching) stopBeingAlarmedEvent.Invoke();
            timer += Time.deltaTime;
        }
        
        var pos = transform.position;

        if (path.Count == 0) return;
        
        rb.MovePosition(transform.position + (path[0] - new Vector3(transform.position.x, 0, transform.position.z)).normalized * moveSpeed);
        if (Vector3.Distance(new Vector3(pos.x, 0, pos.z), new Vector3(path[0].x, 0, path[0].z)) < 0.1f)
        {
            if (isAlarmed) backPath.Add(new Vector3(path[0].x ,path[0].y, path[0].z));
            
            path.RemoveAt(0);
            
            if (!isAlarmed && path.Count == 0)
            {
                pathPointNumber++;
                if (pathPointNumber >= pathPoints.Count) pathPointNumber = 0;
                path.Add(new Vector3(pathPoints[pathPointNumber].transform.position.x, 0, pathPoints[pathPointNumber].transform.position.z));
            }

            else if (isAlarmed)
            {
                timer = 0;
                if (path == null)
                {
                    stopBeingAlarmedEvent.Invoke();
                    return;
                }
            }
        }
    }

    private void StartBeingAlarmed(GameObject targetObject)
    {
        target = targetObject;
        isAlarmed = true;
        path.Clear();
        backPath.Clear();
        SetNewPathWithPathFinding();
    }
    
    private void StopBeingAlarmed()
    {
        hitColliders.Clear();
        target = null;
        timer = 0;
        isAlarmed = false;
        if (backPath.Count != 0)
        {
            path.Clear();
            backPath.Reverse();
            path = backPath;
        }
        else
        {
            SetNewPath();
        }
    }

    void SetNewPath()
    {
        float minDistance = Vector3.Distance(transform.position, new Vector3(pathPoints[0].transform.position.x, 
            0, pathPoints[0].transform.position.z));
        int i = 0;
        foreach (var point in pathPoints)
        {
            var p = new Vector3(point.transform.position.x, 0, point.transform.position.z);
            if (Vector3.Distance(transform.position, p) <= minDistance)
            {
                target = point;
                pathPointNumber = i;
            }

            i++;
        }
        SetNewPathWithPathFinding();
    }
    
    void SetNewPathWithPathFinding()
    {
        for (int i = 0; i < 60; i++)
        {
            Vector3 direction = Quaternion.AngleAxis(6 * i, Vector3.up) * Vector3.right;
            Vector3 position = transform.position;
            Vector3 origin = new Vector3(position.x, position.y + bCollider.height / 2, position.z);
            RaycastHit[] hits = Physics.RaycastAll(origin, direction);
            Debug.DrawRay(origin, direction, Color.red, 0.5f);
            foreach (var hit in hits)
            {
                if (!hitColliders.ContainsKey(hit.collider.GetInstanceID())) hitColliders.Add(hit.collider.GetInstanceID(), hit.collider);
            }
            origin = new Vector3(position.x, position.y - bCollider.height / 2 + 0.1f, position.z);
            hits = Physics.RaycastAll(origin, direction);
            Debug.DrawRay(origin, direction, Color.red, 0.5f);
            foreach (var hit in hits)
            {
                if (!hitColliders.ContainsKey(hit.collider.GetInstanceID())) hitColliders.Add(hit.collider.GetInstanceID(), hit.collider);
            }
        }
        
        // Find path using A*
        path = GetComponent<AI.Pathfinding>().FindPath(transform.position, target.transform.position);

        if(path.Count == 0) stopBeingAlarmedEvent.Invoke();
        
        else
        {
            /*List<Vector3> destinations = new List<Vector3>();
        
            // Bezier to path smoothing
            for (int i = 0; i < path.Count - 1; i ++)
            {
                var t = Vector2.Distance(new Vector2(path[i].x, path[i].z), new Vector2(path[i + 1].x, 
                    path[i + 1].z)) / 100;
                for (float j = 0; j <= 1; j += t)
                {
                    Vector2 vec = Mathf.Pow((1 - j), 2) * new Vector2(path[i].x, path[i].z) +
                                  2 * (1 - j) * j * new Vector2(path[i + 1].x, path[i].z) +
                                  Mathf.Pow(j, 2) * new Vector2(path[i + 1].x, path[i + 1].z);
                    
                    destinations.Add(new Vector3(vec.x, 0, vec.y));
                }
            }

            path = destinations;*/
            
            for (int i = 0; i < path.Count - 1; i++)
            {
                Debug.DrawLine(path[i], path[i+1], Color.red, 2.0f);
            }
        }
    }
}
