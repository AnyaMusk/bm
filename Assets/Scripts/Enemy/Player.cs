using UnityEngine;

public class Player : MovableUnit
{
    // Upon turn start disable input
    protected override void TurnStartCallBack()
    {
        GameManager.instance.inputAllowed = false;
    }
    // Upon turn end, Declare its end
    protected override void TurnEndCallBack()
    {
        GameManager.instance.turnManager.EndPlayerTurn();
    }
}
