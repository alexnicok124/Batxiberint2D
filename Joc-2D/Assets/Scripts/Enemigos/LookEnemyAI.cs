using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class LookEnemyAI : MonoBehaviour
{
    [Header("Pathfinding")]
    public Transform target;
    public float maxDistanceFromPlayer;
    public int searchLenght;
    public int spread;
    public float nextWaypointDistance = 0.44f;
    public LayerMask layerMask;

    [Header("Temps d'espera")]
    public float errantCooldown = 10f;
    public float perseguintCooldown = 0.5f;

    [Header("Physics")]
    public float speed = 200f;


    // Variables internas
    Path path;
    int currentWaypoint = 0;
    float nextSearch = 0f;

    Seeker seeker;
    Rigidbody2D rb;

    // Estats:
    bool errant = true;
    bool perseguint = false;
    bool atacar = false;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

    }

    // Actualitza el camí del enemic
    void UpdatePath()
    {
        Debug.Log("Perseguint");
        seeker.StartPath(rb.position, target.position, OnPathComplete);
    }
    void ErrantPath()
    { 
        Debug.Log("Buscant per on errar");
        RandomPath path = RandomPath.Construct(transform.position, searchLenght);
        path.spread = spread;
        seeker.StartPath(path, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error) //comporba que no hi ha ningun error en el camí
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Linecast(rb.position, target.position, layerMask);
        

        // Seleccionador de quin estat està l'enemic
        if (hit.collider.name == "Player") //Perseguint l'enemic
        {
            perseguint = true;
            errant = false;
            atacar = false;
        }
        else // Errant
        {
            perseguint = false;
            errant = true;
            atacar = false;
        }
        // Comprobar si el jugador està a suficient distancia per atacar-lo
        if (hit.collider.name == "Player" && Vector2.Distance(rb.position, target.position) < maxDistanceFromPlayer)
        {
            perseguint = false;
            errant = false;
            atacar = true;
        }


        
        if (atacar)
        {
            //Codi per atacar
            Debug.Log("Atacant");
        }
        else
        {

            if (perseguint)
            {
                if (nextSearch < Time.time)
                {
                    UpdatePath();
                    nextSearch = Time.time + perseguintCooldown;
                } 
            }
            else if (errant)
            {
                if (nextSearch < Time.time)
                {
                    ErrantPath();
                    nextSearch = Time.time + errantCooldown;
                }
            }
        }
        //Comproba que hi hagui un camí a sergüir
        if (path == null)
        {
            return;
        }
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
            currentWaypoint++;
        
    }
}
