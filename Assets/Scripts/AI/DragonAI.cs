using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DragonAI : MonoBehaviour
{

    public Animator anim;
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

    [Header("Variables")]
    public float FlightSpeed = 10f;
    public float rotationSpeed = 2f;
    public float attackDistance = 10f;
    public float attackCooldown = 3f;
    public float FireballCooldown = 7f;

    public GameObject DragonMouth;
    public float FireBallSpeed;
    public int Damage= 10;

    [Header("Range check")]
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    public GameObject FireBallEffect;

    private bool isAttacking = false;
    private bool isAttckingMagic = false;

    private void Update()
    {
        if (player == null)
            player = FindObjectOfType<Character>().transform;

        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange)
        {
            Sleeping();
        }
        if (playerInSightRange && !playerInAttackRange)
        {
            anim.SetTrigger("Active");
            ChasePlayer();
        }
        if (playerInSightRange && playerInAttackRange)
        {
            Attack();
        }
    }
    private void Sleeping()
    {
        anim.SetFloat("Speed", 0f);
    }
    private void Flying()
    {

    }
    private void ChasePlayer()
    {
        anim.SetFloat("Speed",1f);
        agent.SetDestination(player.position);
        transform.LookAt(player.position);
    }
    private void Attack()
    {
        MeleeAttack();
        FireAttack();
    }
    #region Simpleattck
    private void MeleeAttack()
    {
        anim.SetBool("attack", isAttacking);
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if (!isAttacking)
        {
            if (Physics.Raycast(DragonMouth.transform.position, DragonMouth.transform.forward, out RaycastHit hit, 2f))
            {
                Debug.Log(hit.transform.name);
                if (hit.transform.TryGetComponent<Character>(out var character))
                {
                    character.TakeDamage(Damage);
                }
            }
            isAttacking = true;
            Invoke(nameof(EndAttack), attackCooldown);
        }
    }
    private void EndAttack()
    {
        isAttacking = false;
    }
    #endregion

    #region fireAttack
    private void FireAttack()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if (!isAttckingMagic)
        {
            GameObject fireball = Instantiate(FireBallEffect, DragonMouth.transform.position, transform.rotation);
            Rigidbody projectileRb = fireball.GetComponent<Rigidbody>();
            Vector3 forceDiretion = transform.forward;
            if (Physics.Raycast(DragonMouth.transform.position, DragonMouth.transform.forward, out RaycastHit hit, 500f))
            {
                forceDiretion = (hit.point - DragonMouth.transform.position).normalized;
                Vector3 forcetoAdd = forceDiretion * FireBallSpeed;
                projectileRb.AddForce(forcetoAdd, ForceMode.Impulse);
                Debug.Log(hit.transform.name);
            }
            isAttckingMagic = true;
            Invoke(nameof(ResetAttack), FireballCooldown);
        }
    }
    private void ResetAttack()
    {
        isAttckingMagic = false;
    }
    #endregion

    private void MoveTowardsTarget()
    {
        // Rotate the dragon to face the target
        Vector3 targetDirection = player.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

        // Move the dragon forward
        transform.Translate(Vector3.forward * FlightSpeed * Time.deltaTime);
    }


}
