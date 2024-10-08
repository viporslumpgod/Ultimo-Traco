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
        // Configurações iniciais padrão que podem ser modificadas nas classes filhas
        colisorDano = GetComponent<Collider2D>();
        colisorDano.enabled = false;
       
        // Pega todos os colliders que estão no gameObject e nos seus filhos
        collidersAtaques = GetComponentsInChildren<Collider2D>();

        // Verificação para garantir que o array não esteja vazio ou nulo
        if (collidersAtaques == null || collidersAtaques.Length == 0)
        {
            Debug.LogWarning("Nenhum Collider2D foi encontrado!");
        }
        else
        {
            Debug.Log("Colliders encontrados: " + collidersAtaques.Length);
            // Exemplo: Desativa todos os colliders no início
            DesativarTodosColliders();
        }
    }
    private void DesativarTodosColliders()
    {
        foreach (Collider2D col in collidersAtaques)
        {
            if (col != null) // Verifica se o collider não é nulo
            {
                col.enabled = false; // Desativa o collider
            }
        }
    }

    // Função para ativar um collider específico (por índice)
    public void AtivarCollider(int index)
    {
        if (index >= 0 && index < collidersAtaques.Length)
        {
            collidersAtaques[index].enabled = true; // Ativa o collider do índice especificado
        }
        else
        {
            Debug.LogWarning("Índice fora do alcance do array de colliders!");
        }
    }



    public virtual IEnumerator Ataque()
    {
        if (podeAtacar)
        {
            podeAtacar = false;

            // Ativa o colisor de dano e executa a animação de ataque
            colisorDano.enabled = true;

            // Simula o tempo do ataque com um delay
            yield return new WaitForSeconds(velocidadeAtaque);

            // Desativa o colisor de dano após o ataque
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




