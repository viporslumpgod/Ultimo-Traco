using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : inimigo
{
    [Header("attack parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private int damage;
    [SerializeField] private float range;

    [Header("collider parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private CapsuleCollider2D CapsuleCollider;

    [Header("player layer")]
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;

    //referencias
    private Health playerHealth;

    private EnemyPatrol enemyPatrol;

    private void Awake()
    {
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
    }

    private void OnDisable()
    {
        //aqui vc seta a boleana de animção pra quando ele parar de patrulhar.
    }





    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        //atack only when player is on the sight??
        if (PlayerOnSight())
        {
            if (cooldownTimer >= attackCooldown)
            {

                //attack

            }
        }
        if (enemyPatrol != null)
            enemyPatrol.enabled = !PlayerOnSight();
    }


    private void DamagePlayer()
    {

        if (!PlayerOnSight())
        {
            playerHealth.TakeDamage(damage);
            //colocar animmacao e colliderr de dano pra nim do inimigo ele so vai dar dano depois disso.
        }

    }

    private bool PlayerOnSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(CapsuleCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, 
        new Vector3(CapsuleCollider.bounds.size.x * range, CapsuleCollider.bounds.size.y, CapsuleCollider.bounds.size.z), 
        0, Vector2.left, 0, playerLayer);
        return hit.collider != null;

        if (hit.collider != null)
        {
           playerHealth = hit.transform.GetComponent<Health>();
        }


    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(CapsuleCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, new Vector3(CapsuleCollider.bounds.size.x * range, CapsuleCollider.bounds.size.y, CapsuleCollider.bounds.size.z));
    }







}
