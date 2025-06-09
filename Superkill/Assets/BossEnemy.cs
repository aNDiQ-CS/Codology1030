using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossEnemy : MonoBehaviour
{
    [Header("Movement Settings")]
    public float normalSpeed = 3.5f;
    public float rushSpeed = 10f;
    public float rushDistance = 15f;
    public float rushCooldown = 8f;
    public float stoppingDistance = 2f;

    [Header("Attack Settings")]
    public float attackRange = 2.5f;
    public int attackDamage = 20;
    public float attackCooldown = 2f;
    public float attackWindupTime = 0.5f;

    [Header("References")]
    public Transform player;
    public Animator animator;
    public ParticleSystem rushParticles;

    private NavMeshAgent agent;
    private float lastAttackTime;
    private float lastRushTime;
    private bool isRushing;
    private bool isAttacking;
    private Vector3 rushTarget;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = normalSpeed;
        agent.stoppingDistance = stoppingDistance;

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void Update()
    {
        if (isAttacking || player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Если не в процессе рывка и не на кулдауне - проверяем возможность рывка
        if (!isRushing && Time.time > lastRushTime + rushCooldown && distanceToPlayer > rushDistance)
        {
            StartRush();
        }

        // Если в процессе рывка - двигаемся к цели рывка
        if (isRushing)
        {
            agent.SetDestination(rushTarget);

            // Проверяем достигли ли цели рывка
            if (Vector3.Distance(transform.position, rushTarget) < 1f)
            {
                EndRush();
            }
            return;
        }

        // Обычное преследование
        agent.SetDestination(player.position);

        // Проверка атаки
        if (distanceToPlayer <= attackRange && Time.time > lastAttackTime + attackCooldown)
        {
            StartAttack();
        }
    }

    void StartRush()
    {
        isRushing = true;
        lastRushTime = Time.time;
        agent.speed = rushSpeed;
        agent.stoppingDistance = 0f;

        // Устанавливаем цель рывка - позицию игрока с предсказанием
        rushTarget = player.position + player.forward * 3f; // Предсказываем движение игрока

        // Визуальные эффекты
        if (rushParticles != null) rushParticles.Play();
        if (animator != null) animator.SetTrigger("Rush");
    }

    void EndRush()
    {
        isRushing = false;
        agent.speed = normalSpeed;
        agent.stoppingDistance = stoppingDistance;

        if (rushParticles != null) rushParticles.Stop();
    }

    void StartAttack()
    {
        isAttacking = true;
        lastAttackTime = Time.time;
        agent.isStopped = true;

        if (animator != null) animator.SetTrigger("Attack");
     

        // Задержка перед нанесением урона (синхронизация с анимацией)
        Invoke("PerformAttack", attackWindupTime);
    }

    void PerformAttack()
    {
        // Проверяем все еще ли игрок в радиусе атаки
        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            // Наносим урон игроку
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
            }
            Debug.Log("attack");
        }

        // Возобновляем движение
        agent.isStopped = false;
        isAttacking = false;
    }

    // Визуализация в редакторе
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rushDistance);

        if (isRushing)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, rushTarget);
            Gizmos.DrawSphere(rushTarget, 0.5f);
        }
    }
}