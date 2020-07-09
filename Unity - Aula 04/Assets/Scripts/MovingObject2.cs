using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Abstract: classes incompletas que precissam ser implementadas nas classes filhas.
public abstract class MovingObject2 : MonoBehaviour
{
    // O tempo que nossa unidade demora para se mover (em segundos).
    public float moveTime = 0.1f;
    // A camada que checa colisões enquanto movemos para determinar se um espaço está livre ou não. É a camada do player, paredes e inimigos.
    public LayerMask blockingLayer;


    private BoxCollider2D boxCollider;
    // O Rigidbody da unidade.
    private Rigidbody2D rb2D;
    // Para tornar o cálculo do movimento das unidades mais eficiente.
    private float inverseMoveTime;

    // Virtual é para permitir que as classes herdadas possam ter uma implementação diferente do start.
    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        // Para usar inverseMoveTime multiplicando, ao invés de dividir por moveTime.
        inverseMoveTime = 1f / moveTime;
    }

    // Uma "co-rotina" para mover nossas unidades de um ponto a outro.
    protected IEnumerator SmoothMovement (Vector3 end)
    {
        // Salva a raíz quadrada da magnitude do vetor resultante. Usamos pois é mais eficiente. Ver operações com vetores.
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);
            rb2D.MovePosition(newPosition);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            // Espera um frame antes de testar o loop novamente.
            yield return null;
        }
    }

    protected abstract void OnCantMove<T>(T component)
        where T : Component;

    // out permite que argumentos sejam passados como referência. Está sendo usado para retornar mais de um valor: o boolean e o RaycastHit2D.
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

    // Usamos o parânetro genérico pois nosso Player e nosso Inimigo irão herdar esse script, então não sabemos
    // com qual tipo de objeto eles irão interagir.
    // O próximo passo é criar o script da parede.
}
