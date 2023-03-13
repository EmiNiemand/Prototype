using System.Collections.Generic;
using AI;
using UnityEngine;

public class PersonMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float floorMinX;
    [SerializeField] private float floorMaxX;
    [SerializeField] private float floorMinZ;
    [SerializeField] private float floorMaxZ;

    private CapsuleCollider bCollider;
    private Rigidbody rb;
    private List<Vector3> path;
    private Pathfinding pathFinder;
    private Vector3 endTarget;
    private Vector3 previousTarget;
    private float speedMultiplier = 1.0f;
    private bool isAlarmed = false;
    private Vector3 currPostition;

    public Dictionary<int, Collider> hitColliders;
    
    // Start is called before the first frame update
    void Start()
    {
        hitColliders = new Dictionary<int, Collider>();
        path = new List<Vector3>();
        pathFinder = gameObject.AddComponent<Pathfinding>();

        bCollider = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();

        SetNewRandomPoint();
        SetNewPathWithPathFinding();
    }

    // Update is called once per frame
    void Update()
    {
        currPostition = transform.position;

        if (path.Count == 0 && !isAlarmed)
        {
            SetNewRandomPoint();
            SetNewPathWithPathFinding();
            return;
        } 
        
        // Jak chcemy by się trzymali od gracz na dany dystans, aczkolwiek wtedy
        // tłum przypomina małpi gaj (kolizje colliderów)
        // if (path.Count == 0 && isAlarmed)
        // {
        //     SetNewPathWithPathFinding();
        // }
        
        if (path.Count != 0)
        {
            rb.MovePosition(transform.position + (path[0] - new Vector3(currPostition.x, 0, currPostition.z)).normalized 
                * (moveSpeed * speedMultiplier));
            
            if (Vector3.Distance(new Vector3(currPostition.x, 0, currPostition.z), 
                    new Vector3(path[0].x, 0, path[0].z)) < 0.1f)
            {
                path.RemoveAt(0);
            }
        }
        
    }

    private void SetNewRandomPoint()
    {
        do
        {
            endTarget = new Vector3(UnityEngine.Random.Range(floorMinX, floorMaxX), 0,
                UnityEngine.Random.Range(floorMinZ, floorMaxZ));
        } while (Physics.Raycast(endTarget, transform.TransformDirection(Vector3.forward), 1.5f) ||
                 Physics.Raycast(endTarget, transform.TransformDirection(Vector3.back), 1.5f) ||
                 Physics.Raycast(endTarget, transform.TransformDirection(Vector3.left), 1.5f) ||
                 Physics.Raycast(endTarget, transform.TransformDirection(Vector3.right), 1.5f));

    }

    public void SetNewPathToPlayer(Vector3 playerPosition)
    {
        previousTarget = endTarget;
        endTarget = playerPosition;
        isAlarmed = true;
        speedMultiplier = 1.5f;
        SetNewPathWithPathFinding();
    }

    public void ReturnToPreviousPath()
    {
        endTarget = previousTarget;
        isAlarmed = false;
        speedMultiplier = 1.0f;
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
        path = pathFinder.FindPath(transform.position, endTarget);

        for (int i = 0; i < path.Count - 1; i++)
        {
            Debug.DrawLine(path[i], path[i+1], Color.red, 2.0f);
        }
    }
    
}
