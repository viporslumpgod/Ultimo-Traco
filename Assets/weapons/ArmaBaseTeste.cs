using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmaBaseTeste : MonoBehaviour
{
    protected Collider2D colisorDano;
    protected virtual int dano { get; set; }
    protected virtual float velocidadeAtaque { get; set; }
    protected bool podeAtacar;

    protected int passoCombo = 0; // Passo atual do combo
    protected int totalDeCombos = 3; // Total de ataques no combo
    protected float tempoUltimoAtaque;
    protected float tempoParaResetCombo = 1f; // Tempo máximo entre ataques no combo

    protected virtual void Start()
    {
        colisorDano = GetComponent<Collider2D>();
        colisorDano.enabled = false;
    }

    // Método de ataque usando coroutine para controlar o tempo de ativação do colisor
    public virtual IEnumerator Ataque()
    {
        if (!podeAtacar) yield break;

        podeAtacar = false;
        passoCombo++;

        // Limita o combo ao número máximo de ataques
        if (passoCombo > totalDeCombos)
        {
            passoCombo = 1;
        }

        // Ativa a animação do ataque correspondente ao passo do combo
        GetComponent<Animator>().SetTrigger("Ataque" + passoCombo);

        // Ativa o colisor para causar dano
        colisorDano.enabled = true;
        yield return new WaitForSeconds(velocidadeAtaque);
        colisorDano.enabled = false;

        podeAtacar = true;
        tempoUltimoAtaque = Time.time; // Atualiza o tempo do último ataque
    }

    protected void ResetarCombo()
    {
        passoCombo = 0;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        // Causa dano ao inimigo caso ele colida com o colisor durante o ataque
        if (collision.GetComponent<inimigo>() != null)
        {
            collision.GetComponent<inimigo>().levaDano(dano);
        }
    }
}

