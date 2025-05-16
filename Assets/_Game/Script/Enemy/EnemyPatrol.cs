using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class EnemyPatrol : MonoBehaviour
{
    public NavMeshAgent agent;

    [Header("Ground Detection")]
    public LayerMask groundLayer;
    public float groundSize = 20f;

    [Header("Patrol")]
    public Transform homePoint;        // Điểm quay về
    public int patrolCount = 5;        // Số điểm random

    private List<Vector3> patrolPoints = new List<Vector3>();
    private int currentIndex = 0;
    private bool returningHome = false;

    private void Start()
    {
        GenerateRandomPatrolPoints();
        MoveToNextPoint();
    }

    private void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            if (returningHome)
            {
                // Đã về đến home => reset lại
                returningHome = false;
                GenerateRandomPatrolPoints();
                currentIndex = 0;
            }
            else
            {
                currentIndex++;
            }

            MoveToNextPoint();
        }
    }

    void MoveToNextPoint()
    {
        if (currentIndex >= patrolPoints.Count)
        {
            // Hết 5 điểm, quay về home
            returningHome = true;
            agent.SetDestination(homePoint.position);
        }
        else
        {
            agent.SetDestination(patrolPoints[currentIndex]);
        }
    }

    void GenerateRandomPatrolPoints()
    {
        patrolPoints.Clear();
        for (int i = 0; i < patrolCount; i++)
        {
            Vector3 randomPos = GetRandomPointOnGround();
            patrolPoints.Add(randomPos);
        }
    }

    Vector3 GetRandomPointOnGround()
    {
        Vector3 origin = transform.position;
        for (int attempts = 0; attempts < 20; attempts++)
        {
            float randX = Random.Range(-groundSize, groundSize);
            float randZ = Random.Range(-groundSize, groundSize);
            Vector3 checkPos = origin + new Vector3(randX, 10f, randZ);

            // Raycast xuống để tìm mặt đất
            if (Physics.Raycast(checkPos, Vector3.down, out RaycastHit hit, 20f, groundLayer))
            {
                return hit.point;
            }
        }

        // Nếu không tìm được, trả về vị trí hiện tại
        return transform.position;
    }
}
