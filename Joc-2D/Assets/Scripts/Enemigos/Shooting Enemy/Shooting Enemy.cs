using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShootingEnemy : MonoBehaviour
{
    [Header("Pathfinding:")]
    private Transform target;
    public float detectionRange;
    public float stopDistance = 0.1f;
    public float retreatDistance = 0.1f;
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
    public float maxBlinkTime = 0f;
    float nextBlink = 0f;

    [Header("Physics:")]
    public float speed = 200f;
    public float retreatSpeed = 1f;
    public float rotationSpeed = 200f;

    [Header("Attack:")]
    public int attackDamage = 20;
    public float attackRange;
    public float attackingCooldown = 1f;
    float nextAttack = 0f;
    public GameObject projectile;

    // Variables internes
    Path path;
    int currentWaypoint = 0;
    bool firstChasingPath = false;

    Seeker seeker;
    Rigidbody2D rb;
    HealthEnemy health;
    Animator animator;

    // Estatus:
    bool wandering = true;
    bool chasing = false;
    bool attacking = false;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<HealthEnemy>();
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;

    }



    // Update is called once per frame
    void FixedUpdate()
    {
        if (!health.viu)
        {
            Die();
        }
        RaycastHit2D hit = Physics2D.Linecast(rb.position, target.position, layerMask);
        // Seleccionador de quin estat est� l'enemic
        if (Vector2.Distance(rb.position, target.position) <= detectionRange && target.GetComponent<MovementPlayerScript>().movement != Vector2.zero) //chasing l'enemic
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
        if (Vector2.Distance(rb.position, target.position) < attackRange && hit.collider.gameObject.name == "Player" && target.GetComponent<MovementPlayerScript>().movement != Vector2.zero) // Comprobar si el jugador est� a suficient distancia per attacking-lo
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
                StartCoroutine(Attack());
                nextAttack = attackingCooldown + Time.time;
            }
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

        // Cambia la rotaci� del enemic
        if (attacking)
        {
            Vector3 lookAt = target.position - transform.position;
            float angle = Mathf.Atan2(lookAt.y, lookAt.x) * Mathf.Rad2Deg - 90;
            Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        }

        // Comproba si currentWaypoint est� dintre del index de path
        if (currentWaypoint >= 0 && currentWaypoint < path.vectorPath.Count)
        {
            //Aplicar una fuerza
            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            Vector2 force = speed * Time.deltaTime * direction;

            if (wandering || chasing)
            {
                Vector3 lookAt = path.vectorPath[currentWaypoint] - transform.position;
                float angle = Mathf.Atan2(lookAt.y, lookAt.x) * Mathf.Rad2Deg - 90;
                Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            //Condicions per que el enemic es mogui
            if (Vector2.Distance(rb.position, target.position) >= stopDistance | hit.collider.gameObject.name != "Player" | target.GetComponent<MovementPlayerScript>().movement == Vector2.zero)
            {
                rb.AddForce(force);
            }
            if (Vector2.Distance(rb.position, target.position) <= retreatDistance && hit.collider.gameObject.name == "Player" && target.GetComponent<MovementPlayerScript>().movement != Vector2.zero)
            {
                transform.position = Vector2.MoveTowards(transform.position, target.position, -retreatSpeed * Time.deltaTime);
            }


            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
            if (distance < nextWaypointDistance)
            {
                currentWaypoint++;
            }
        }
        // Blinking Animation
        if (nextBlink < Time.time)
        {
            animator.SetTrigger("Blink");
            nextBlink = Time.time + Random.Range(3f, maxBlinkTime);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, retreatDistance);
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
    private void OnCollisionEnter(Collision collision)
    {
        chasing = false;
        wandering = false;
        attacking = true;
    }

    IEnumerator Attack()
    {
        animator.SetTrigger("Blink");
        nextBlink = Time.time + Random.Range(3f, maxBlinkTime);
        yield return new WaitForSeconds(0.1f);
        projectile.GetComponent<Projectile>().damage = attackDamage;
        Instantiate(projectile, transform.position, Quaternion.identity);
    }

    // Apartat de vida
    void Die()
    {

        // Animaci� de morir:
        animator.SetBool("Death", true);
        // Desactivar l'enemic
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }
}
