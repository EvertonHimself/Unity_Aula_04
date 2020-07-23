using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// O script da parede.
public class Wall3 : MonoBehaviour
{
    // O sprite que aparece quando a parede é atingida.
    public Sprite dmgSprite;
    // A quantidade de dano que a parede pode sofrer.
    public int hp = 4;

    // O componente SpriteRenderer da parede.
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Causar danos a parede, passando como parâmetro a "força" do dano.
    public void DamageWall(int loss)
    {
        spriteRenderer.sprite = dmgSprite;
        hp -= 1;

        if (hp <= 0)
        {
            // Usaremos o SetActive ao invés de Destroy.
            gameObject.SetActive(false);
        }
    }
}
