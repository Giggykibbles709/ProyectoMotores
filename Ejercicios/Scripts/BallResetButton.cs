using UnityEngine;

public class BallResetButton : MonoBehaviour
{
    public BallController ball;

    public void OnResetButtonClick()
    {
        if (ball != null)
        {
            ball.ResetBall();
        }
    }
}
