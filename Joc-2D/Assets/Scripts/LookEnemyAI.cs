using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class LookEnemyAI : MonoBehaviour
{
    [Header("Pathfinding")]
    public Transform target;
    public float maxDetectionRange;
    public float maxDistanceFromPlayer;
    public float nextWaypointDistance = 0.44f;
    public float pathSearchCooldown = 0.5f;
    public LayerMask layerMask;


    [Header("Physics")]
    public float speed = 200f;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    float nextSearch = 0f;

    Seeker seeker;
    Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

    }

    // Update the path of the enemy

    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete); 
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Linecast(rb.position, target.position, layerMask);
        //Calculate Constantly new paths
        if (nextSearch < Time.time && hit.collider.name == "Player")
        {
            UpdatePath();
            nextSearch = Time.time + pathSearchCooldown;
            Debug.DrawLine(transform.position, target.position, Color.green);
            Debug.Log("All Clear");
        }
        else if (nextSearch < Time.time)
        {
            Debug.DrawLine(transform.position, target.position, Color.red);
            Debug.Log(("Found "+ hit.collider.name));
        }

        if (path == null)
        {
            return;
        }
        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        
        Vector2 force = direction * speed * Time.deltaTime;

        if (Vector2.Distance(rb.position, target.position) > maxDistanceFromPlayer)
        {
            rb.AddForce(force);
        }

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }
}
