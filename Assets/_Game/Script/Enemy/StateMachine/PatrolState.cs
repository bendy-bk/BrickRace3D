        
public class PatrolState : IState
{
    public void OnEnter(Enemy enemy)
    {
        enemy.GetListPoint();
        enemy.CheckNevMeshAgent();
        enemy.TargetIndex = 0;
    }

    public void OnExcute(Enemy enemy)
    {
        enemy.EnemyMovePointTarget();
    }

    public void OnExit(Enemy enemy)
    {

    }
}

