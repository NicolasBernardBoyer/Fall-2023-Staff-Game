using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class MaxMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed= 4.0f;

    private enum Direction { Down, Right, Left, Up, None }
    private bool isMoving = false;

    private List<Direction> availableDirections;

    public LayerMask wallLayer;
    public LayerMask playerLayer;
    public LayerMask enemyLayer;
    [SerializeField] private float rayLength = 1.2f;
    [SerializeField] private int playerSightRange = 5;

    private Vector3 prevTile;

    private void Awake()
    {
        availableDirections = new List<Direction> {
            Direction.Up, Direction.Down, Direction.Right, Direction.Left
        };

        GameManager.onTurn += (int a) => MoveMax();
    }

    private void Start()
    {
        if (gameObject.tag == "Base")
        {
            DimensionManager.Instance.baseDimEnemies.Add(gameObject);
            gameObject.SetActive(true);
        }
        else
        {
            DimensionManager.Instance.alternateDimEnemies.Add(gameObject);
            gameObject.SetActive(false);
        }
    }

    private void MoveMax()
    {
        if (isMoving || !gameObject.activeSelf) return;

        SetupAvailableDirections();
        Vector3 targetPos = Vector3.zero;

        Direction direction = FindPlayer();
        Debug.Log(direction);
        bool directionIsAvailable = availableDirections.Contains(direction);

        if (!directionIsAvailable || direction == Direction.None)
        {
            if (availableDirections.Count > 1)
            {
                switch (direction)
                {
                    case Direction.Up:
                        if (availableDirections.Contains(Direction.Down))
                            availableDirections.Remove(Direction.Down);
                        break;
                    case Direction.Down:
                        if (availableDirections.Contains(Direction.Up))
                            availableDirections.Remove(Direction.Up);
                        break;
                    case Direction.Right:
                        transform.localScale = new Vector3(-1, 1, 1);
                        if (availableDirections.Contains(Direction.Left))
                            availableDirections.Remove(Direction.Left);
                        break;
                    case Direction.Left:
                        transform.localScale = new Vector3(1, 1, 1);
                        if (availableDirections.Contains(Direction.Right))
                            availableDirections.Remove(Direction.Right);
                        break;
                }
            }

            int randomIndex = (int)Random.Range(0.0f, availableDirections.Count);
            direction = availableDirections[randomIndex];
        }

        switch (direction)
        {
            case Direction.Up:
                targetPos = transform.position + Vector3.up;
                break;
            case Direction.Down:
                targetPos = transform.position + Vector3.down;
                break;
            case Direction.Right:
                transform.localScale = new Vector3(-1, 1, 1);
                targetPos = transform.position + Vector3.right;
                break;
            case Direction.Left:
                transform.localScale = new Vector3(1, 1, 1);
                targetPos = transform.position + Vector3.left;
                break;
        }

        prevTile = transform.position;

        StartCoroutine(Move(targetPos));
    }

    private IEnumerator Move(Vector3 target)
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

    private Direction FindPlayer()
    {
        RaycastHit2D hitUp = Physics2D.Raycast(
            transform.position, Vector2.up, playerSightRange, playerLayer);
        if (hitUp.collider != null) return Direction.Down;

        RaycastHit2D hitDown = Physics2D.Raycast(
            transform.position, Vector2.down, playerSightRange, playerLayer);
        if (hitDown.collider != null) return Direction.Up;

        RaycastHit2D hitRight = Physics2D.Raycast(
            transform.position, Vector2.right, playerSightRange, playerLayer);
        if (hitRight.collider != null) return Direction.Left;

        RaycastHit2D hitLeft = Physics2D.Raycast(
            transform.position, Vector2.left, playerSightRange, playerLayer);
        if (hitLeft.collider != null) return Direction.Right;

        return Direction.None;
    }

    private void SetupAvailableDirections()
    {
        availableDirections = new List<Direction>();

        if (!CollisionCheck(Vector2.up)) availableDirections.Add(Direction.Up);
        if (!CollisionCheck(Vector2.down)) availableDirections.Add(Direction.Down);
        if (!CollisionCheck(Vector2.right)) availableDirections.Add(Direction.Right);
        if (!CollisionCheck(Vector2.left)) availableDirections.Add(Direction.Left);
    }

    private bool CollisionCheck(Vector2 direction)
    {
        //Vector3 ortho = Vector3.Cross(direction, Vector3.forward).normalized;

        RaycastHit2D hit1 = Physics2D.Raycast(
            transform.position, direction.normalized, rayLength, wallLayer);
        //RaycastHit2D hit2 = Physics2D.Raycast(
        //    transform.position + 0.2f * ortho, direction.normalized, rayLength, wallLayer);
        //RaycastHit2D hit3 = Physics2D.Raycast(
        //    transform.position - 0.2f * ortho, direction.normalized, rayLength, wallLayer);

        return hit1.collider != null;
    }
}