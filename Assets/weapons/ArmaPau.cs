using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmaPau : MonoBehaviour
{
    #region Var animacoes 
    Animator animator;
    #endregion

    [SerializeField] private Collider2D[] collidersAtaques;
    protected Collider2D colisorDano;
    [SerializeField] public int dano;
    [SerializeField] public float velocidadeAtaque;
    public bool podeAtacar { get; protected set; } = true;
    protected float tempoUltimoAtaque;
    [SerializeField] protected float tempoParaResetCombo = 1f;

    public void Update()
    {

    }

    protected virtual void Start()
    {
        // Configura��es iniciais padr�o que podem ser modificadas nas classes filhas
        colisorDano = GetComponent<Collider2D>();
        colisorDano.enabled = false;
       
        // Pega todos os colliders que est�o no gameObject e nos seus filhos
        collidersAtaques = GetComponentsInChildren<Collider2D>();

        // Verifica��o para garantir que o array n�o esteja vazio ou nulo
        if (collidersAtaques == null || collidersAtaques.Length == 0)
        {
            Debug.LogWarning("Nenhum Collider2D foi encontrado!");
        }
        else
        {
            Debug.Log("Colliders encontrados: " + collidersAtaques.Length);
            // Exemplo: Desativa todos os colliders no in�cio
            DesativarTodosColliders();
        }
    }
    private void DesativarTodosColliders()
    {
        foreach (Collider2D col in collidersAtaques)
        {
            if (col != null) // Verifica se o collider n�o � nulo
            {
                col.enabled = false; // Desativa o collider
            }
        }
    }

    // Fun��o para ativar um collider espec�fico (por �ndice)
    public void AtivarCollider(int index)
    {
        if (index >= 0 && index < collidersAtaques.Length)
        {
            collidersAtaques[index].enabled = true; // Ativa o collider do �ndice especificado
        }
        else
        {
            Debug.LogWarning("�ndice fora do alcance do array de colliders!");
        }
    }



    public virtual IEnumerator Ataque()
    {
        if (podeAtacar)
        {
            podeAtacar = false;

            // Ativa o colisor de dano e executa a anima��o de ataque
            colisorDano.enabled = true;

            // Simula o tempo do ataque com um delay
            yield return new WaitForSeconds(velocidadeAtaque);

            // Desativa o colisor de dano ap�s o ataque
            colisorDano.enabled = false;

            podeAtacar = true;
        }
    }



    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<inimigo>() != null)
        {
            collision.GetComponent<inimigo>().levaDano(dano);
        }
    }

    public enum Colliders
    {



    }



}




