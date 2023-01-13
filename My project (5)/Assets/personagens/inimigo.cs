using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class inimigo : personagem
{
    player pl ;
    [SerializeField] protected float i;
    [SerializeField] protected float j;
    

    // Start is called before the first frame update
    void Start()
    {
         pl = FindObjectOfType<player>();
        print(pl.transform.position.x);
        base.Start();

    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > j)
        {
            j = Time.time + 1/i ;
            HandFire();
        }

        Morrer();
    }

   

    protected virtual void HandleDash() {} 
    protected virtual void HandFire() {
        // os if identificam a altura do player e com base nisso decidem a direção do tiro do inimigo
        if (pl.fiscaldechao.position.y > this.transform.position.y)upFire(); 

        else if (pl.fiscaldechao.position.y +0.2F < this.fiscaldechao.position.y )DownFire();

        else fire();
        
    }
    protected virtual void HandleJump() {}

    protected void SeguirPlayer()// faz o inimigo virar pro player
    {
        if (pl.transform.position.x < this.transform.position.x)  virar(-1); 
        else if (pl.transform.position.x > this.transform.position.x) virar(1); 
    }

}
