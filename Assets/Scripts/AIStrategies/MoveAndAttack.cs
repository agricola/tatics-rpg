using System;

public class MoveAndAttack : IEnemyStrategy
{
    public IActionStrategy ActionStrategy
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public IMoveStrategy MoveStrategy
    {
        get
        {
            throw new NotImplementedException();
        }
    }
}
