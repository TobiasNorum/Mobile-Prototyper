using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
public class PlayerCombat : MonoBehaviour
{
    public Transform attackPoint;
    public Animator anim;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public int attackDamage;
    public float attackRate = 2f;
    float nextAttackTime = 0f;
  
    void Update()
    {
        if(Time.time > nextAttackTime)
        {
            if (CrossPlatformInputManager.GetButtonDown("Fire1"))
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
                Debug.Log("attack");
            }
        }
    }
    
    void Attack()
    {
        
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach(Collider2D enemy in hitEnemies)
        {
            attackDamage = UnityEngine.Random.Range(100, 200);
            bool isCritical = UnityEngine.Random.Range(0, 100) < 30;
            if (isCritical) attackDamage *= 2; 
            Debug.Log("We Hit" + enemy.name);
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
            DamagePopup.Create(enemy.GetComponent<Enemy>().GetPosition(), attackDamage, isCritical);
            //DamagePopup.Create(attackPoint.position(), attackDamage, isCritical);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
