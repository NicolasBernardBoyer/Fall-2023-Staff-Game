using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 3;

    public bool isMoving;
    private Vector2 input;

    // Update is called once per frame
    private void Update()
    {
        if (!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");
        }

        if (input != Vector2.zero)
        {
            Vector3 targetPos = new Vector3(
                transform.position.x + input.x,
                transform.position.y + input.y
            );

            StartCoroutine(Move(targetPos));

            input = Vector2.zero;
            GameManager.Instance.IncrementTurn();
        }

        
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
