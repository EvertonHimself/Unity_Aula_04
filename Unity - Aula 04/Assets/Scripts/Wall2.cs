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

    // Os sons de destruição das paredes.
    public AudioClip chopSound1;
    public AudioClip chopSound2;
    // Vá para DamageWall.

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void DamageWall(int loss)
    {
        // Randomiza os efeitos sonoros.
        SoundManager.instance.RandomizeSfx(chopSound1, chopSound2);
        spriteRenderer.sprite = dmgSprite;
        hp -= loss;
        if (hp <= 0 )
        {
            gameObject.SetActive(false);
        }
    }
}
