﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] GameObject fBullet,aBullet,eBullet,wBullet;
    public float speed;
    [SerializeField] float dashTime;
    [SerializeField] float dashSpeed;
    [SerializeField] int dashCD;
    [SerializeField] int health;
    Vector2 dir;
    Vector3 mousePos;
    GameObject bullet;
    private float dashing;
    private int dashWait;
    private bool canDash;
    private int ammo;
    enum PlayerState { Moving, Dashing, Shooting,Dead };
    PlayerState playerState;
    enum AmmoType { Fire,Water,Earth,Air}
    AmmoType ammoType;
    Rigidbody2D rb;
    [SerializeField] AudioClip dodgeOnCD;
    [SerializeField] AudioClip catchBullet;
    [SerializeField] AudioClip eShot, fShot, wShot, aShot;
    [SerializeField] AudioClip hitFX;
    [SerializeField] AudioClip dashFX;
    AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        speed = 4;
        dashTime = 40;
        dashSpeed = 15;
        dashCD = 160;

        playerState = PlayerState.Moving;
        rb = GetComponent<Rigidbody2D>();
        dashWait = 0;
        ammo = 0;
        audio = GetComponent<AudioSource>();
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
            Reload();
        else
            audio.PlayOneShot(hitFX);
            health--;
     }

    private void Reload()
    {
        audio.PlayOneShot(catchBullet);
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
      }

       

    

    private void Shoot()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            ammo--;
            
            switch (ammoType) {
                case AmmoType.Air:
                    audio.PlayOneShot(aShot);
                    bullet.GetComponent<PlayerBullet>().vel = dir.normalized;
                    Instantiate(bullet, transform.position, Quaternion.identity);
                    bullet.GetComponent<PlayerBullet>().vel = dir.normalized;
                    Instantiate(bullet, transform.position + new Vector3(dir.x, dir.y)/2, Quaternion.identity);
                    bullet.GetComponent<PlayerBullet>().vel = dir.normalized;
                    Instantiate(bullet, transform.position+new Vector3(dir.x,dir.y), Quaternion.identity);
                    break;
                case AmmoType.Earth:
                    audio.PlayOneShot(eShot);
                    Vector2 temp = new Vector2(Random.Range(-0.05f,0.05f), Random.Range(-0.05f, 0.05f));
                    bullet.GetComponent<PlayerBullet>().vel = (dir+temp).normalized;
                    Instantiate(bullet, transform.position, Quaternion.identity);
                    bullet.GetComponent<PlayerBullet>().vel = dir.normalized;
                    Instantiate(bullet, transform.position, Quaternion.identity);
                    bullet.GetComponent<PlayerBullet>().vel = (dir+2*temp).normalized;
                    Instantiate(bullet, transform.position, Quaternion.identity);
                    break;
                case AmmoType.Fire:
                    audio.PlayOneShot(fShot);
                    bullet.GetComponent<PlayerBullet>().vel = dir.normalized;
                    Instantiate(bullet, transform.position, Quaternion.identity);
                    break;
                case AmmoType.Water:
                    audio.PlayOneShot(wShot);
                    bullet.GetComponent<PlayerBullet>().vel = dir.normalized;
                    Instantiate(bullet, transform.position, Quaternion.identity);
                    break;
                default:
                    bullet.GetComponent<PlayerBullet>().vel = dir.normalized;
                    Instantiate(bullet, transform.position, Quaternion.identity);
                    break;

            }
            
            if (ammo <= 0)
            {
                playerState = PlayerState.Moving;
            }
        }
    }

    private void Dash()
    {
        if (!canDash) {
            dashWait++;
            if (dashWait == dashCD) {
                canDash = true;
                
                dashWait = 0;
            }
        }
        if (Input.GetButtonDown("Fire1") && playerState == PlayerState.Moving && canDash)
        {
            audio.PlayOneShot(dashFX);
            playerState = PlayerState.Dashing;
            dashing = dashTime;
            canDash = false;
        }
        else if (Input.GetButtonDown("Fire1")) {
            audio.PlayOneShot(dodgeOnCD);
        }

        if (dashing > 0)
        {
            
            rb.velocity = dir * dashSpeed;
            dashing--;
        }
        else
        {
            if (ammo > 0)
            {
                playerState = PlayerState.Shooting;
            }
            else
            {
                playerState = PlayerState.Moving;
                
            }
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
