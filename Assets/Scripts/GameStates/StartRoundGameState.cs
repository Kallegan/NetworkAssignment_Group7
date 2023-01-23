using Alteruna;

public class StartRoundGameState : GameState
{
    public override void Update()
    {
    }

    public override void Run()
    {
        WorldManager.Instance.StartShrinkGrid();
#if UNITY_EDITOR
        GameManager.Instance.PrintDebug("GameManager - ", "Game started! Shrinking of map enabled");
#endif
    }
}