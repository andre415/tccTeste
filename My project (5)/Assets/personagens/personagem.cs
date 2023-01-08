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
    [Header("Variaveis de movimento")]
    [SerializeField] protected float speed = 1.0f;
    [SerializeField] protected float direction = 1.0f;
    [SerializeField] protected float Vertical = 0;
    [SerializeField] protected float direçãoAtual = 2.0f;
    [SerializeField] protected float dashTime = 0.20f;
    [SerializeField] protected float dashdelay;

    [SerializeField] protected bool facingRight = true;

    [Header("Variaveis de dash")]
    [SerializeField] protected float dashSpeed;
    [SerializeField] protected float dashForce;
    [SerializeField] protected bool isdash = false;

    [SerializeField] protected float dashTimeCount;
    protected float dashdeleyCount;
    [Header("Variaveis de pulo")]

    [SerializeField] protected float jumpforce;
    [SerializeField] protected float jumpTime;// o ideal é ser un decimo do jumpforce
    [SerializeField] protected int isjump;
    [SerializeField] public Transform fiscaldechao;
    [SerializeField] protected float radOCircle;
    [SerializeField] protected LayerMask chao;
    [SerializeField] protected  bool nochao;
    public bool jumpstoped;
    protected  float jumpTimeCount;

    [Header("Variaveis de Jetpack")]
    [SerializeField] protected float jetTime;
    [SerializeField] protected float jetTimeCount;
    [SerializeField] protected float jetUpSpeed;
    [SerializeField] protected float jetFallSpeed;
    [SerializeField] protected float jetForce;
    [SerializeField] protected bool isjet = false;
    [Header("Variaveis de ataque")]
    [SerializeField] protected GameObject bullet;
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected quaternion rtFirePoint;
    


    [Header("Estatos dos personagens")]
    [SerializeField] protected float hp;
    [SerializeField] public float damage;

    protected Rigidbody2D rb;
    
    protected Animator myAnimator;
    #region  monos
    public virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();   
        myAnimator = GetComponent<Animator>();
        
        jumpTimeCount = 0;
        dashTimeCount = dashTime;
        print("a");
        
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

        
        // aqui mantem as mecanicas e fisica 
        
         
    }
    #endregion
    #region mecanicas

    public void upMove(float upSpeed)
    {
        rb.velocity = new Vector2(rb.velocity.x, upSpeed);
    }
    public void Move()
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
        if (jetTimeCount <= jetTime && isjet == false)
        {
            jetForce = jetTimeCount * jetUpSpeed;
            jetTimeCount += Time.deltaTime;

            upMove(jetForce);
            
        }
        else if (!nochao && jetForce > 0)
        {
            isjet = true;
            jetForce = jetTimeCount * jetFallSpeed;
            jetTimeCount -= Time.deltaTime *1.5f;
            upMove(jetForce);
        }
        else if (!nochao)
        {
            isjet = true;
            jetForce = jetTimeCount * jetFallSpeed;
            jetTimeCount -= Time.deltaTime /10;
            upMove(jetForce);
        }else
        {
            
            isjet = false;
            jetForce = 0;
            jetTimeCount = 0;
        }


    } 

    protected void fire()
    {
   
        Instantiate(bullet, firePoint.position, firePoint.rotation);
        

    }

    protected void upFire()
    {
        
        Instantiate(bullet, firePoint.position, firePoint.rotation * Quaternion.Euler(0, 0,45));
        
    }

    protected void DownFire()
    {
        
        Instantiate(bullet, firePoint.position, firePoint.rotation * Quaternion.Euler(0, 0, -45f));

    }





    #endregion

    #region subMecanicas

    protected virtual void HandleDash() { }
    protected virtual void HandFire() { }
    protected virtual void HandleJump() { }
    public virtual void HandleMovement()
    {
        // colocar condicional para move
        Move();
      
    }
    protected void virar( float horizontal)
    {


        if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
        {

            facingRight = !facingRight;
            transform.Rotate(0f, 180f, 0f);
            
        }
        if (horizontal != 0) { direçãoAtual = horizontal; }
     

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



    protected void layers()
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