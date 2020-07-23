using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

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

    // Detecta quando o personagem ou inimigo não pode se mover.
    protected abstract void OnCantMove<T>(T component)
        where T : Component;

    // Movimenta esse objeto. "out": Usado para retornar mais de um valor, no caso o RaycastHit2D, além do bool.
    protected bool Move (int xDir, int yDir, out RaycastHit2D hit)
    {
        // Posição atual do objeto.
        Vector2 start = transform.position;
        // Posição final (para onde ele deve ir).
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

    // Virtual: significa que esse método pode ser sobrescrito na classe filha (Enemy ou Player).
    // Generic Methods, ou "métodos genéricos".
    protected virtual void  AttemptMove<T>(int xDir, int yDir) where T:Component
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
}
