using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingEnemy : MonoBehaviour
{
    [Header("Pathfinding")]
    Transform target;
    public int searchLenght;
    public int spread;
    public float nextWaypointDistance = 0.44f;
    public LayerMask layerMask;
    public float wanderingCooldown = 5f;
    public float chasingCooldown = 0.5f;
    float nextSearch = 0f;
    public float chasingExtraTime = 10f;
    float chasingScanTime = 0f;

    [Header("Physics")]
    public float speed = 200f;
    public float rotationSpeed = 200f;
    public float chargingSpeed = 400f;
    public LayerMask deathIgnoreLayers;

    [Header("Attack:")]
    public int attackDamage = 20;
    public float attackRange;
    public float attackCooldown;
    float nextAttack;
    public Transform hitPoint;
    public float hitRange = 5f;
    public LayerMask playerLayer;
    public float ChargingDuration;
    public float chargeCooldown = 1f;
    float nextCharge = 0f;

    // Stun Variables
    bool isStunned = false;
    public float stunDuration;
    float stunEndTime = 0f;

    // Variables internas
    Path path;
    int currentWaypoint = 0;
    bool firstChasingPath = false;
    bool chargePointCheck = false;
    Vector2 direction;
    Vector2 force;
    Vector2 attackDirection;


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
        

        if (isStunned && Time.time >= stunEndTime)
        {
            isStunned = false;
            animator.SetBool("Stunned", false);
        }

        RaycastHit2D hit = Physics2D.Linecast(rb.position, target.position, layerMask);

        // Seleccionador de quin estat està l'enemic
        // Comprobar si el jugador està a suficient distancia per attacking-lo
        if (hit.collider.name == "Player" && Vector2.Distance(rb.position, target.position) < attackRange && nextAttack < Time.time)
        {
            chasing = false;
            wandering = false;
            attacking = true;
        }
        if (!attacking)
        {
            if (hit.collider.name == "Player") //chasing l'enemic
            {
                chasing = true;
                wandering = false;
                if (chasingScanTime < Time.time)
                    chasingScanTime += chasingExtraTime;
            }
            else // wandering
            {

                if (chasingScanTime < Time.time) // Dona temps al enemic per perseguir una mica al jugador
                {
                    chasing = false;
                    wandering = true;
                }
            } 
        }

        if (chasing && hit.collider.gameObject.name == "Player")
        {
            if (!firstChasingPath)
            {
                ChasingPath();
                firstChasingPath = true;
                nextSearch = Time.time;
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
        

        //Comproba que hi hagui un camí a sergüir
        if (path == null)
        {
            return;
        }

        if (isStunned)
        {
            rb.velocity = Vector2.zero; //Deixa paralitzat al enemic
            animator.SetBool("Stunned", true);
        }

        // Comproba si currentWaypoint està dintre del index de path
        if (currentWaypoint >= 0 && currentWaypoint < path.vectorPath.Count)
        {
            if (attacking)
            {
                StartCoroutine(Charge());
            }
            else
            {
                if (wandering || chasing)
                {
                    direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
                    force = speed * Time.deltaTime * direction;
                }

                if (wandering || chasing)
                {
                    Vector3 lookAt = path.vectorPath[currentWaypoint] - transform.position;
                    float angle = Mathf.Atan2(lookAt.y, lookAt.x) * Mathf.Rad2Deg - 90;
                    Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                }

                if (Vector2.Distance(rb.position, target.position) >= attackRange || hit.collider.gameObject.name != "Player")
                {
                    animator.SetBool("Moving", true);
                    rb.AddForce(force);
                } 

                float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
                if (distance < nextWaypointDistance)
                    currentWaypoint++;

            }
        } else
            animator.SetBool("Moving", false);

        if (!health.viu)
            Die();
    }

    //Funcions
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(hitPoint.position, hitRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
    IEnumerator Charge()
    {
        firstChasingPath = false;
        if (!chargePointCheck && nextCharge < Time.time)
        {
            attackDirection = ((Vector2)target.position - rb.position).normalized;
            chargePointCheck = true;
            nextCharge = chargeCooldown + Time.time;
            animator.SetBool("Moving", false);
            animator.SetBool("ChargingCharge", true);
        }
        Vector3 lookAt = attackDirection;
        float angle = Mathf.Atan2(lookAt.y, lookAt.x) * Mathf.Rad2Deg - 90;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        direction = attackDirection;
        force = chargingSpeed * Time.deltaTime * direction;
        yield return new WaitForSeconds(3f);
        animator.SetBool("Charging", true);
        animator.SetBool("ChargingCharge", false);
        rb.AddForce(force);
            
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") && animator.GetBool("Charging"))
        {
            attacking = false;
            chargePointCheck = false;
            rb.velocity = Vector2.zero;
            animator.SetBool("Charging", false);
            ApplyStun(stunDuration);
            nextAttack = attackCooldown + Time.time;
        } else if (collision.gameObject.CompareTag("Player") && animator.GetBool("Charging"))
        {
            Attack();
            attacking = false;
            chargePointCheck = false;
            rb.velocity = Vector2.zero;
            animator.SetBool("Charging", false);
            ApplyStun(stunDuration);
            nextAttack = attackCooldown + Time.time;
        }
    }

    // Actualitza el camí del enemic
    void ChasingPath()
    {
        seeker.StartPath(rb.position, target.position, OnPathComplete);
    }
    void WanderingPath()
    {
        RandomPath path = RandomPath.Construct(transform.position, searchLenght);
        path.spread = spread;
        seeker.StartPath(path, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error) // Comporba que no hi ha ningun error en el camí
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void Attack()
    {
        // Detecció
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(hitPoint.position, hitRange, playerLayer);
        // Atac
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Health>().TakeDamage(attackDamage);
        }
    }
    public void ApplyStun(float duration)
    {
        isStunned = true;
        stunEndTime = Time.time + duration;

        // Agregar animacions + suroll
        
    }

    void Die()
    {
        // Animació de morir:
        animator.SetBool("Moving", false);
        animator.SetBool("Death", true);

        // Desactivar l'enemic
        GetComponent<Collider2D>().excludeLayers = deathIgnoreLayers;
        this.enabled = false;
    }
}
