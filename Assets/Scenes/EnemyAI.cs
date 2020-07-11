using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class EnemyAI : MonoBehaviour
{
    public Transform target;
     public float speed = 3f;
    public float cooldownTime;

    public float range = 1f;
    // Start is called before the first frame update

    
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       MoveToPlayer();
        
    }

     public void MoveToPlayer ()
         {
 
             transform.LookAt (target.position);
             transform.Rotate (new Vector3 (0, -90, 0), Space.Self);
         
 
             if (Vector3.Distance (transform.position, target.position) > range) 
             {
                     transform.Translate (new Vector3 (speed * Time.deltaTime, 0, 0));
             }
             else{

             }
         }


}
