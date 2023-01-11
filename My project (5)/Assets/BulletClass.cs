using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletClass : MonoBehaviour
{
    protected Rigidbody2D rb;
    [SerializeField] public float bldirection = 1;
    [SerializeField] public float blspeed = 20.0f;
    [SerializeField] public float bldamege = 20.0f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        disparo();   
    }

    private void Update()
    {
    }
    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        BulletClass bullet =  hitInfo.GetComponentInChildren<BulletClass>();

        personagem alvo = hitInfo.GetComponentInChildren<personagem>();
  
        if (alvo != null)
        {
            alvo.TomarDano(1);

            Destroy(gameObject);
        }
        else if(bullet == null) Destroy(gameObject);
    }

     public void disparo()
    {
        rb.velocity = transform.right * blspeed;
    }
    
}
