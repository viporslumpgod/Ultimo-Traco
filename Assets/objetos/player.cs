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
    public Rigidbody2D rb { get; private set; }

    //Variaveis Movimentaçoes e Pulo
    [SerializeField] float VelocidadePadrao;
    public float Velocidade;
    public float ForcaPulo = 10;
    public float ForcaPuloFrente = 10;
    public int puloduplo = 0;
    public int MaxPayne = 2;
    float horizontal;
    bool isFacingRight = true;
    bool estaPulando;
    [SerializeField] KeyCode Keycodepulo;
    CapsuleCollider2D capsuleCollider;

    // Variáveis do dash
    [SerializeField] float dashSpeed = 5f;
    [SerializeField] float dashDuration = 0.1f;
    [SerializeField] float dashCooldown = 1f;
    [SerializeField] public int vidaPlayer = 10;
    public bool estaDashando = false;
    private bool podeDashar = true;

    // Variaveis WallJump
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
   
    // Variavel cTynhia
    private bool cTynhia = true;


    LayerMask enemylayer;

    //Raycasts e variaveis
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
    public bool estaAndando;
    public bool estaAtacando;
    bool taNoChao;
    bool estaCaindo;
    bool aterrizando;
    private bool podePular;
    public bool podeMover;
    public bool podeAtacar = true;
    private float wallHopDelay;
    private bool canWallJumpAgain;
    public bool isCrouching;
    #endregion

    private void Awake()
    {
        podePular = true;
        podeMover = true;
        estaAtacando = false;
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
        PuloCarregado();
        Ataque();
        IsWalled();
        crouch();
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

    IEnumerator DelayDePulo()
    {
        yield return new WaitForSeconds(delayDePulo);
    }
    bool Pulo()
    {
        if (Input.GetKeyDown(Keycodepulo) && podePular)
        {
            Vector2 direcaoRay = horizontal == 1 ? Vector2.right : Vector2.left;
            puloduplo++;

            StartCoroutine(DelayDePulo());

            if (DownHit.collider != null || puloduplo < MaxPayne)
            {


                if (horizontal == 1)
                {
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                    rb.AddForce(new Vector2(1, 1) * ForcaPulo);
                   // rb.AddForce(direcaoRay * ForcaPulo);
                }
                else 
                {
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                    rb.AddForce(new Vector2(-1, 1) * ForcaPulo);
                    //rb.AddForce(direcaoRay * ForcaPulo);
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
            taNoChao = true;
            aterrizando = true;
            MaxPayne = 2;
            animator.SetBool("noChao", taNoChao);
            puloduplo = 0;
            return false;

        }
        else
        {
            estaPulando = true;
            Debug.DrawRay(transform.position, Vector2.down * (transform.localScale.y / sizeRaycastjumpCaindo), Color.blue);
            estaCaindo = true;
            taNoChao = false;
            return true;

        }
    }

    
    void PuloCarregado()
    {

        if (Input.GetKeyDown(Keycodepulo) && podePular && taNoChao && isCrouching)
        {
            puloduplo = 2;
            Vector2 direcaoRay = horizontal == 1 ? Vector2.right : Vector2.left;
            if (horizontal == 1)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(new Vector2(1, 1) * ForcaPulo);
                rb.AddForce(direcaoRay * ForcaPulo);

            }
            else if (horizontal == -1)
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


   //ALL WALLJUMP START
    
    
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
    private void WallJump()
    {
        if  (isWallSliding)
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

    bool IsGrounded()
    {
        if (DownHit.collider != null)
        {
            return true;
        }
        else 
        {
            return false;
        }
    }


    public bool IsWalled()
    {
        if (SideHit.collider != null && DownHit.collider == null)
        {
            isOnTheWall = true;
            ForcaPulo = 850;
            podeAtacar = false;
            return true;
        }
        else
        {
            isOnTheWall = false;
            ForcaPulo = 690;
            podeAtacar = false;
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

    //ALL WALLJUMP END

    void crouch()
    {
        if (Input.GetKey(KeyCode.S) && taNoChao)
        {
            capsuleCollider.enabled = false;
            isCrouching = true;
        }
        else
        {
            capsuleCollider.enabled = true;
            isCrouching = false;
        }
    }


    void MovePlayer()
    {
        if (!estaDashando) //Nao deixa movimento durante dash
        {
            if (podeMover == true && (Input.GetAxisRaw("Horizontal") != 0))
            {
                rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * Velocidade * Time.deltaTime, rb.velocity.y);
                
                estaAndando = true;
                horizontal = Input.GetAxisRaw("Horizontal");
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * horizontal, transform.localScale.y, transform.localScale.z);
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
            podeMover = false; // Impede o movimento enquanto ataca
            estaAtacando = true;
            podeAtacar = false;

            // Projeta o player para frente durante o ataque
            float ataqueProjecao = 9f; // Ajusta esse valor para o quanto você quer que ele se mova
            rb.velocity = new Vector2(horizontal * ataqueProjecao, rb.velocity.y);

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
