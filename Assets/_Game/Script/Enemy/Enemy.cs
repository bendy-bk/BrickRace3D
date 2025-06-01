using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    [Header("Enemy")]
    [SerializeField] private int targetIndex = 0;
    [SerializeField] private int maxTargetIndex = 5;       // điểm cuối để về home
    [SerializeField] private float reachThreshold = 0.2f;  // khoảng cách để tính đã đến điểm
    [SerializeField] private PoolControl poolControl;

    [Header("Navmesh")]
    [SerializeField] private NavMeshAgent agent;
    private Vector3 homePosition;

    [Header("StateMachine")]
    [SerializeField] private IState currentState;

    public PoolControl PoolControl { get => poolControl; set => poolControl = value; }
    public int TargetIndex { get => targetIndex; set => targetIndex = value; }
    public int MaxTargetIndex { get => maxTargetIndex; set => maxTargetIndex = value; }

    private List<Vector3> listPatrolPoints ;

    void Start()
    {
        
    }

    void Update()
    {
       
    }
    public override void OnInit()
    {
        base.OnInit();
        ChangeState(new IdleState());
    }

    public override void OnDespawn(GameObject g)
    {
        base.OnDespawn(g);
    }


    public void EnemyMovePointTarget()
    {
        if (listPatrolPoints == null || listPatrolPoints.Count == 0) return;

        // Kiểm tra nếu agent đã gần đến điểm đích hiện tại
        if (!agent.pathPending && agent.remainingDistance <= reachThreshold)
        {
            TargetIndex++;
            Debug.Log(TargetIndex); 
            // Nếu tới target thứ maxTargetIndex hoặc vượt quá số điểm, về home và reset
            if (TargetIndex > MaxTargetIndex || TargetIndex >= listPatrolPoints.Count)
            {
                agent.SetDestination(homePosition);
                TargetIndex = 0;
            }
            else
            {
                agent.SetDestination(listPatrolPoints[TargetIndex]);
            }
        }
    }

    public void CheckNevMeshAgent()
    {
        if (agent == null)
        {
            Debug.LogError("Enemy requires a NavMeshAgent component!");
            enabled = false;
            return;
        }

        if (listPatrolPoints == null || listPatrolPoints.Count == 0)
        {
            Debug.LogError("Patrol points chưa được thiết lập hoặc rỗng!");
            enabled = false;
            return;
        }

        homePosition = transform.position;
        TargetIndex = 0;

        agent.SetDestination(listPatrolPoints[TargetIndex]);
    }

    public void GetListPoint()
    {
        listPatrolPoints = PoolControl.RandomPositions;
        foreach (var pos in listPatrolPoints)
        {
            Debug.Log(pos);
        }
    }

    public void ChangeState(IState newState)
    {
        if (currentState != null)
        {
            currentState.OnExit(this);
        }

        currentState = newState;

        if (currentState != null)
        {
            currentState.OnEnter(this);

        }

    }

    internal void Moving()
    {
        throw new NotImplementedException();
    }

    internal void StopMoving()
    {
        throw new NotImplementedException();
    }

}
