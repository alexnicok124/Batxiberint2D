using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class SoundEnemyAI : MonoBehaviour
{
    public Transform target;
    public float speed = 200f;
    public float maxDetectionRange;
    public float nextWaypointDistance = 0.44f;
    public float pathSearchCooldown = 0.5f;
    private float nextSearch = 0f;
    public bool showGizmos = false;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

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
        //Calculate Constantly new paths
        if (nextSearch < Time.time && Vector2.Distance(rb.position, target.position) < maxDetectionRange)
        {
            UpdatePath();
            nextSearch = Time.time + pathSearchCooldown;
        }
        if (showGizmos)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(rb.position, maxDetectionRange);
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
        
        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }
}
