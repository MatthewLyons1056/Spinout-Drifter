using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyClass : MonoBehaviour
{
    //This scrip controls enemy properties

    //Enemy movement var
    public float speed;
    public int collisionStrength; //value in which determins whether or not a car loses in a collision
    
    private BoxCollider bCollider;
    public GameObject explosionParticle;

    public bool warning;

    //public PlayerHealth playerHealth;
   
    // Start is called before the first frame update
    void Start()
    {
        if(warning == true)
        {
            StartCoroutine(EnemyWarning());
        }


        //playerHealth = GameObject.Find("Player").GetComponent<PlayerHealth>(); //Finished and finalized reference
    }
    private void Awake()
    {
        
        bCollider = GetComponent<BoxCollider>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            //explosionPFX.GetComponentInChildren<ParticleSystem>().Play();
            var currentPosition = this.transform.position;
            Instantiate(explosionParticle, currentPosition, transform.rotation);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            DestroySelf();
        }

        EnemySpeed();
        EnemyBounds();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))//track collision to player
        {
            
            var playerCollisionStrength = other.GetComponentInParent<PlayerController>().collisionStrength;
            //car blows up
            if(playerCollisionStrength >= collisionStrength)
            {
                DestroySelf();
            }
           
            //reference other enemy script
            //check to see if it has a collisionStrength value lesser or greater
        }
        if (other.gameObject.CompareTag("Enemy")) //controls interaction of enemy vs enemy 
        {

            var script = other.GetComponent<EnemyClass>();
            if(script.collisionStrength >= collisionStrength) //compare collision strength
            {
                DestroySelf();
            }
            Debug.Log("enemy Collision detected from " + other.gameObject.name);
        }
    }

    public bool complexGFX; //complexGFX = police cars that have multiple GFX
    void DestroySelf()
    {
        Debug.Log("destroying self = " + this.gameObject.name);
        BoxCollider bCol = GetComponent<BoxCollider>();// turn of box collider
        bCol.enabled = false;
        if (complexGFX == false)
        {
            MeshRenderer graphics = GetComponentInChildren<MeshRenderer>();//turn off graphics
            graphics.enabled = false;
        }
        if(complexGFX == true)
        {
            foreach(Transform child in transform)
            {
                Destroy(gameObject);
            }

        }

       


        //expload 
        var currentPosition = this.transform.position;
        Instantiate(explosionParticle, currentPosition, transform.rotation);
       
        Destroy(this.gameObject, 1.5f); //destroys the gameobject after time
    }

    void EnemySpeed()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void EnemyBounds()
    {
        //Will delete enemy's when outside of camera range

        //establish bounds
        int xBounds = 18;
        int zBounds = 50;
        int yBounds = 10;

        if (transform.position.x > xBounds)
        {
            //destroy car
            Destroy(gameObject);
        }
        if (transform.position.x < -xBounds)
        {
            Destroy(gameObject);
        }
        if (transform.position.y > yBounds)
        {
            Destroy(gameObject);
        }
        if (transform.position.y < -yBounds)
        {
            Destroy(gameObject);
        }
        if (transform.position.z > zBounds)
        {
            Destroy(gameObject);
        }
        if (transform.position.z < -zBounds)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator EnemyWarning()
    {
        var referenceArrowFlicker = GetComponentInChildren<ArrowFlicker>();
        StartCoroutine(referenceArrowFlicker.FlickerImage());
        //display notification for police car
        //display sounds FX for police car
        Debug.Log("starting warning now");
        var originalSpeed = speed;
        speed = 0;
        var waitTime = 1f;
        // suspend execution for waitime seconds
        yield return new WaitForSeconds(waitTime); //time till dash and inform player
        speed = originalSpeed;
        StopCoroutine(referenceArrowFlicker.FlickerImage());
        referenceArrowFlicker.charging = false;
    }

}
