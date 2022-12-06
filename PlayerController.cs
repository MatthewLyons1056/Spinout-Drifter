using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float horizontalInput;
    private float verticalInput;
    private float bounds = 17.5f;

    //new movement update

    public float moveSpeed = 50;
    public float maxSpeed = 15;
    public float drag = 0.98f;
    public float steerAngle = 7.5f;
    public float traction = 1;

    //player physics reference

    private Rigidbody rb;

    //drift variables
    public float driftSpeed = 25;
    public float brakeTraction = 0;
    public float driftSteerAngle = 0;
    private float originalTraction = 0;
    private float originalSpeed = 0;
    private float originalDrag = 0;
    private float originalSteerAngle = 0;

    private Vector3 MoveForce;

    //particles for drift

    public ParticleSystem driftParticle1;
    public ParticleSystem driftParticle2;

    public ParticleSystem chargeParticle;
    public TrailRenderer driftTrailL;
    public TrailRenderer driftTrailR;

    //reference for gamestate controller

    public GameObject gameStateController;

    public int collisionStrength = 1;

    
    private void Start()
    {
        originalTraction = traction;
        originalSpeed = moveSpeed;
        originalDrag = drag;
        originalSteerAngle = steerAngle;

        //physics

        rb = GetComponent<Rigidbody>();

        //gamestate controller reference
        
        gameStateController = GameObject.Find("GameState Manager");
    }

    void Update()
    {

        //drift system

        //change movement during drift reference

        if (Input.GetButtonDown("Jump"))
        {
            driftParticle1.Play(); //could just toss this in a coroutine to avoid the auto checks
            driftParticle2.Play();

            chargeParticle.Play();

            traction = brakeTraction;
            moveSpeed = driftSpeed;
            
            steerAngle = driftSteerAngle;
            drag = .99f;
            
        }
        if (Input.GetButtonUp("Jump"))
        {
            driftParticle1.Stop();
            driftParticle2.Stop();

            //to make the drive feel better, increase the steer speed while drifting
            traction = originalTraction;
            moveSpeed = originalSpeed;
            drag = originalDrag;
            steerAngle = originalSteerAngle;

            chargeParticle.Stop();
            PlayerBoost();
        }

        PlayerBounds();
    }

    private void LateUpdate()
    {
        //PlayerBounds();
        //DriftControls();
    }

    private void FixedUpdate()
    {
        DriftControls();
    }

    void PlayerBounds()
    {
        // create bounds to limit player


        //limit X
        if (transform.position.x < -bounds)
        {
            transform.position = new Vector3(-bounds, transform.position.y, transform.position.z);
        }

        if (transform.position.x > bounds)
        {
            transform.position = new Vector3(bounds, transform.position.y, transform.position.z);
        }

        //limit z

        if (transform.position.z < -10)
        {
            //transform.position = new Vector3(transform.position.x, transform.position.y, -10);

            //gameover spinout!
            
            var gsController = gameStateController.GetComponent<GameState>();
            gsController.GameOver();
            //player loses control of car as well
        }
        if (transform.position.z > 28)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 28);
        }
    }

    void PlayerBoost()
    {
        StartCoroutine(CarBoost());      
        //Debug.Log("boost activated");
    }

    IEnumerator CarBoost()
    {
        //StopCoroutine(CarBoost());
        //increase speed
        //start boost
        moveSpeed = 600f;
        driftTrailL.emitting = true;
        driftTrailR.emitting = true;
        for (int i = 0; i < 6; i++)
        {
            yield return new WaitForSeconds(.5f);
            if (moveSpeed >= 100)
            {
                moveSpeed -= 100;
            }
            if (moveSpeed <= 100)
            {
                //reset speed
                if (!stunCondition)
                {
                    moveSpeed = originalSpeed;
                    break;
                }
            }
        }

        //end boost
        moveSpeed = originalSpeed;

        driftTrailL.emitting = false;
        driftTrailR.emitting = false;
    }

    void DriftControls()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        //PlayerControls();

        //new movement
        if (!stunCondition)
        {
            //moving
            MoveForce += transform.forward * moveSpeed * verticalInput * Time.deltaTime;
            transform.position += MoveForce * Time.deltaTime;

            transform.Rotate(Vector3.up * horizontalInput * MoveForce.magnitude * steerAngle * Time.deltaTime);

            //drag
            MoveForce *= drag;
            MoveForce = Vector3.ClampMagnitude(MoveForce, maxSpeed);

            //traction - the less traction, the more if feels like youre on "ice"
            Debug.DrawRay(transform.position, MoveForce.normalized * 3);
            Debug.DrawRay(transform.position, transform.forward * 3, Color.black);
            MoveForce = Vector3.Lerp(MoveForce.normalized, transform.forward, traction * Time.deltaTime) * MoveForce.magnitude;
        }

        
    }
    public bool stunCondition = false;

   public IEnumerator stunned(float duration)
   {
        //stun player
        stunCondition = true;
        StopCoroutine(CarBoost());
        //Debug.Log("Stunned");
        //MoveForce *= 0;
        moveSpeed = 0;
        drag = .9f;

        yield return new WaitForSeconds(duration);

        //unstun player
        //Debug.Log("Unstunned");
        moveSpeed = originalSpeed;
        drag = originalDrag;
        stunCondition = false;
   }
}
