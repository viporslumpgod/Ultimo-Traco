using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Espatula : MonoBehaviour
{
    public static Espatula instance;

    // Referências para os GameObjects dentro da arma
    [SerializeField] private GameObject objeto1; // GameObject 1
    [SerializeField] private GameObject objeto2; // GameObject 2
    [SerializeField] private GameObject objeto3; // GameObject 3

    // Colliders dos GameObjects
    private Collider2D colisor1;
    private Collider2D colisor2;
    private Collider2D colisor3;

    private CapsuleCollider2D playerCollider; // o colider do palyer

    protected Collider2D colisorDano;
    [SerializeField] public int dano;
    [SerializeField] public float velocidadeAtaque;
    public bool podeAtacar { get; protected set; } = true;
    protected float tempoUltimoAtaque;

    // Sobrescrevendo o método Start para modificar as variáveis específicas da Espátula
    protected virtual void Start()
    {
        //player.instance.GetComponent<Collider2D>().enabled = true;  //ativar o collider do player
        instance = this;

        // Configurações iniciais padrão que podem ser modificadas nas classes filhas
        colisorDano = GetComponent<Collider2D>();
        colisorDano.enabled = false;

       
            // Acessa os Colliders dos GameObjects
            if (objeto1 != null)
                colisor1 = objeto1.GetComponent<Collider2D>();

            if (objeto2 != null)
                colisor2 = objeto2.GetComponent<Collider2D>();

            if (objeto3 != null)
                colisor3 = objeto3.GetComponent<Collider2D>();

        playerCollider = transform.root.GetComponent<CapsuleCollider2D>();

        // Exemplo: desativar todos os colliders no começo do jogo
        
        DesativarCollidersArma();
        
        

    }

    private void DesativarCollidersArma()
    {
        if (colisor1 != null) colisor1.enabled = false;
        if (colisor2 != null) colisor2.enabled = false;
        if (colisor3 != null) colisor3.enabled = false;
    }

    // Função para ativar um collider específico (por índice)
    public void AtivarCollider1()
    {
        colisor1.enabled = false;
        
        if (player.instance.estaAtacando) 
        { 
            if (colisor1 != null) colisor1.enabled = true; 
        }
        else
        {
            colisor1.enabled = false;
        }
        
    }

    public void AtivarCollider2()
    {
        DesativarCollidersArma();
        if (colisor2 != null) colisor2.enabled = true;
    }

    public void AtivarCollider3()
    {
        DesativarCollidersArma();
        if (colisor3 != null) colisor3.enabled = true;
    }


    // Sobrescrevendo o método de ataque para comportamentos específicos da Espátula
    public IEnumerator Ataque()
    {


        // Ativa o colisor para causar dano
        colisorDano.enabled = true;
        podeAtacar = false;
        // Espera o tempo do ataque (velocidadeAtaque)
        yield return new WaitForSeconds(velocidadeAtaque);

        // Desativa o colisor após o tempo de ataque
        colisorDano.enabled = false;
        podeAtacar = false;
        // Espera o tempo de cooldown (que é o mesmo que a velocidade do ataque)
        yield return new WaitForSeconds(velocidadeAtaque);

        // Após o cooldown, pode atacar novamente
        podeAtacar = true;

    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<inimigo>() != null)
        {
            collision.GetComponent<inimigo>().levaDano(dano);
        }
    }

    
}