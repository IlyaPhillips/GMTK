using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretShoot : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] int shotDelay;
    private int count = 0;
    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
        count++;
        if (count == shotDelay) {
            bullet.GetComponent<Bullet>().vel = new Vector2(1, 0);
            Instantiate(bullet, transform.position, Quaternion.identity);
            count = 0;
        }
    }
}
