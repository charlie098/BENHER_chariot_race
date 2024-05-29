using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class CarEngine2 : MonoBehaviour
{

    public Transform path;
    public float maxSteerAngle = 45.0f;
    public float turnSpeed = 5.0f;

    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public WheelCollider wheelRL;
    public WheelCollider wheelRR;

    public float maxBreakTorque = 150.0f;
    public float maxMotorTorque;
    public float maxSteeringAngle;


    public float currentSpeed;
    public float maxSpeed = 100.0f;
    public Vector3 centerOfMass;
    public bool isBreaking = false;



    public Animator[] animator;
    public Animator horse1;
    public Animator horse2;
    public Animator horse3;
    public Animator horse4;

    Rigidbody _rigidBody;
    private void Start()
    {

        //말이 4마리니까 배열로 초기화해서 올려야됨
        //animator = GetComponents<Animator>();
        
        /*
        horse1 = GetComponent<Animator>();        
        horse2 = GetComponent<Animator>();
        horse3 = GetComponent<Animator>();
        horse4 = GetComponent<Animator>();
        */
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        float motor = maxMotorTorque * Input.GetAxis("Vertical");
        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");
        float brakeTorque = Mathf.Abs(Input.GetAxis("Jump"));

    }


    private void Drive()
    {
        //wheelFL.motorTorque = 10f;
        //wheelFR.motorTorque = 10f;

        //currentSpeed = 2 * Mathf.PI * wheelFL.radius * wheelFL.rpm * 60 / 1000;
        
        //currentSpeed = 2 * Mathf.PI * wheelFL.radius * wheelFL.rpm * 300 / 1000;


        

        if (currentSpeed < maxSpeed && !isBreaking)
        {
            wheelFL.motorTorque = maxMotorTorque;
            wheelFR.motorTorque = maxMotorTorque; 
        }
        else
        {
            wheelFL.motorTorque = 0;
            wheelFR.motorTorque = 0;
        }

        //animator.SetFloat("Speed", currentSpeed * 0.001f);

        //horse1.SetFloat("Speed", _rigidBody.velocity.magnitude);

        horse1.SetFloat("Speed", _rigidBody.velocity.magnitude * 0.325f);
        horse2.SetFloat("Speed", _rigidBody.velocity.magnitude * 0.325f);
        horse3.SetFloat("Speed", _rigidBody.velocity.magnitude * 0.325f);
        horse4.SetFloat("Speed", _rigidBody.velocity.magnitude * 0.325f);
        //Debug.Log("_rigidBody.velocity.magnitude : " + _rigidBody.velocity.magnitude);
        //Debug.Log("Avoid  : " + avoiding + " : " + currentSpeed * 0.001f);
        /*
        horse2.SetFloat("Speed", currentSpeed * 0.001f);
        horse3.SetFloat("Speed", currentSpeed * 0.001f);
        horse4.SetFloat("Speed", currentSpeed * 0.001f);
        */
        //bool b = true;
        //if (!(b == false))
        //{
        //animator.ToDictionary();
        //animator[].GetComponent<Animator>().SetFloat("Speed", currentSpeed * 0.001f);
        //}
        /*

        if(currentSpeed == 0 )
        {
            animator.SetFloat("speed", 0);
        }
        else if(currentSpeed > 0 && currentSpeed < 1000)
        {
            animator.SetFloat("speed", 1); 
        }
        else
        {
            animator.SetFloat("speed", 2);
        }

        */


    }

}
