using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChasePlayerWithPatrol : MonoBehaviour
{
    [Header("Player Settings")]
    public Transform player;
    public float detectionRange = 10f;
    public float stoppingDistance = 2f;
    public LayerMask obstacleLayer;

    [Header("Patrol Settings")]
    public Transform[] patrolPoints;
    public float patrolSpeed = 3f;
    public float chaseSpeed = 5f;
    public float waitTimeAtPoint = 2f;
    public float distanceThreshold = 0.5f;

    private NavMeshAgent agent;
    private int currentPatrolIndex = 0;
    private float waitTimer = 0f;
    private bool isWaiting = false;
    private bool isChasing = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = stoppingDistance;

        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) player = playerObj.transform;
        }

        if (patrolPoints.Length > 0)
        {
            MoveToPatrolPoint();
        }
    }

    void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            bool canSeePlayer = distanceToPlayer <= detectionRange &&
                              !Physics.Linecast(transform.position, player.position, obstacleLayer);

            if (canSeePlayer)
            {
                if (!isChasing)
                {
                    isChasing = true;
                    agent.speed = chaseSpeed;
                    agent.stoppingDistance = stoppingDistance;
                }
                agent.SetDestination(player.position);

                if (distanceToPlayer <= stoppingDistance * 1.5f)
                {
                    FaceTarget(player.position);
                }
            }
            else
            {
                if (isChasing)
                {
                    isChasing = false;
                    agent.speed = patrolSpeed;
                    agent.stoppingDistance = 0f;
                    MoveToPatrolPoint();
                }

                if (patrolPoints.Length > 0 && !isChasing)
                {
                    Patrol();
                }
            }
        }
    }

    void Patrol()
    {
        if (!agent.pathPending && agent.remainingDistance <= distanceThreshold)
        {
            if (!isWaiting)
            {
                isWaiting = true;
                waitTimer = waitTimeAtPoint;
            }
            else
            {
                waitTimer -= Time.deltaTime;
                if (waitTimer <= 0f)
                {
                    isWaiting = false;
                    GetNextPatrolPoint();
                    MoveToPatrolPoint();
                }
            }
        }
    }

    void MoveToPatrolPoint()
    {
        if (patrolPoints.Length > 0)
        {
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }
    }

    void GetNextPatrolPoint()
    {
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }

    void FaceTarget(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}