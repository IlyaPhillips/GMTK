using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float maxDist;
    public Vector2 vel;
    private Vector2 start;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = vel*speed;
        start = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
        if ((Vector2.Distance(start,transform.position) > maxDist)) {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }
}
