using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Unity.Burst.CompilerServices;

public class BaseSoundEnemyAI : MonoBehaviour
{
    [Header("Pathfinding:")]
    public Transform target;
    public float detectionRange;
    public float maxDistanceFromPlayer = 0.1f;
    public int searchLenght;
    public int spread;
    public float nextWaypointDistance = 0.44f;
    public LayerMask layerMask;

    [Header("Temps d'espera:")]
    public float wanderingCooldown = 5f;
    public float chasingCooldown = 0.5f;
    float nextSearch = 0f;
    public float chasingExtraTime = 10f;
    float chasingScanTime = 0f;
    public float attackingCooldown = 1f;
    float nextAttack = 0f;

    [Header("Physics:")]
    public float speed = 200f;
    public float rotationSpeed = 200f;

    [Header("Attack:")]
    public Transform attackPoint;
    public float attackRange;

    // Variables internes
    Path path;
    int currentWaypoint = 0;
    bool firstChasingPath = false;

    Seeker seeker;
    Rigidbody2D rb;

    // Estatus:
    bool wandering = true;
    bool chasing = false;
    bool attacking = false;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
    }

    

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Linecast(rb.position, target.position, layerMask);
        // Seleccionador de quin estat està l'enemic
        if (Vector2.Distance(rb.position, target.position) <= detectionRange) //chasing l'enemic
        {
            chasing = true;
            wandering = false;
            attacking = false;
            if (chasingScanTime < Time.time)
                chasingScanTime += chasingExtraTime;
        }
        else // wandering
        {
            if (chasingScanTime < Time.time) // Dona temps al enemic per perseguir una mica al jugador
            {
                chasing = false;
                wandering = true;
                attacking = false; 
            }
        }
        if (Vector2.Distance(rb.position, target.position) < maxDistanceFromPlayer && hit.collider.gameObject.name == "Player") // Comprobar si el jugador està a suficient distancia per attacking-lo
        {
            chasing = false;
            wandering = false;
            attacking = true;
        }

        // Selecciona quina 
        if (attacking)
        {
            firstChasingPath = false;
            if (nextAttack < Time.time)
            {
                Attack();
                nextAttack += attackingCooldown;
            }
            Debug.Log("Atacant");
        }
        else
        {

            if (chasing && Vector2.Distance(rb.position, target.position) <= detectionRange)
            {
                if (!firstChasingPath)
                {
                    ChasingPath();
                    firstChasingPath = true;
                }
                if (nextSearch < Time.time)
                {
                    ChasingPath();
                    nextSearch = Time.time + chasingCooldown;
                }
            }
            else if (wandering)
            {
                firstChasingPath = false;
                if (nextSearch < Time.time)
                {
                    WanderingPath();
                    nextSearch = Time.time + wanderingCooldown;
                }
            }
        }

        if (path == null)
        {
            return;
        }

        // Cambia la rotació del enemic
        if (attacking)
        {
            Vector3 lookAt = target.position - transform.position;
            float angle = Mathf.Atan2(lookAt.y, lookAt.x) * Mathf.Rad2Deg - 90;
            Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        }

        // Comproba si currentWaypoint està dintre del index de path
        if (currentWaypoint>=0 && currentWaypoint < path.vectorPath.Count)
        {
            //Aplicar una fuerza
            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            Vector2 force = direction * speed * Time.deltaTime;

            if (wandering || chasing)
            {
                Vector3 lookAt = path.vectorPath[currentWaypoint] - transform.position;
                float angle = Mathf.Atan2(lookAt.y, lookAt.x) * Mathf.Rad2Deg - 90;
                Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            //Condicions per que el enemic es mogui
            if (Vector2.Distance(rb.position, target.position) >= maxDistanceFromPlayer | hit.collider.gameObject.name != "Player")
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
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, maxDistanceFromPlayer);
    }


    // Actualitza el objectiu de l'enemic
    //      Pergesuir
    void ChasingPath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete); 
        }
    }
    //      Errar
    void WanderingPath()
    {
        Debug.Log("Buscant per on errar");
        RandomPath path = RandomPath.Construct(transform.position, searchLenght);
        path.spread = spread;
        seeker.StartPath(path, OnPathComplete);
    }

    // 
    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void Attack()
    {
        
    }
}
