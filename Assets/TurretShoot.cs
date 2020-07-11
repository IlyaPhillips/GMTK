using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretShoot : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] int shotDelay;
     private int count = 0;
    ////////////////////////////// FROM ENEMYAI SCRIPT
    public Transform target;
    public float speed = 3f;
    public int cooldownTime = 2000;
    public int maxRange = 2;
    public int minRange = 1;
    
    private Vector3 targetVariance;
    private float either;
    [SerializeField] int hm;
    private void Awake() {
        either = hm;
    targetVariance = new Vector3(Random.Range(target.position.x - 10, target.position.x + 10), Random.Range(target.position.y - 10, target.position.y + 10), 0) * either;
        
    }

    // Start is called before the first frame update
         public void MoveToPlayer ()
         {

             
             count--;
            
             transform.LookAt (target.position + targetVariance);
             transform.Rotate (new Vector3 (0, -90, 0), Space.Self);
         
 
             if (Vector3.Distance (transform.position, target.position) > maxRange) 
             {
                     transform.Translate (new Vector3 (speed * Time.deltaTime, 0, 0));
             }
             else if ((Vector3.Distance (transform.position, target.position) <= maxRange) && (Vector3.Distance (transform.position, target.position) > minRange)){
                 transform.Translate (new Vector3 (speed * Time.deltaTime, 0, 0));
                 Shoot();
                 


           
        }
        else if((Vector3.Distance (transform.position, target.position) <= minRange)){
Shoot();
        }


         }


         public void Shoot(){
             if(count == 0){
                             bullet.GetComponent<Bullet>().vel = (target.position - transform.position).normalized;
            Instantiate(bullet, transform.position, Quaternion.identity);
            count = cooldownTime;
             }
        

         }
void Start(){
    count = 200;
}
    // Update is called once per frame
    void Update()
    {
  
        MoveToPlayer();
        
      
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "PlayerBullet    "){
              Destroy(gameObject);

        }
      
    }
}
////Change Vec3's to Vec 2 and transforms to 2d transforms -- remove Z axis 