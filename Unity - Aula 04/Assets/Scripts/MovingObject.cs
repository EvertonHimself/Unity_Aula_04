using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Abstract classes are incomplete and must be implemented in derived classes.
public abstract class MovingObject : MonoBehaviour
{
    // The time the unit will take to move.
    public float moveTime = .1f;
    // Check collision with this layer to know if the object can move to a given space.
    public LayerMask blockingLayer;

    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;
    // Make movements calculations more efficient.
    private float inverseMoveTime;

    // This start method is virtual so derived classes can implement their own Start().
    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        // The reciproque of inverseTime, so we can use it multiplying instead of diving, which is more efficient.
        inverseMoveTime = 1f / moveTime;
    }

    protected bool Move (int xDir, int yDir, out RaycastHit2D hit)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);

        boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, blockingLayer);
        boxCollider.enabled = true;

        if (hit.transform == null)
        {
            StartCoroutine(SmoothMovement(end));
            return true;
        }

        return false;
    }

    // Coroutine that moves the unit from one space to the next. The parameter "end" is where to move to.
    protected IEnumerator SmoothMovement(Vector3 end)
    {
        // The remaining distance to move.
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);
            rb2D.MovePosition(newPosition);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            yield return null;
        }
    }

    protected virtual void AttemptMove<T>(int xDir, int yDir) where T : Component
    {
        RaycastHit2D hit;
        bool canMove = Move(xDir, yDir, out hit);
        if (hit.transform == null)
        {
            return;
        }

        T hitComponent = hit.transform.GetComponent<T>();
        if (!canMove && hitComponent != null)
        {
            OnCantMove(hitComponent);
        }
    }

    protected abstract void OnCantMove<T>(T component)
        where T : Component;

}
