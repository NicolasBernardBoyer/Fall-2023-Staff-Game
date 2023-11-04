using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] Animator animator;

    private enum HorizontalState {Right, Left}
    private enum VerticalState {Up, Neutral, Down}

    HorizontalState currentHState = HorizontalState.Right;
    VerticalState currentVState = VerticalState.Neutral;

    // Animations
    const string RIGHT = "right";
    const string RIGHT_UP = "rightUp";
    const string RIGHT_DOWN = "rightDown";

    const string LEFT = "left";
    const string LEFT_UP = "leftUp";
    const string LEFT_DOWN = "leftDown";

    public void OnMove(Vector2 direction)
    {
        float threshhold = 0.5f;

        float x = direction.x;
        float y = direction.y;

        if (x > threshhold)
        {
            currentHState = HorizontalState.Right;
        } else if (x < -threshhold)
        {
            currentHState = HorizontalState.Left;
        }

        if (y > threshhold)
        {
            currentVState = VerticalState.Up;
        } else if (y < -threshhold)
        {
            currentVState = VerticalState.Down;
        } else
        {
            currentVState = VerticalState.Neutral;
        }

        ChangeAnimation();
    }

    private void ChangeAnimation()
    {
        if (currentHState == HorizontalState.Right)
        {
            switch (currentVState)
            {
                case VerticalState.Up:
                    animator?.Play(RIGHT_UP);
                    break;
                case VerticalState.Neutral:
                    animator?.Play(RIGHT);
                    break;
                case VerticalState.Down:
                    animator?.Play(RIGHT_DOWN);
                    break;
            }
        } else
        {
            switch (currentVState)
            {
                case VerticalState.Up:
                    animator?.Play(LEFT_UP);
                    break;
                case VerticalState.Neutral:
                    animator?.Play(LEFT);
                    break;
                case VerticalState.Down:
                    animator?.Play(LEFT_DOWN);
                    break;
            }
        }
    }

}
