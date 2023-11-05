using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 3;

    [HideInInspector] public bool isMoving;
    private Vector2 input;

    [SerializeField] private PlayerAnimator animator;

    [SerializeField] private float rayLength = 1.2f;

    public LayerMask layer;

    // Update is called once per frame
    private void Update()
    {
        PollPause();
        if (GameManager.Instance.isPaused) return;

        if (!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");
        }

        if (input != Vector2.zero)
        {
            if (input.x != 0) input.y = 0;

            Vector2 targetPos = new Vector2(
                transform.position.x + input.x,
                transform.position.y + input.y
            );

            Vector2 moveDirection = targetPos - new Vector2(transform.position.x, transform.position.y);

            animator.OnMove(moveDirection);
            input = Vector2.zero;

            if (CollisionCheck(moveDirection)) return;

            // On success
            StartCoroutine(Move(targetPos));
            GameManager.Instance.IncrementTurn();
        }  
    }

    private void PollPause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.Instance.isPaused)
            {
                UIManager.Instance.Resume();
            } else GameManager.Instance.Pause();
        }
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
        animator.OnMove(Vector2.zero);
    }

    private bool CollisionCheck(Vector2 direction)
    {
        Vector3 ortho = Vector3.Cross(direction, Vector3.forward).normalized;

        RaycastHit2D hit1 = Physics2D.Raycast(transform.position, direction.normalized, rayLength, layer);
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position + 0.4f * ortho, direction.normalized, rayLength, layer);
        RaycastHit2D hit3 = Physics2D.Raycast(transform.position - 0.4f * ortho, direction.normalized, rayLength, layer);
        return hit1.collider != null || hit2.collider != null || hit3.collider != null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.Instance.GameOver();
    }
}
