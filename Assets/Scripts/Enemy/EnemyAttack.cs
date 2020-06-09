using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public Transform target;
    public float attackRange;
    public int damage;
    private float lastAttackTime;
    public float attackDelay;
    public Animator anim;
    //public Enemy enemu;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        //GameObject enemy = GameObject.FindGameObjectWithTag("enemy");

        //enemu = enemy.GetComponent<Enemy>();
    }
    void Update()
    {
        damage = UnityEngine.Random.Range(100, 200);
        float distanceToPlayer = Vector3.Distance(transform.position, target.position);
        if (distanceToPlayer <attackRange)
        {
            if(Time.time > lastAttackTime + attackDelay)
            {
            //enemu.Walking = false;

            target.SendMessage("TakeDamage", damage);
            lastAttackTime = Time.time;
                Debug.Log("playerHit");
                anim.Play("Attack");
            }
        }
    }
}
