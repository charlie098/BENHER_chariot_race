using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class CarEngine1 : MonoBehaviour
{

    public Transform path;
    public float maxSteerAngle = 45.0f;
    public float turnSpeed = 5.0f;

    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public WheelCollider wheelRL;
    public WheelCollider wheelRR;

    public float maxMotorTorque = 80.0f;
    public float maxBreakTorque = 150.0f;

    public float currentSpeed;
    public float maxSpeed = 100.0f;
    public Vector3 centerOfMass;
    public bool isBreaking = false;

    [Header("Sensors")]
    public float sensorLength = 4.0f;
    public float sideSensorLength = 5.0f;
    public Vector3 frontSensorPosition = new Vector3(0.0f, 0.2f, 1.0f);

    public Vector3 rightSensorPosition = new Vector3(0.0f, 0.2f, 3.0f);
    public Vector3 leftSensorPosition = new Vector3(0.0f, 0.2f, 3.0f);


    //public float frontSensorPosition = 0.5f;
    public float frontSideSensorPosition = 0.4f;
    public float frontSensorAngle = 30.0f;
    //public Vector3 rightSideSensorPos = new Vector3(5.0f, 0.2f, 0.0f);
    //public Vector3 leftSideSensorPos = new Vector3(-5.0f, 0.2f, 0.0f);

    private List<Transform> nodes;
    private int currentNode = 0;
    private bool avoiding = false;
    private bool backward = false;
    private float targetSteerAngle = 0.0f;

    private bool colide = false;

    public Animator[] animator;
    public Animator horse1;
    public Animator horse2;
    public Animator horse3;
    public Animator horse4;

    Rigidbody _rigidBody;
    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _rigidBody.centerOfMass = centerOfMass;

        Transform[] pathTrnsforms = path.GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();

        for (int i = 0; i < pathTrnsforms.Length; i++)
        {
            if (pathTrnsforms[i] != path.transform)
            {
                nodes.Add(pathTrnsforms[i]);
                //print(pathTrnsforms[i]);
            }
        }

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
        Sensors();

        ApplySteer();
        if(colide == false)
        {
            //Invoke("Drive", 2);
            Drive();
            //backward = false;
        }
        else
        {
            Back();
        }
        CheckWaypointDistance();
        Breaking();
        LerpToSteerAngle();
        //print((Vector3.Distance(transform.position, nodes[currentNode].position)));
        //print(transform.position);
    }

    private void Sensors()
    {
        //this use raycast
        RaycastHit hit;
        Vector3 sensorStartPos = transform.position;
        sensorStartPos += transform.forward * frontSensorPosition.z;
        sensorStartPos += transform.up * frontSensorPosition.y;

        Vector3 rightSensorStartPos = transform.position;
        rightSensorStartPos += transform.right * rightSensorPosition.z;
        rightSensorStartPos += transform.up * rightSensorPosition.y;
        rightSensorStartPos += transform.forward * rightSensorPosition.x;


        Vector3 leftSensorStartPos = transform.position;
        leftSensorStartPos -= transform.right * leftSensorPosition.z;
        leftSensorStartPos += transform.up * leftSensorPosition.y;
        leftSensorStartPos += transform.forward * leftSensorPosition.x;


        /*

        Vector3 rightSideSensor = transform.position;
        rightSideSensor += transform.right * rightSideSensorPos.z;
        rightSideSensor += transform.up * rightSideSensorPos.y;

        Vector3 leftSideSensor = transform.position;
        leftSideSensor += transform.right * leftSideSensorPos.z;
        leftSideSensor += transform.up * leftSideSensorPos.y;

        */

        float avoidMultiplier = 0.0f;
        avoiding = false;
        //sensorStartPos.z += frontSensorPosition;



        //front right sensor
        sensorStartPos += transform.right * frontSideSensorPosition;
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength))
        {
            if (!hit.collider.CompareTag("Terrain"))
            {
                if (backward == true)
                {
                    Debug.DrawLine(sensorStartPos, hit.point);
                    avoiding = true;
                    avoidMultiplier += 1.0f;
                }
                else
                {
                    Debug.DrawLine(sensorStartPos, hit.point);
                    avoiding = true;
                    avoidMultiplier -= 1.0f;
                }
            }
        }

        //front right angle sensor
        else if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength))
        {
            if (!hit.collider.CompareTag("Terrain"))
            {
                if (backward == true)
                {
                    Debug.DrawLine(sensorStartPos, hit.point);
                    avoiding = true;
                    avoidMultiplier += 0.5f;
                }
                else
                {
                    Debug.DrawLine(sensorStartPos, hit.point);
                    avoiding = true;
                    avoidMultiplier -= 0.5f;
                }
            }
        }

        //front left sensor
        sensorStartPos -= transform.right * frontSideSensorPosition * 2;
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength))
        {
            if (!hit.collider.CompareTag("Terrain"))
            {
                if (backward == true)
                {
                    Debug.DrawLine(sensorStartPos, hit.point);
                    avoiding = true;
                    avoidMultiplier -= 1.0f;
                }
                else
                {
                    Debug.DrawLine(sensorStartPos, hit.point);
                    avoiding = true;
                    avoidMultiplier += 1.0f;
                }
            }
        }

        //front left angle sensor
        if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(-frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength))
        {
            if (!hit.collider.CompareTag("Terrain"))
            {
                if (backward == true)
                {
                    Debug.DrawLine(sensorStartPos, hit.point);
                    avoiding = true;
                    avoidMultiplier -= 0.5f;
                }
                else
                {
                    Debug.DrawLine(sensorStartPos, hit.point);
                    avoiding = true;
                    avoidMultiplier += 0.5f;
                }
            }
        }

        //front center sensor
        if (avoidMultiplier == 0)
        {
            if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength))
            {
                if (!hit.collider.CompareTag("Terrain"))
                {
                    Debug.DrawLine(sensorStartPos, hit.point);
                    avoiding = true;
                    if(hit.normal.x < 0)
                    {
                        avoidMultiplier = -1;
                    }
                    else
                    {
                        avoidMultiplier = 1;
                    }
                }
            }
        }


        //right sesnsor
        if(avoidMultiplier == 0)
        {
            if (Physics.Raycast(rightSensorStartPos, transform.right, out hit, sideSensorLength)&& Physics.Raycast(leftSensorStartPos, -transform.right, out hit, sideSensorLength))
            {
                if (!hit.collider.CompareTag("Terrain"))
                {
                    //Debug.DrawLine(rightSensorStartPos, hit.point);
                    avoiding = false;
                    avoidMultiplier = 0;
                }
            }
            else if (Physics.Raycast(rightSensorStartPos, transform.right, out hit, sideSensorLength))
            {
                if(!hit.collider.CompareTag("Terrain"))
                {
                    Debug.DrawLine(rightSensorStartPos, hit.point);
                    avoiding = true;
                    avoidMultiplier = -1;
                }
            }
            else if(Physics.Raycast(leftSensorStartPos, -transform.right, out hit, sideSensorLength))
            {
                if (!hit.collider.CompareTag("Terrain"))
                {
                    Debug.DrawLine(leftSensorStartPos, hit.point);
                    avoiding = true;
                    avoidMultiplier = 1;
                }
            }
        }




        /*

        rightSideSensor += transform.right;
        if (Physics.Raycast(rightSideSensor, transform.forward, out hit, sensorLength))
        {
            if (!hit.collider.CompareTag("Terrain"))
            {
                Debug.DrawLine(rightSideSensor, hit.point);
                avoiding = true;
                avoidMultiplier += 1.0f;
            }
        }

        leftSideSensor -= transform.right;
        if (Physics.Raycast(leftSideSensor, transform.forward, out hit, sensorLength))
        {
            if (!hit.collider.CompareTag("Terrain"))
            {
                Debug.DrawLine(leftSideSensor, hit.point);
                avoiding = true;
                avoidMultiplier += 1.0f;
            }
        }

        */

        if (avoiding)
        {
            targetSteerAngle = maxSteerAngle * avoidMultiplier;
            //wheelFL.steerAngle = maxSteerAngle * avoidMultiplier;
            //wheelFR.steerAngle = maxSteerAngle * avoidMultiplier;
        }

        //print(hit.point);

    }

    private void ApplySteer()
    {
        if (avoiding) return;
        Vector3 relativeVector = transform.InverseTransformPoint(nodes[currentNode].position);
        //print(relativeVector);
        //relativeVector /= relativeVector.magnitude;

        float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteerAngle;
        targetSteerAngle = newSteer;
        //wheelFL.steerAngle = newSteer;
        //wheelFR.steerAngle = newSteer;
    }

    private void Drive()
    {
        backward = false;
        //wheelFL.motorTorque = 10f;
        //wheelFR.motorTorque = 10f;

        //currentSpeed = 2 * Mathf.PI * wheelFL.radius * wheelFL.rpm * 60 / 1000;

        //currentSpeed = 2 * Mathf.PI * wheelFL.radius * wheelFL.rpm * 300 / 1000;

        
        //0524fixed
        if (avoiding)
        {
            currentSpeed = 2 * Mathf.PI * wheelFL.radius * wheelFL.rpm * 300 / 1000;            
        }
        else
        {
            currentSpeed = 2 * Mathf.PI * wheelFL.radius * wheelFL.rpm;
        }

        
        //0524current
        //currentSpeed = 2 * Mathf.PI * wheelFL.radius * wheelFL.rpm;


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

    private void CheckWaypointDistance()
    {
        if((Vector3.Distance(transform.position, nodes[currentNode].position)) < 15.0f)
        {
            if(currentNode == nodes.Count - 1)
            {
                currentNode = 0;
            }
            else
            {
                currentNode++;
            }
            //print(currentNode);
            Debug.Log(currentNode);
        }
    }

    private void Breaking()
    {
        if(isBreaking)
        {
            wheelRL.brakeTorque = maxBreakTorque;
            wheelRR.brakeTorque = maxBreakTorque;
        }
        else
        {
            wheelRL.brakeTorque = 0;
            wheelRR.brakeTorque = 0;
        }
    }

    private void LerpToSteerAngle()
    {
        wheelFL.steerAngle = Mathf.Lerp(wheelFL.steerAngle, targetSteerAngle, Time.deltaTime * turnSpeed);
        wheelFR.steerAngle = Mathf.Lerp(wheelFR.steerAngle, targetSteerAngle, Time.deltaTime * turnSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        //currentSpeed = 2 * Mathf.PI * wheelFL.radius * -wheelFL.rpm;
        colide = true;
        Debug.Log("success");
        //print(maxMotorTorque);
    }

    private void Back()
    {
        wheelFL.motorTorque = -maxMotorTorque;
        wheelFR.motorTorque = -maxMotorTorque;
        backward = true;
    }

    private void OnTriggerExit(Collider other)
    {
        colide = false;
        Debug.Log("false");
    }

}
