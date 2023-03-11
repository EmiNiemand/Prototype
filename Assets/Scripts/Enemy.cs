using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Palmmedia.ReportGenerator.Core.Logging;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


public class Enemy : MonoBehaviour
{
    [SerializeField] private List<GameObject> pathPoints;
    
    public UnityEvent<GameObject> startBeingAlarmedEvent;
    public UnityEvent stopBeingAlarmedEvent;
    public Dictionary<int, Collider> hitColliders;
    private RaycastHit[] hits;
    
    private Rigidbody rb;
    
    public bool isAlarmed = false;

    private int pathPointNumber = 0;
    private CapsuleCollider bCollider;
    private GameObject target;
    
    [SerializeField] private float timeToStopSearching = 5;
    [SerializeField] private float moveSpeed;
    
    private float timer = 0;

    private List<Vector3> path;
    private List<Vector3> backPath;

    private Vector3 previousPosition;

    // Start is called before the first frame update
    void Start()
    {
        hitColliders = new Dictionary<int, Collider>();
        backPath = new List<Vector3>();
        path = new List<Vector3>();
        hits = new RaycastHit[100];

        previousPosition = transform.position;
        
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
            if (timer > timeToStopSearching || path.Count == 0) stopBeingAlarmedEvent.Invoke();
            previousPosition = transform.position;
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
                if (path == null)
                {
                    stopBeingAlarmedEvent.Invoke();
                }
            }
        }
    }
    
    private void StartBeingAlarmed(GameObject targetObject)
    {
        hitColliders.Clear();
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
        float minDistance;
        if (backPath.Count == 0) minDistance = Vector3.Distance(transform.position, 
            new Vector3(pathPoints[0].transform.position.x, 0, pathPoints[0].transform.position.z));
        else minDistance = Vector3.Distance(backPath.Last(), new Vector3(pathPoints[0].transform.position.x, 
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
        path.Clear();


        foreach (var obj in GameObject.FindGameObjectsWithTag("Obstacles"))
        {
            var col = obj.GetComponent<BoxCollider>();
            if (col && ((transform.position.y + bCollider.height / 2 > obj.transform.position.y - col.size.y / 2 &&
                         transform.position.y - bCollider.height / 2 < obj.transform.position.y - col.size.y / 2) ||
                        (transform.position.y - bCollider.height / 2 < obj.transform.position.y + col.size.y / 2 && 
                         transform.position.y + bCollider.height / 2 > obj.transform.position.y + col.size.y / 2) ||
                        (transform.position.y + bCollider.height / 2 < obj.transform.position.y + col.size.y / 2 &&
                        transform.position.y - bCollider.height / 2 > obj.transform.position.y - col.size.y / 2)))
            {
                if (!hitColliders.ContainsKey(col.GetInstanceID())) hitColliders.Add(col.GetInstanceID(), col);
            }
        }

        // Find path using A*
        path = GetComponent<AI.Pathfinding>().FindPath(transform.position, target.transform.position);

        if(path.Count == 0) stopBeingAlarmedEvent.Invoke();
        
        for (int i = 0; i < path.Count - 1; i++)
        {
            Debug.DrawLine(path[i], path[i+1], Color.red, 2.0f);
        }
    }
}
