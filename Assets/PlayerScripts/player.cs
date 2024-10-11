using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class player : MonoBehaviour
{
    public static player instance;
    public Rigidbody2D rb;
    CapsuleCollider2D capsuleCollider;


    //variaveis de combate/vida
    //[SerializeField] public int vidaPlayer;





    
    [Header("Variaveis Movimentaçoes e Pulo")]
    [SerializeField] float VelocidadePadrao;
    public float Velocidade;
    public float horizontal;
    public float ForcaPulo = 10;
    public float ForcaPuloFrente = 10;
    [SerializeField] KeyCode Keycodepulo;
    private bool doubleJump;
    public bool taPulando;
    public LayerMask jumpLayerMask;
    bool isFacingRight = true;
    [SerializeField] private Transform checktaNoChao;
    [SerializeField] private LayerMask layerChao;

    [Header("Variaveis Dash")]
    [SerializeField] float dashSpeed = 5f;
    [SerializeField] float dashDuration = 0.1f;
    [SerializeField] float dashCooldown = 1f;
    public bool estaDashando = false;
    public bool podeDashar = true;

    [Header("Variaveis WallHop")]
    public bool isWallSliding;
    public float wallSlidingSpeed;
    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(8f, 16f);
    public float delayDePulo = 1.5f;
    public bool isOnTheWall;

    [Header("Variavel cTynhia")]
    private bool cTynhia = true;
    LayerMask enemylayer;

    [Header("Raycasts e suas variaveis")]
    RaycastHit2D DownHit; // Raycast Pulo
    RaycastHit2D SideHit; // Raycast Paredes
    [SerializeField] float sizeRaycastjumpCaindo = 0.3f;
    [SerializeField] float sizeRaycastjump = 2.4f;
    [SerializeField] float sizeRaycastWall = 2f;
    [Tooltip("checked todas as layers que voce deseja que o player ignore quando for pular, inclua a layer player")]


    #region Var animacoes 
    [Header("Variavel animacoes")]
    public Animator animator;
    public bool estaAndando;
    public bool estaAtacando;
    public bool taNoChao;
    public bool estaCaindo;
    public bool aterrizando;
    public bool podePular;
    public bool podeMover;
    public bool podeAtacar = true;
    private float wallHopDelay;
    private bool canWallJumpAgain;
    public bool isCrouching;
    #endregion

    private void Awake()
    {
        taPulando = false;
        podePular = true;
        podeMover = true;
        estaAtacando = false;
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
        capsuleCollider = GetComponent<CapsuleCollider2D>();

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
        Vector2 direcaoRay = horizontal == 1 ? Vector2.right : Vector2.left;

        // Lança o Raycast na direção que o player tá virado
        SideHit = Physics2D.Raycast(transform.position, direcaoRay, sizeRaycastWall, jumpLayerMask);
        Debug.DrawRay(transform.position, direcaoRay * sizeRaycastWall, Color.green);

        // Raycast pra baixo (pra checar o chão)
        DownHit = Physics2D.Raycast(transform.position, Vector2.down, transform.localScale.y / 3f + sizeRaycastjump, jumpLayerMask);
        //Debug.Log(DownHit.collider.gameObject.name);

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
        //PuloCarregado();
        Ataque();
        IsWalled();
        Crouch();
        WallSlide();
        WallJump();
        Flip();

        if (!isWallJumping)
        {

        }

        

        if (Input.GetKeyDown(KeyCode.LeftShift) && podeDashar && !estaDashando)
        {
            StartCoroutine(StartDashing());
            animator.SetBool("dash", estaDashando);
        }

    }

    void RaycastInteractions(RaycastHit2D Ray)
    {

        if (Ray.collider.gameObject.GetComponent<plataforma>() != null)
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
        rb.velocity = new Vector2(horizontal * dashSpeed, 0f);

        yield return new WaitForSeconds(dashDuration);

        rb.velocity = new Vector2(0, rb.velocity.y); // para o movimento horizontal após o dash
        rb.gravityScale = originalGravity; // volta a gravidade

        estaDashando = false;
        animator.SetBool("dash", estaDashando);
        
        yield return new WaitForSeconds(dashCooldown); // cooldown do dash
        
        podeDashar = true;

    }

    public void Pulo()
    {
        // Se o jogador está no chão e não está pressionando a tecla de pulo, redefine o double jump
        if (IsGrounded())
        {
            doubleJump = true; // Ativa o double jump novamente ao tocar o chão
        }

        // Se a tecla de pulo for pressionada
        if (Input.GetKeyDown(Keycodepulo))
        {
            // Se o jogador está agachado e no chão, executa o pulo carregado
            if (IsGrounded() && isCrouching)
            {
                Vector2 direcaoRay = horizontal == 1 ? Vector2.right : Vector2.left; // Direção do pulo

                rb.velocity = new Vector2(0, 0); // Reseta a velocidade antes de aplicar a força
                rb.AddForce(new Vector2(horizontal, 1) * ForcaPuloFrente, ForceMode2D.Impulse); // Aplica o impulso de pulo

                taPulando = true; // Necessário para o animator
            }
            // Se o jogador não está agachado, realiza o pulo normal ou o double jump
            else if (IsGrounded() || doubleJump)
            {
                taPulando = true; // Necessário para o animator
                rb.velocity = new Vector2(rb.velocity.x, ForcaPulo); // Aplica a força do pulo
            
                if (!IsGrounded()) // Se não está no chão, usa o double jump
                {
                    doubleJump = false; // Desativa o double jump após usá-lo
                    taPulando |= false;
                }
            }
        }

        // reduz a altura se a tecla de pulo n for ate o final
        if (Input.GetKeyUp(Keycodepulo) && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            taPulando = false; // Necessário para o animator
        }
    }


    //ALL WALLJUMP START

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetKeyDown(Keycodepulo) && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                horizontal *= -1;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;

            }


            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    public bool IsWalled()
    {
        if (SideHit.collider != null && DownHit.collider == null)
        {
            isOnTheWall = true;
            ForcaPulo = 200;
            podeAtacar = false;
            return true;
        }
        else
        {
            isOnTheWall = false;
            ForcaPulo = 20;
            podeAtacar = true;

            return false;
        }

    }

    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && transform.position.x != 0f)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private bool IsGrounded()
    {
        // Verifica se está colidindo com o chão usando o OverlapCircle
        bool estaNoChao = Physics2D.OverlapCircle(checktaNoChao.position, 1f, layerChao);

        if (estaNoChao)
        {
            taNoChao = true;
            animator.SetBool("noChao", true); // Ativa a animação de "no chão"
        }
        else
        {
            taNoChao = false;
            animator.SetBool("noChao", false); // Desativa a animação de "no chão"
        }

        return estaNoChao; // Retorna o estado de "no chão" para outros usos
    }


    

    //ALL WALLJUMP END

    void Crouch()
    {
        if (Input.GetKey(KeyCode.S) && taNoChao)
        {
            capsuleCollider.enabled = false;
            isCrouching = true;
            podeMover = false;
        }
        else
        {
            capsuleCollider.enabled = true;
            isCrouching = false;
            podeMover = true;
        }
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || isFacingRight && horizontal > 1f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    public void MovePlayer()
    {
        if (!estaDashando) //Nao deixa movimento durante dash
        {
            if (podeMover == true && (Input.GetAxisRaw("Horizontal") != 0))
            {
                rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * Velocidade * Time.deltaTime, rb.velocity.y);
                estaAndando = true;
                animator.SetBool("andando", true);
                taNoChao = true;
                horizontal = Input.GetAxisRaw("Horizontal");
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * horizontal, transform.localScale.y, transform.localScale.z);               
            }
            else
            {
                animator.SetBool("andando", false);
                estaAndando = false;
            }


        }

    }

    public void Ataque()
    {
        if (Input.GetMouseButtonDown(0) && podeAtacar == true)
        {
            podeMover = false; // Impede o movimento enquanto ataca
            estaAtacando = true;
            podeAtacar = false;

            // Projeta o player para frente durante o ataque
            //float ataqueProjecao = 15f; // Ajusta esse valor para o quanto você quer que ele se mova
            //rb.velocity = new Vector2(horizontal * ataqueProjecao, rb.velocity.y);

            // Inicia a animação de ataque
            StartCoroutine(Espatula.instance.Ataque());

            // Espera um tempo para permitir que o ataque termine antes de poder se mover de novo
            StartCoroutine(ResetarMovimentoAposAtaque());
        }
    }
    IEnumerator ResetarMovimentoAposAtaque()
    {
        yield return new WaitForSeconds(0.5f); // Define o tempo do ataque antes de permitir o movimento de novo
        podeMover = true;
        estaAtacando = false;
        podeAtacar = true;
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


