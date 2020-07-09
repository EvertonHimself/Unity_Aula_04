using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall2 : MonoBehaviour
{
    // O sprite que aparece quando a parede é atingida.
    public Sprite dmgSprite;
    // A quantidade de danos que a parede pode sofrer.
    public int hp = 4;

    // O componente SpriteRenderer da parede.
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void DamageWall(int loss)
    {
        spriteRenderer.sprite = dmgSprite;
        hp -= loss;
        if (hp <= 0 )
        {
            gameObject.SetActive(false);
        }
    }
}
