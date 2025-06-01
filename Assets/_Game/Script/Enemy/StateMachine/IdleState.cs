
public class IdleState : IState
{
    public void OnEnter(Enemy enemy)
    {
        enemy.StopMoving();
    }

    public void OnExcute(Enemy enemy)
    {
        enemy.Moving();
    }

    public void OnExit(Enemy enemy)
    {
    }
}

