public interface IEnemyStrategy
{
    IMoveStrategy MoveStrategy { get; }
    IActionStrategy ActionStrategy { get; }
}