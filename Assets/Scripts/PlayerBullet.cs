using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float maxDist;
    [SerializeField] GameObject burst;
    public Vector2 vel;
    private Vector2 start;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = vel*speed;
        start = transform.position;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        print(vel);
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = vel * speed;
        print(rb.velocity);
        if ((Vector2.Distance(start,transform.position) > maxDist)) {
            Destroy(gameObject);
        }
        if (this.name == "FireBullet(Clone)") {
            transform.localScale = transform.localScale+ new Vector3(0.01f,0.01f);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Terrain")
        {
              Destroy(gameObject);

        }
      
    }
}
