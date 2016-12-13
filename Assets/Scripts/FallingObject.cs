using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : MonoBehaviour
{
    public float moveTime = 0.3f;
    public LayerMask blockingLayer;

    private Vector2 moveTarget;
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;
    private float inverseMoveTime;

    void Start()
    {

    }

    private bool IsMoving()
    {
        Vector2 current = transform.position;
        return !moveTarget.Equals(current);
    }

    public void FallSetup()
    {
        if (!IsMoving())
        {
            SetFallTarget();
        }
    }

    public void SetFallTarget()
    {
    }

    public void Move()
    {
        if (IsMoving())
        {
            RaycastHit2D hit;
            bool canMove = Move(moveTarget, out hit);

            if (!canMove)
            {
                //Debug.Log("Cant move to (" + moveTarget.x + "," + moveTarget.y + ")");
            }
        }
    }

    protected bool Move(Vector2 target, out RaycastHit2D hit)
    {
        Vector2 start = transform.position;

        boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, target, blockingLayer);
        boxCollider.enabled = true;

        if (hit.transform == null)
        {
            StartCoroutine(SmoothMovement(target));
            return true;
        }

        return false;
    }

    protected IEnumerator SmoothMovement(Vector3 target)
    {
        float sqrRemaingDistance = (transform.position - target).sqrMagnitude;

        while (sqrRemaingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(rb2D.position, target, inverseMoveTime * Time.deltaTime);
            rb2D.MovePosition(newPosition);
            sqrRemaingDistance = (transform.position - target).sqrMagnitude;
            yield return null;
        }
    }
}
