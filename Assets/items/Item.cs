using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    private CircleCollider2D cc;
    private Rigidbody2D rb;
    [SerializeField] string type;

    void Start()
    {
        Debug.Log("powerup");

        cc = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    public abstract IEnumerator usePowerUp(PlayerMovement pm);

}
