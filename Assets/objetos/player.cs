using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class player : MonoBehaviour
{
    public static player instance;
    public Rigidbody2D rb { get; private set; }

    //Variaveis Movimentaçoes e Pulo
    [SerializeField] float VelocidadePadrao;
    public float Velocidade;
    public float ForcaPulo = 10;
    public float ForcaPuloFrente = 10;
    public int puloduplo = 0;
    public int MaxPayne = 2;
    int UltimaDirecao = 1;
    bool estaPulando;
    [SerializeField] KeyCode Keycodepulo;
    CapsuleCollider2D capsuleCollider;


    [SerializeField] Modificadores modificadores;
   

    public AmuletoVida AmuletoEquipado;

    public ArmaPau ArmaEquipada;

    // Variáveis do dash
    [SerializeField] float dashSpeed = 5f;
    [SerializeField] float dashDuration = 0.1f;
    [SerializeField] float dashCooldown = 1f;
    [SerializeField] public int vidaPlayer = 10;
    [SerializeField] float wallHopForce = 10000f;  // Força do wall hop
    [SerializeField] float wallHopTime = 1f;  // Tempo para permitir o wall hop
    public bool estaDashando = false;
    private bool podeDashar = true;

    // Variaveis Wallhop
    float wallHopAirTime = 1f;
    bool isTouchingWall;
    bool canWallHop;
    public bool isWallHopping;
    bool podePular;
    public float delayDePulo = 1.5f;
    

    // Variavel cTynhia
    public bool cTynhia = false;
   
    
    LayerMask enemylayer;
    
    //Raycasts
    RaycastHit2D DownHit; // Raycast Pulo
    RaycastHit2D SideHit; // Raycast Paredes
    [SerializeField] float sizeRaycastjumpCaindo = 0.3f;
    [SerializeField] float sizeRaycastjump = 2.4f;
    [SerializeField] float sizeRaycastWall = 2f;

    [Tooltip("checked todas as layers que voce deseja que o player ignore quando for pular, inclua a layer player")]
    public LayerMask jumpLayerMask;
    [Tooltip("define o tamanho do raycast de pulo")]
   
    #region Var animacoes 
    public Animator animator;
    bool estaAndando;
    public bool estaAtacando;
    bool estaNochao;
    bool estaCaindo;
    bool aterrizando;
    public bool podeMover;
    public bool podeAtacar = true;
    private float wallHopDelay;
    private bool canWallJumpAgain;
    #endregion

    private void Awake()
    {
        podePular = true;
        podeMover = true;
        estaAtacando = false;
        vidaPlayer = modificadores.vidaPlayerMax;
        puloduplo = 0;
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        rb = GetComponent<Rigidbody2D>();
        jumpLayerMask = ~jumpLayerMask;
        
    }

    void FixedUpdate()
    {
        MovePlayer();
        RaycastInteractions(DownHit);
        
    }

    void Start()
    {
        
        Velocidade = VelocidadePadrao;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {

        // Direção do Raycast muda de acordo com a direção do player (esquerda ou direita)
        Vector2 direcaoRay = UltimaDirecao == 1 ? Vector2.right : Vector2.left;

        // Lança o Raycast na direção que o player tá virado
        SideHit = Physics2D.Raycast(transform.position, direcaoRay, sizeRaycastWall, jumpLayerMask);
        Debug.DrawRay(transform.position, direcaoRay * sizeRaycastWall, Color.green);

        // Raycast pra baixo (pra checar o chão)
        DownHit = Physics2D.Raycast(transform.position, Vector2.down, transform.localScale.y / 3.5f + sizeRaycastjump, jumpLayerMask);

        if (DownHit.collider != null)
        {
            transform.SetParent(DownHit.collider.transform);
        }
        else
        {
            transform.SetParent(null);
        }

        // Aqui o código de pulo, ataque, dash, etc.
        Pulo();
        Ataque();

        AmuletoEquipado.Efeito();

        if (Input.GetKeyDown(KeyCode.LeftShift) && podeDashar && !estaDashando)
        {
            StartCoroutine(StartDashing());
            animator.SetBool("dash", estaDashando);
        }

        // Atualiza capacidade de wall hop
        if (SideHit.collider != null && DownHit.collider == null)
        {
            isTouchingWall = SideHit;

            canWallHop = SideHit && !estaNochao && rb.velocity.y != 0 && isTouchingWall;

            if (canWallHop)
            {
                StartCoroutine(WallHop());
            }
        }
        else
        {
            canWallHop = false;
            podeMover = true;
        }
    }
    
    void RaycastInteractions(RaycastHit2D Ray)
    {

        if(Ray.collider.gameObject.GetComponent<plataforma>() != null)
        {
            Ray.collider.gameObject.GetComponent<plataforma>().PlayerEmCima();
        }
    }

    IEnumerator StartDashing()
    {
        estaDashando = true;
        podeDashar = false;

     float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f; // desativa a gravidade durante o dash
        rb.velocity = new Vector2(UltimaDirecao * dashSpeed, 0f); 

        yield return new WaitForSeconds(dashDuration);

        rb.velocity = new Vector2(0, rb.velocity.y); // para o movimento horizontal após o dash
        rb.gravityScale = originalGravity; // volta a gravidade
        
       
        estaDashando = false;
        animator.SetBool("dash", estaDashando);
        yield return new WaitForSeconds(dashCooldown); // cooldown do dash
        podeDashar = true;


       


    }

    IEnumerator DelayDePulo()
    {
        yield return new WaitForSeconds(delayDePulo);
    }
    bool Pulo()
    {
        if (Input.GetKeyDown(Keycodepulo) && podePular)
        {
            Vector2 direcaoRay = UltimaDirecao == 1 ? Vector2.right : Vector2.left;
            puloduplo++;
           
            StartCoroutine(DelayDePulo());
          
            if (DownHit.collider != null || puloduplo < MaxPayne)
            {

                if(UltimaDirecao == 1)
                {
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                    rb.AddForce(new Vector2(1, 1) * ForcaPulo);
                    rb.AddForce(direcaoRay * ForcaPulo);
                }
                else
                {
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                    rb.AddForce(new Vector2(-1, 1) * ForcaPulo);
                    rb.AddForce(direcaoRay * ForcaPulo);
                }

                estaPulando = true;
                estaCaindo = false;
                if (puloduplo == 2)
                {
                    DelayDePulo();
                }

            }
        }

        animator.SetBool("aterrizando", aterrizando);
        aterrizando = false;
        animator.SetBool("caindo", estaCaindo);
        estaCaindo = false;
        animator.SetBool("estaPulando", estaPulando);
       
        if (DownHit.collider != null)
        {
            Debug.DrawRay(transform.position, Vector2.down * (transform.localScale.y / 3f + sizeRaycastjump), Color.red);
             

            estaPulando = false;
            estaNochao = true;
            aterrizando = true;
            MaxPayne = 2;
            animator.SetBool("noChao", estaNochao);
            puloduplo = 0;
            return false;

        }
        else
        {          
            estaPulando = true;
            Debug.DrawRay(transform.position, Vector2.down * (transform.localScale.y / sizeRaycastjumpCaindo), Color.blue);
            estaCaindo = true;
            estaNochao = false;
            return true;
           
        }
    }

    IEnumerator WallHop()
    {
        // Se já está wallhoping, sai
        if (isWallHopping) yield break;

        // Apenas pode pular se está tocando a parede
        if (canWallHop && isTouchingWall)
        {


            isWallHopping = true; // Marca que tá wallhoping
            rb.velocity = new Vector2(rb.velocity.x, 0); // Reseta a velocidade vertical

            // Adiciona a força do pulo
            int PressingJump = Input.GetKey(Keycodepulo) ? 1 : 0;
            rb.AddForce(new Vector2(-UltimaDirecao, 1) * PressingJump * ForcaPulo);

            // Espera um tempo para permitir que o jogador suba
            yield return new WaitForSeconds(wallHopDelay);

            // Verifica se ainda está tocando a parede
           

            // Verifica se o jogador não está mais tocando a parede
            if (SideHit == false && DownHit == false)
            {
                // Espelha o player apenas após pular
                float direction = UltimaDirecao == 1 ? -1 : 1; // Inverte a direção para o raycast
                isTouchingWall = Physics2D.Raycast(transform.position, direction == 1 ? Vector2.right : Vector2.left, sizeRaycastWall, jumpLayerMask);
                UltimaDirecao *= -1;
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * UltimaDirecao, transform.localScale.y, transform.localScale.z);
            }

            yield return new WaitForSeconds(wallHopDelay);

            // Se não estiver tocando a parede, permite movimento
            if (!isTouchingWall)
            {
                podeMover = true; // Permite movimento normal
            }

            canWallJumpAgain = true; // Reseta a flag para permitir novo wall hop
        }

        isWallHopping = false; // Reseta a flag após wallhop
    }









    void MovePlayer()
    {
        if (!estaDashando) //Nao deixa movimento durante dash
        {
            if (podeMover == true && (Input.GetAxisRaw("Horizontal") != 0))
            {
                rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * Velocidade * Time.deltaTime, rb.velocity.y);
                estaAndando = true;
                UltimaDirecao = (int)Input.GetAxisRaw("Horizontal");
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * UltimaDirecao, transform.localScale.y, transform.localScale.z);
                animator.SetBool("andando", estaAndando);
            }
            else
            {
                animator.SetBool("andando", estaAndando);
                estaAndando = false;
            }


        }
       
    }

    public void Ataque()
    {

        if (Input.GetMouseButtonDown(0) && podeAtacar == true)
        {
            estaAtacando = true;
            podeAtacar = false;
            StartCoroutine(Espatula.instance.Ataque());
        }

    }

    public virtual void levaDano(int dano)
    {
        vidaPlayer -= dano;
    }

}

[Serializable]
public class Modificadores
{

    public int vidaPlayerMax = 10;
    public int dano = 1;
    public float atackSpeed = 0.5f;
    public float VelocidadePadrao = 650;
    public int VelocidadeBonus;
    
}
