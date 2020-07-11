using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    [SerializeField] GameObject bullet;
    [SerializeField] float dashTime;
    [SerializeField] float dashSpeed;
    [SerializeField] float dashCD;
    [SerializeField] int health;
    Vector2 dir;
    Vector3 mousePos;
    private float dashing;
    private float dashWait;
    private int ammo;
    enum PlayerState { Moving, Dashing, Shooting };
    PlayerState playerState;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        playerState = PlayerState.Moving;
        rb = GetComponent<Rigidbody2D>();
        dashWait = 0;
        ammo = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Dash();
        if (playerState == PlayerState.Moving)
        {
            Move();
        }

        if (playerState == PlayerState.Shooting)
        {
            Move();
            Shoot();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject go = collision.gameObject;

        switch (collision.tag)
        {
            case "PowerUp":
                StartCoroutine(go.GetComponent<PowerUp>().usePowerUp(this));
                Destroy(go);
                break;
            case "EnemyBullet":
                bulletCollision();
                break;
        }

    }

    private void bulletCollision()
    {
        if (playerState == PlayerState.Dashing)
            ammo = UnityEngine.Random.Range(2, 5);
        else
            health--;
    }

    private void Shoot()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            ammo--;
            bullet.GetComponent<Bullet>().vel = dir.normalized;
            Instantiate(bullet, transform.position,Quaternion.identity);
            if (ammo <= 0)
            {
                playerState = PlayerState.Moving;
            }
        }
    }

    private void Dash()
    {
        
        Vector3 mouseTemp = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 15f);
        mousePos = Camera.main.ScreenToWorldPoint(mouseTemp);
        dir = new Vector3(mousePos.x, mousePos.y, 0) - transform.position;

        dir.Normalize();
        
        if (Input.GetButtonDown("Fire1") && playerState == PlayerState.Moving)// && dashWait == 0)
        {
            playerState = PlayerState.Dashing;
            dashing = dashTime;
            
        }

        if (dashing > 0)
        {
            
            rb.velocity = dir * dashSpeed;
            dashing--;
        }
        else
        {
            dashWait = dashCD;
            if (ammo > 0)
            {
                playerState = PlayerState.Shooting;
            }
            else
            {
                playerState = PlayerState.Moving;
            }
        }
        if (dashWait > 0) {
            dashWait--;
        }
    }

    private void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector2 vel = new Vector2(x, y);
        rb.velocity = vel.normalized * speed;
    }
}
