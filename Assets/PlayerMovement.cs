using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] GameObject fBullet,aBullet,eBullet,wBullet;
    [SerializeField] float dashTime;
    [SerializeField] float dashSpeed;
    [SerializeField] float dashCD;
    [SerializeField] int health;
    Vector2 dir;
    Vector3 mousePos;
    GameObject bullet;
    private float dashing;
    private float dashWait;
    private int ammo;
    enum PlayerState { Moving, Dashing, Shooting,Dead };
    PlayerState playerState;
    enum AmmoType { Fire,Water,Earth,Air}
    AmmoType ammoType;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        speed = 4;
        dashTime = 80;
        dashSpeed = 15;
        dashCD = 2;

        playerState = PlayerState.Moving;
        rb = GetComponent<Rigidbody2D>();
        dashWait = 0;
        ammo = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (health < 0) {
            playerState = PlayerState.Dead;
            rb.velocity = new Vector2(0, 0);
        }
        if (playerState != PlayerState.Dead)
        {
            Targetting();
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
    }

    private void Targetting()
    {
        Vector3 mouseTemp = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 15f);
        mousePos = Camera.main.ScreenToWorldPoint(mouseTemp);
        dir = new Vector3(mousePos.x, mousePos.y, 0) - transform.position;

        dir.Normalize();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.tag == ("EnemyBullet"))
        //{
            if (playerState == PlayerState.Dashing)
            {
                Reload();
            }
            else {
                health--;
            }
        //}
        
    }
    

    private void Reload()
    {
        ammo = UnityEngine.Random.Range(2,5);
        int tempRand = UnityEngine.Random.Range(0, 4);
        
            switch (tempRand)
        {
            case 0:
                ammoType = AmmoType.Fire;
                bullet = fBullet;
                break;
            case 1:
                ammoType = AmmoType.Water;
                bullet = wBullet;
                break;
            case 2:
                ammoType = AmmoType.Air;
                bullet = aBullet;
                break;
            case 3:
                ammoType = AmmoType.Earth;
                bullet = eBullet;
                break;
            default:
                print("Invalid number");
                ammoType = AmmoType.Fire;
                bullet = fBullet;
                break;
        }
        print(ammoType);
    }
    

    private void Shoot() {
        if (Input.GetButtonDown("Fire2")) {
            ammo--;
            bullet.GetComponent<Bullet>().vel = dir.normalized;
            Instantiate(bullet, transform.position,Quaternion.identity);
            if (ammo <= 0) {
                playerState = PlayerState.Moving;
            }
        }
    }
    private void Dash()
    {
        
        
        
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
