    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : personagem
{
    [SerializeField] protected float carregarAtaque;

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
        direction = Input.GetAxisRaw("h" + "orizontal");
        Vertical = Input.GetAxisRaw("Vertical");

        HandleMovement();
        HandleJump();
        HandleDash();
        ExecutarJetpack();
        layers();
        ExecutarAtaque();
    }

    public override void HandleMovement()
    {
        base.HandleMovement();
        myAnimator.SetFloat("AniSpeed", direction);
        HandFire();
        virar(direction);
    }

    protected override void HandleJump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (nochao)
            {
                jumpTimeCount = jumpTime;
                myAnimator.ResetTrigger("jump");
                myAnimator.SetBool("fall", false);
            }
            isjump++;
            // verifica se voce esta no chão e se ainda n pulou 2 vezes, caso seja verdade o player pula 
            if (nochao || isjump < 2)
            {
                upMove(jumpforce);
                jumpstoped = false;
                myAnimator.SetTrigger("jump");

                //reseta o contador de pulos  quanto o player chega na chão
                if (nochao) { isjump = 0; }
            }
        }

        // quando vc esta pulando e segura o botão aumenta seu pulo
        if (Input.GetButton("Jump") && !jumpstoped && (jumpTimeCount > 0))
        {

            upMove(jumpforce);
            jumpTimeCount -= Time.deltaTime;
            myAnimator.SetTrigger("jump");
        }

        // quando voce para de pular
        if (Input.GetButtonUp("Jump"))
        {
            jumpTimeCount = 0;
            jumpstoped = true;
            myAnimator.SetBool("fall", true);
            myAnimator.ResetTrigger("jump");
        }

    }

    protected override void HandFire()
    {

        if (Input.GetButtonDown("Fire2"))
        {

            // esses if identificam qual direcinal o jogador esta apertando para definir a direção do tiro
            if (Vertical == 1)
            {
                upFire();

            }
            else if (Vertical == -1)
            {
                DownFire();

            }
            else
            {
                fire();
            }



        }
    }

    protected override void HandleDash()
    {
        if (dashdelay > 0)
        {

            dashdelay -= Time.deltaTime;
        }
        if (dashdelay <= 0)
        {
            if (dashTimeCount > 0)
            {
                if (Input.GetButton("Fire1"))
                {
                    isdash = true;
                    dash();
                }

                else
                {
                    Move();
                }

                if (isdash == true)
                {
                    dashTimeCount -= Time.deltaTime;
                }
            }

            else
            {
                dashTimeCount = dashTime;
                isdash = false;
                Move();
                dashdelay++;
            }
        }
    }
    protected void ExecutarJetpack()
    {
        if (Input.GetButton("Fire3"))
        {
            Jetpack();
        }



    }
    protected void ExecutarAtaque()
    {
        if (Input.GetButton("A"))
        {
            carregarAtaque += Time.deltaTime;

        }
        else if (!Input.GetButtonUp("A")) carregarAtaque = 0;
        if (Time.time >= attackTime)
        {
            

            if (Input.GetButtonUp("A") && carregarAtaque < 1)
            {
                Attack(05);
                attackTime = Time.time + 2f / attackRate;
            }
            else if (Input.GetButtonUp("A") && carregarAtaque >= 1) Attack(10);
           

           

        }
    }
}
      
        
