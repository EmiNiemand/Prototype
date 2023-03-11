using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PersonMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    
    private CapsuleCollider bCollider;
    private Rigidbody rb;
    private List<Vector3> path;
    private Vector3 endTarget;
    private Vector3 previousTarget;
    private float timer = 0;
    private float speedMultiplier = 1.0f;
    
    private Dictionary<int, Collider> hitColliders;
    private bool isAlarmed = false;

    // Start is called before the first frame update
    void Start()
    {
        hitColliders = new Dictionary<int, Collider>();
        path = new List<Vector3>();

        bCollider = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();

        path.Add(GetRandomPoint());
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;

        if (path.Count == 0 && !isAlarmed)
        {
            path.Add(GetRandomPoint());
            SetNewPathWithPathFinding();
        }
        
        if (isAlarmed)
        {
            speedMultiplier = 1.5f;
        }
        else
        {
            speedMultiplier = 1.0f;
        }

        rb.MovePosition(transform.position + (path[0] - new Vector3(pos.x, 0, pos.z)).normalized 
            * (moveSpeed * speedMultiplier));
        
        if (Vector3.Distance(new Vector3(pos.x, 0, pos.z), new Vector3(path[0].x, 0, path[0].z)) < 0.1f)
        {
            if (!isAlarmed)
            {
                StartCoroutine(WaitAndGetNewRandomPoint());
            }
        }
    }

    private Vector3 GetRandomPoint()
    {
        endTarget = new Vector3(UnityEngine.Random.Range(-500, 500), UnityEngine.Random.Range(-500, 500), 0);
        
        // TODO: Dopisac sprawdzanie dla spawnu bo juz mi sie nie chcialo
        
        // foreach (var obj in GameObject.FindGameObjectsWithTag("Obstacles"))
        // {
        //     var col = obj.GetComponent<BoxCollider>();
        //     if (col && ((transform.position.y + bCollider.height / 2 > obj.transform.position.y - col.size.y / 2 &&
        //                  transform.position.y - bCollider.height / 2 < obj.transform.position.y - col.size.y / 2) ||
        //                 (transform.position.y - bCollider.height / 2 < obj.transform.position.y + col.size.y / 2 && 
        //                  transform.position.y + bCollider.height / 2 > obj.transform.position.y + col.size.y / 2) ||
        //                 (transform.position.y + bCollider.height / 2 < obj.transform.position.y + col.size.y / 2 &&
        //                  transform.position.y - bCollider.height / 2 > obj.transform.position.y - col.size.y / 2)))
        //     {
        //         endTarget = new Vector3(UnityEngine.Random.Range(-500, 500), UnityEngine.Random.Range(-500, 500), 0);
        //     }
        // }
        
        return endTarget;
    }
    
    private IEnumerator WaitAndGetNewRandomPoint()
    {
        yield return new WaitForSeconds(1.5f);
        endTarget = GetRandomPoint();
        SetNewPathWithPathFinding();
    }
    
    private IEnumerator WaitAndGoToPlayer()
    {
        yield return new WaitForSeconds(1.0f);
        SetNewPathWithPathFinding();
    }

    public void SetNewPathToPlayer(Vector3 playerPosition)
    {
        previousTarget = endTarget;
        endTarget = playerPosition;
        isAlarmed = true;
        StartCoroutine(WaitAndGoToPlayer());
    }
        
    public void ReturnToPreviousPath()
    {
        endTarget = previousTarget;
        isAlarmed = false;
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
        path = GetComponent<AI.Pathfinding>().FindPath(transform.position, endTarget);

        for (int i = 0; i < path.Count - 1; i++)
        {
            Debug.DrawLine(path[i], path[i+1], Color.red, 2.0f);
        }
    }
    
}
