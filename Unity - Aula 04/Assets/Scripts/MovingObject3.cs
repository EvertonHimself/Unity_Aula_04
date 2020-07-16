using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Abstract: classe incompleta, que precisa ser implementada pelas classes filha.
public abstract class MovingObject3 : MonoBehaviour
{
    // O tempo que uma unidade demora para se mover (em segundos).
    public float moveTime = 0.1f;
    // A camada que checa colisões enquanto movemos e determina se o espaço está livre ou não.
    public LayerMask blockingLayer;

    // O Box Collider 2D desse game object.
    private BoxCollider2D boxCollider;
    // O Rigid Body 2D desse game object.
    private Rigidbody2D rb2d;
    // Para tornar o cálculo do movimento mais eficiente.
    private float inverseMoveTime;

    // Membros "virtual" podem ter uma implementação diferente nas classes filhas.
    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1f / moveTime;
    }

    // "Corrotina", se parece com Update.
    protected IEnumerator SmoothMovement(Vector3 end)
    {

        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(rb2d.position, end, inverseMoveTime * Time.deltaTime);
            rb2d.MovePosition(newPosition);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            yield return null;
        }
    }
}
