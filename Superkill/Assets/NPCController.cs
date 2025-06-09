using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    [Header("Настройки патрулирования")]
    public Transform[] patrolPoints;
    public float patrolSpeed = 2f;
    public float waitTimeAtPoint = 2f;

    [Header("Настройки преследования")]
    public float chaseSpeed = 4f;
    public float chaseRange = 10f;
    public float stopDistance = 20f;
    public float rotationSpeed = 5f;

    private NavMeshAgent agent;
    private Transform player;
    private int currentPatrolIndex;
    private float waitCounter;
    private bool isWaiting;
    private bool isChasing;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (patrolPoints.Length > 0)
        {
            agent.SetDestination(patrolPoints[0].position);
        }

        agent.speed = patrolSpeed;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Проверка, нужно ли начать преследование
        if (distanceToPlayer <= chaseRange && distanceToPlayer > stopDistance)
        {
            isChasing = true;
            ChasePlayer();
        }
        else if (distanceToPlayer <= stopDistance)
        {
            isChasing = false;
            StopNearPlayer();
        }
        else
        {
            isChasing = false;
            Patrol();
        }
    }

    void ChasePlayer()
    {
        agent.speed = chaseSpeed;
        agent.stoppingDistance = stopDistance;

        // Рассчитываем позицию на stopDistance единиц перед игроком
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        Vector3 targetPosition = player.position - directionToPlayer * stopDistance;

        agent.SetDestination(targetPosition);

        // Плавный поворот к игроку
        if (agent.velocity.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(agent.velocity.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void StopNearPlayer()
    {
        agent.ResetPath();

        // Плавный поворот к игроку
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    void Patrol()
    {
        agent.speed = patrolSpeed;
        agent.stoppingDistance = 0f;

        if (patrolPoints.Length == 0) return;

        if (agent.remainingDistance <= agent.stoppingDistance && !isWaiting)
        {
            isWaiting = true;
            waitCounter = waitTimeAtPoint;
        }

        if (isWaiting)
        {
            waitCounter -= Time.deltaTime;
            if (waitCounter <= 0)
            {
                isWaiting = false;
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
                agent.SetDestination(patrolPoints[currentPatrolIndex].position);
            }
        }
    }

    // Визуализация в редакторе
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
    }
}
