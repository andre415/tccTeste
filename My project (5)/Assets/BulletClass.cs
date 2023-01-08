using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletClass : MonoBehaviour
{
    protected Rigidbody2D rb;
    [SerializeField] public float bldirection = 1;
    [SerializeField] public float blspeed = 20.0f;
    [SerializeField] private float bldamege = 20.0f;
    // Start is called before the first frame update
    void Start()
    {
        print("affsr");
        rb = GetComponent<Rigidbody2D>();
        BlDefrente();

        
        
        
    }

    private void Update()
    {
    }
    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        BulletClass bullet =  hitInfo.GetComponentInChildren<BulletClass>();
        personagem alvo = hitInfo.GetComponentInChildren<personagem>();
        Debug.Log(hitInfo.name + "a,sla,s");
        if (alvo != null)
        {
            alvo.TomarDano(1);
            Destroy(gameObject);
        }
        else if(bullet == null)
        {
            Destroy(gameObject);
        }


        
        
    }

     public void BlDefrente()
    {

        rb.velocity = transform.right * blspeed;
    }

      public void BlDeCima()
    {
        rb.velocity = transform.right * blspeed;
        rb.velocity = new Vector2(rb.velocity.x, blspeed);
        
    }

    // Update is called once per frame

}
