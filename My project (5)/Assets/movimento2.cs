using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movimento2 : MonoBehaviour
{

    private Rigidbody2D rb2d;
    private Animator myanimtor;

    public float speed = 2.0f;
    public float horizmove;
    private bool virandoesquerda = true;        
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        myanimtor = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // verifar o lado que o jogador se moveu

        horizmove = Input.GetAxisRaw("Horizontal");
        FixUpdate();
    }
    private void FixUpdate()
    {
        // move the player pro lado selecionado

        rb2d.velocity = new Vector2(horizmove * speed, rb2d.velocity.y);
        Flip(horizmove);
        myanimtor.SetFloat("AniSpeed", Mathf.Abs(horizmove));
    }

     private void Flip(float horiz)
    {
        if (horiz < 0 && 
            virandoesquerda || horiz > 0 && !virandoesquerda)
        {
            virandoesquerda = !virandoesquerda;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }

    }

}
