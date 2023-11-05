using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed;

    private enum Direction { Down, Right, Left, Up }
    private bool isMoving = false;

    private List<Direction> availableDirections;

    private void Awake()
    {
        availableDirections = new List<Direction> {
            Direction.Up, Direction.Down, Direction.Right, Direction.Left
        };

        GameManager.onTurn += (int a) => MoveEnemy();
    }

    private void MoveEnemy()
    {
        if (isMoving) return;

        int randomIndex = (int) Random.Range(0.0f, (float)availableDirections.Count);

        Direction dir = availableDirections[randomIndex];

        Vector3 targetPos = Vector3.zero;
        switch (dir)
        {
            case Direction.Up:
                targetPos = transform.position + Vector3.up;
                break;
            case Direction.Down:
                targetPos = transform.position + Vector3.down;
                break;
            case Direction.Right:
                targetPos = transform.position + Vector3.right;
                break;
            case Direction.Left:
                targetPos = transform.position + Vector3.left;
                break;
        }

        StartCoroutine(Move(targetPos));
    }

    IEnumerator Move(Vector3 target)
    {
        isMoving = true;

        while ((target - transform.position).magnitude > Mathf.Epsilon)
        {
            transform.position =
                Vector3.MoveTowards(transform.position, target, movementSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = target;

        isMoving = false;
    }
}
