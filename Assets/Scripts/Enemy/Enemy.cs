using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour//, iEnemy
{
    public GameObject deathEffect;
    public GameObject hitEffect;
    public GameObject coinPrefab;
    public int maxHealth = 100;
    public int currentHealth;
    public int ID { get; set; }
    public float startDazedTime;
    private float dazedTime;
    public float speed;
    public float nextWaypointDistance = 1f;
    public string enemyQuestName;
    public Transform target;
    public Transform enemyGFX;
    bool reachedEndOfPath = false;
    int currentWaypoint = 0;
    Path path;
    Seeker seeker;
    Rigidbody2D rb;
    private QuestManager theQM;
    public Animator anim;
    public bool Walking;
    public bool Attacking;

    //public Transform target;
    public float attackRange;
    public int damage;
    private float lastAttackTime;
    public float attackDelay;
    //public Animator anim;


    void Start()
    {
        currentHealth = maxHealth;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        

        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating("UpdatePath", 0f, .1f);
        ID = 0;

        theQM = FindObjectOfType<QuestManager>();
    }
    void Update()
    {
        damage = UnityEngine.Random.Range(100, 200);
        float distanceToPlayer = Vector3.Distance(transform.position, target.position);
        if (distanceToPlayer < attackRange)
        {
            if (Time.time > lastAttackTime + attackDelay)
            {
            Attacking = true;   
            Walking = false;

                target.SendMessage("TakeDamage", damage);
                lastAttackTime = Time.time;
                Debug.Log("playerHit");
                anim.SetBool("Attacking", Attacking);
                Attacking = true;
                //anim.Play("Attack");
            }
        }
        if (Attacking == true)
        {
            //Walking = false;
            anim.Play("Attack");    
        }

        if (distanceToPlayer > attackRange)
        {
            Attacking = false;
            //Walking = true;
        }

        if (dazedTime <= 0)
        {
            speed = 1;
        }
        else
        {
            speed = 0;
            dazedTime -= Time.deltaTime;
        }

        if (Vector2.Distance(transform.position, target.position) < 2 && Attacking == false)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            anim.SetBool("Walking", Walking);
            Walking = true;
            //anim.Play("Run");
        }
        if (Walking == true)
        {
            anim.Play("Run");
        }

        if (Vector2.Distance(transform.position, target.position) < 2)
            if (path == null || path != null)
                return;
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
        Vector2 force = direction * speed;
        
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
       
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
            
        }
        if (rb.velocity.x >= 0.01f)
        {
            enemyGFX.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (rb.velocity.x <= -0.01f)
        {
            enemyGFX.localScale = new Vector3(1f, 1f, 1f);
        }


        
    }
   
    public void TakeDamage(int damage)
    {
        dazedTime = startDazedTime;
        currentHealth -= damage;
        Debug.Log("I am Hit");
        Instantiate(hitEffect, transform.position, Quaternion.identity);
        if(currentHealth <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        theQM.enemyKilled = enemyQuestName;
        Debug.Log("Enemy Died");
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Instantiate(coinPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    void UpdatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        
    }
    void OnPathComplete(Path p)
    {
        if (Vector2.Distance(transform.position, target.position) < 10)
            if (!p.error)
            {
                path = p;
                currentWaypoint = 0;
            }
    }
   
    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
