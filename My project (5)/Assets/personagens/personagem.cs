using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]

public abstract class personagem : MonoBehaviour
{

    #region Variaveis
    [Header("Variaveis de movimento")]
    [SerializeField] protected float speed = 1.0f;
    [SerializeField] protected float direction = .0f;// indica a direçao horizontal mas so quando ela esta sendo alterada(ou deveria ser alterada)
    [SerializeField] protected float Vertical = 0; // // indica a direçao vertical mas so quando ela esta sendo alterada(ou deveria ser alterada)
    [SerializeField] protected float direçãoAtual = 2.0f;// indica a direçao horizontal.
    [SerializeField] protected bool facingRight = true;

    [Header("Variaveis de dash")]
    [SerializeField] protected float dashSpeed;
    [SerializeField] protected float dashForce;
    [SerializeField] protected bool isdash = false;
    [SerializeField] protected float dashTime = 0.20f;
    [SerializeField] protected float dashdelay;
    [SerializeField] protected float dashTimeCount;
    protected float dashdeleyCount;

    [Header("Variaveis de pulo")]
    [SerializeField] protected float jumpforce;
    [SerializeField] protected float jumpTime;// o ideal é ser un decimo do jumpforce
    [SerializeField] protected int isjump;
    [SerializeField] public Transform fiscaldechao; // é o tranform que define onde é o"pé" do personagem
    [SerializeField] protected float radOCircle; // define o tamanho no gismo que vai ser desenhado
    [SerializeField] protected LayerMask chao; // define onde é o chão
    [SerializeField] protected  bool nochao;// identifica se o personagem esta no chão
    public bool jumpstoped;
    protected  float jumpTimeCount;

    [Header("Variaveis de Jetpack")]
    [SerializeField] protected float jetTime;
    [SerializeField] protected float jetTimeCount;
    [SerializeField] protected float jetUpSpeed;
    [SerializeField] protected float jetFallSpeed;
    [SerializeField] protected float jetForce;
    [SerializeField] protected bool isjet = false;
    [SerializeField] protected bool isjetfall = false;

    [Header("Variaveis de ataque")]
    [SerializeField] protected GameObject bullet;
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected quaternion rtFirePoint;
    [SerializeField] protected float bulletSpeed = 1;
    [SerializeField] protected float bulletDamage = 1;
    GameObject LastBulet;

   [Header("Estatos dos personagens")]
    [SerializeField] protected float hp;
    [SerializeField] public float damage;

    protected Rigidbody2D rb;
    protected Animator myAnimator;
    #endregion

    #region monos  
    public virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();   
        myAnimator = GetComponent<Animator>();
        
        jumpTimeCount = 0;
        dashTimeCount = dashTime;
        jetTimeCount = jetTime;   
    }

    public virtual void Update()
    {
        // aqui mantem os inputs
        nochao = Physics2D.OverlapCircle(fiscaldechao.position, radOCircle, chao);

        if (rb.velocity.y < 0)
        {
            myAnimator.SetBool("fall", true);
        }
    }

    public virtual void FixedUpdate()
    {
        // aqui mantem as mecanicas e fisicas
        // quase tudo que tem chance de crashar o jogo é melhor ficar aqui
    }
    #endregion monos

    #region mecanicas

    public void upMove(float upSpeed) // move o personagem verticalmente (pra cima ou baixo)
    {
        rb.velocity = new Vector2(rb.velocity.x, upSpeed);
    }
    public void Move()// mvove o personagem horizontalmente (pra esquerda ou direita)
    {
            if(isdash == false)
            {
            rb.velocity = new Vector2(direction * speed, rb.velocity.y);
            
            }
    }

    protected void dash() 
    {
        dashSpeed = dashTimeCount * dashForce;
        rb.velocity = new Vector2(direçãoAtual * dashSpeed, rb.velocity.y);
        HandleJump();
    }

    protected void Jetpack() {
        if (jetTimeCount <= jetTime && isjetfall == false)// faz o jetpack acelerar
        {
            isjet = true;
            jetForce = jetTimeCount * jetUpSpeed;
            jetTimeCount += Time.deltaTime;
            upMove(jetForce);
        }

        else if (!nochao && jetForce > 0)// faz o jetpack desacelerar
        {
            isjetfall = true;
            isjet = true;
            jetForce = jetTimeCount * jetFallSpeed;
            jetTimeCount -= Time.deltaTime;
            upMove(jetForce);
        }

        else if (!nochao && isjet == true) // faz o personagem cair devagar
        {
            isjetfall = true;
            isjet = true;
            jetForce = jetTimeCount * jetFallSpeed;
            jetTimeCount -= Time.deltaTime /10;
            upMove(jetForce);
        }

        else if (nochao) jetTimeCount = 0;// reseta o contador do jet pack

    }

    protected void blStatsSet( GameObject lsBullet)
    {
        BulletClass LastBulletClass = lsBullet.GetComponent<BulletClass>();
        LastBulletClass.blspeed = bulletSpeed;
        LastBulletClass.bldamege = bulletDamage;
    }
    protected void fire()
    {
        LastBulet = Instantiate(bullet, firePoint.position, firePoint.rotation);
        blStatsSet(LastBulet); 
    }

    protected void upFire()
    {
        LastBulet = Instantiate(bullet, firePoint.position, firePoint.rotation * Quaternion.Euler(0, 0,45));
        blStatsSet(LastBulet);
    }

    protected void DownFire()
    {
        LastBulet = Instantiate(bullet, firePoint.position, firePoint.rotation * Quaternion.Euler(0, 0, -45f));
        blStatsSet(LastBulet);
    }

    

    #endregion

    #region subMecanicas

    protected virtual void HandleDash() { }
    protected virtual void HandFire() { }
    protected virtual void HandleJump() { }
    public virtual void HandleMovement() { Move(); } 
    
    protected void virar( float horizontal)
    {
        if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
        {
            facingRight = !facingRight;
            transform.Rotate(0f, 180f, 0f);   
        }

        if (horizontal != 0)  direçãoAtual = horizontal; 

    }

    public void TomarDano(float danoRecebido)
    {
        hp -= danoRecebido;
    }

    protected void Morrer()
    { if (hp <= 0)
        {
            Destroy(gameObject);
        }    
    }

    protected void OnDrawGizmos()
    {
        Gizmos.DrawSphere(fiscaldechao.position, radOCircle);
    }

    protected void layers() // define o layer que o personagem esta (serve só pra animação)
    {
        if (!nochao)
        {
            myAnimator.SetLayerWeight(1, 1);
        }
        else
        {
            myAnimator.SetLayerWeight(1, 0);
        }
    }
    
    #endregion 

}