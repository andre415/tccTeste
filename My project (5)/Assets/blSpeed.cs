using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class blSpeed : MonoBehaviour
{
    protected Rigidbody2D rb;
    [SerializeField] public float blspeed = 20.0f;

    [SerializeField] public float bldamege = 20.0f;
    // Start is called before the first frame update
    void Start()
    {
       print("affsr");
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * blspeed;  
    }
    private void OnTriggerEnter2D(Collider2D hitInfo)
    {

        personagem alvo = hitInfo.GetComponentInChildren<personagem>();

        if (alvo != null) {
            alvo.TomarDano(1);

        }

        Debug.Log(hitInfo.name);
        Destroy(gameObject);
    }

    // Update is called once per frame

}
*/