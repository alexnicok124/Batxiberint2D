using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingEnemy : MonoBehaviour
{
    [Header("Pathfinding")]
    public Transform target;
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

    [Header("Attack:")]
    public int attackDamage = 20;
    public float attackingCooldown = 1f;
    float nextAttack = 0f;
    public float attackRange;
    public Transform hitPoint;
    public float hitRange = 5f;
    public LayerMask playerLayer;
    // Stun Variables
    bool isStunned = false;
    public float stunDuration;
    float stunEndTime = 0f;

    [Header("Health")]
    public int maxHealth = 100;
    int currentHealth;

    // Variables internas
    Path path;
    int currentWaypoint = 0;
    bool firstChasingPath = false;
    bool attackPointCheck = false;
    Vector2 direction;
    Vector2 force;
    Vector2 attackDirection;


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
        if (isStunned && Time.time >= stunEndTime)
            isStunned = false;

        RaycastHit2D hit = Physics2D.Linecast(rb.position, target.position, layerMask);

        // Seleccionador de quin estat està l'enemic
        // Comprobar si el jugador està a suficient distancia per attacking-lo
        if (hit.collider.name == "Player" && Vector2.Distance(rb.position, target.position) < attackRange)
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
        }

        // Comproba si currentWaypoint està dintre del index de path
        else if (currentWaypoint >= 0 && currentWaypoint < path.vectorPath.Count)
        {
            if (attacking)
            {
                firstChasingPath = false;
                if (!attackPointCheck && nextAttack < Time.time)
                {
                    attackDirection = ((Vector2)target.position - rb.position).normalized;
                    attackPointCheck = true;
                    Debug.Log("Atacant, " + nextAttack + ", " + Time.time);
                    nextAttack = attackingCooldown + Time.time;
                }
                Vector3 lookAt = attackDirection;
                float angle = Mathf.Atan2(lookAt.y, lookAt.x) * Mathf.Rad2Deg - 90;
                Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                direction = attackDirection;
                force = chargingSpeed * Time.deltaTime * direction;
                Debug.Log(direction + ", " + force);
                rb.AddForce(force);
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
                    rb.AddForce(force);
                }

                float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
                if (distance < nextWaypointDistance)
                    currentWaypoint++;
            }
        }

    }

    //Funcions
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(hitPoint.position, hitRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            attacking = false;
            attackPointCheck = false;
            ApplyStun(stunDuration);
        } else if (collision.gameObject.CompareTag("Player"))
        {
            Attack();

            attacking = false;
            attackPointCheck = false;
            ApplyStun(stunDuration);
        }
        Debug.Log("Colliding with " + collision.gameObject.name);
    }

    // Actualitza el camí del enemic
    void ChasingPath()
    {
        Debug.Log("chasing");
        seeker.StartPath(rb.position, target.position, OnPathComplete);
    }
    void WanderingPath()
    {
        Debug.Log("Buscant per on errar");
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
        // Animacions

        // Detecció
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(hitPoint.position, hitRange, playerLayer);
        // Atac
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("We hit " + enemy.name);
            enemy.GetComponent<Player>().TakeDamage(attackDamage);
        }
    }
    public void ApplyStun(float duration)
    {
        isStunned = true;
        stunEndTime = Time.time + duration;

        // Agregar animacions + suroll
    }

    public void TakeDamage(int Damage)
    {
        currentHealth -= Damage;
        Debug.Log(currentHealth);

        // Posar aqui l'animació de rebre mal

        // Comprobar si ha mort
        if (currentHealth <= 0)
        {
            die();
        }
    }

    void die()
    {
        Debug.Log("Has mort!");

        // Animació de morir:

        // Desactivar l'enemic
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }
}
