using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;
}

public class car : MonoBehaviour
{
    public List<AxleInfo> axleInfos;
    public float maxMotorTorque;
    public float maxSteeringAngle;

    [Header("Horses")]
    public Animator horse1;
    public Animator horse2;
    public Animator horse3;
    public Animator horse4;

    //[Header("Sound")]
    //public AudioClip runSound;
    [Header("SoundCube")]
    public GameObject runSound;

    Rigidbody _rigidBody;

    private float saveMotorTorque = 0.0f;
    //AudioSource runningSound;

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        //ReduceSpeed();
        saveMotorTorque = maxMotorTorque;

        //runningSound.clip = runSound;
    }

    // finds the corresponding visual wheel
    // correctly applies the transform
    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
        {
            return;
        }

        Transform visualWheel = collider.transform.GetChild(0);

        Vector3 position;
        Quaternion rotation;

        collider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        //visualWheel.transform.rotation = Quaternion.Euler(0, 0, 90.0f);
        visualWheel.transform.rotation = rotation;
    }

    public void FixedUpdate()
    {
        //Invoke("DelayStart", 3.0f);
        //시작 지연용

        /*
        int Count = 0;
        if(Count == 0)
        {
            Invoke("DelayStart", 3.0f);
            Count++;
        }
        else
        {
            DelayStart();
        }
        */






        float motor = maxMotorTorque * Input.GetAxis("Vertical");
        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");
        float brakeTorque = Mathf.Abs(Input.GetAxis("Jump"));

        if (brakeTorque > 0.001)
        {
            brakeTorque = maxMotorTorque;
            motor = 0;
            runSound.SetActive(false);
        }
        else
        {
            //runSound.SetActive(true);
            brakeTorque = 0;
        }

        if(motor != 0)
        {
            runSound.SetActive(true);
        }


        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }

            axleInfo.leftWheel.brakeTorque = brakeTorque;
            axleInfo.rightWheel.brakeTorque = brakeTorque;


            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }

        

        //horse1.SetFloat("Speed", Mathf.Lerp(motor,2.0f, Time.deltaTime));
        horse1.SetFloat("Speed", _rigidBody.velocity.magnitude * 0.6f);
        horse2.SetFloat("Speed", _rigidBody.velocity.magnitude * 0.6f);
        horse3.SetFloat("Speed", _rigidBody.velocity.magnitude * 0.6f);
        horse4.SetFloat("Speed", _rigidBody.velocity.magnitude * 0.6f);

        //horse2.SetFloat("Speed", motor*0.001f);
        //horse3.SetFloat("Speed", motor * 0.001f);
        //horse4.SetFloat("Speed", motor * 0.001f);

        //Debug.Log(_rigidBody.velocity.magnitude);
    }

    //0524corutine add
    IEnumerator ReduceSpeed(Collider other)
    {
        //
        //saveMotorTorque = maxMotorTorque;
        if (other.gameObject.CompareTag("NPC"))
        {
            maxMotorTorque = maxMotorTorque / 2.0f;
            Debug.Log("win");
            yield return new WaitForSeconds(2.0f);
        }
        Debug.Log("Slow");
        maxMotorTorque = saveMotorTorque;
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(ReduceSpeed(other));
        Debug.Log("Sucsess");
        //if(!other.gameObject.CompareTag("Terrain"))
        //StartCoroutine(ReduceSpeed());
    }

    private void DelayStart()
    {
        float motor = maxMotorTorque * Input.GetAxis("Vertical");
        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");
        float brakeTorque = Mathf.Abs(Input.GetAxis("Jump"));

        if (brakeTorque > 0.001)
        {
            brakeTorque = maxMotorTorque;
            motor = 0;
        }
        else
        {
            brakeTorque = 0;
        }


        


        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }

            axleInfo.leftWheel.brakeTorque = brakeTorque;
            axleInfo.rightWheel.brakeTorque = brakeTorque;


            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }
    }



}