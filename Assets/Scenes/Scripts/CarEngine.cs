using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CarEngine : MonoBehaviour
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
    public Vector3 frontSensorPosition = new Vector3(0.0f, 0.2f, 1.0f);
    //public float frontSensorPosition = 0.5f;
    public float frontSideSensorPosition = 0.4f;
    public float frontSensorAngle = 30.0f;

    private List<Transform> nodes;
    private int currentNode = 0;
    private bool avoiding = false;
    private float targetSteerAngle = 0.0f;

    private void Start()
    {
        GetComponent<Rigidbody>().centerOfMass = centerOfMass;

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


    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Sensors();

        ApplySteer();
        Drive();
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
        float avoidMultiplier = 0.0f;
        avoiding = false;
        //sensorStartPos.z += frontSensorPosition;



        //front right sensor
        sensorStartPos += transform.right * frontSideSensorPosition;
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength))
        {
            if (!hit.collider.CompareTag("Terrain"))
            {
                Debug.DrawLine(sensorStartPos, hit.point);
                avoiding = true;
                avoidMultiplier -= 1.0f;
            }
        }

        //front right angle sensor
        else if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength))
        {
            if (!hit.collider.CompareTag("Terrain"))
            {
                Debug.DrawLine(sensorStartPos, hit.point);
                avoiding = true;
                avoidMultiplier -= 0.5f;
            }
        }

        //front left sensor
        sensorStartPos -= transform.right * frontSideSensorPosition * 2;
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength))
        {
            if (!hit.collider.CompareTag("Terrain"))
            {
                Debug.DrawLine(sensorStartPos, hit.point);
                avoiding = true;
                avoidMultiplier += 1.0f;
            }
        }

        //front left angle sensor
        if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(-frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength))
        {
            if (!hit.collider.CompareTag("Terrain"))
            {
                Debug.DrawLine(sensorStartPos, hit.point);
                avoiding = true;
                avoidMultiplier += 0.5f;
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

        //wheelFL.motorTorque = 10f;
        //wheelFR.motorTorque = 10f;

        currentSpeed = 2 * Mathf.PI * wheelFL.radius * wheelFL.rpm * 60 / 1000;

        if(currentSpeed < maxSpeed && !isBreaking)
        {
            wheelFL.motorTorque = maxMotorTorque;
            wheelFR.motorTorque = maxMotorTorque;
        }
        else
        {
            wheelFL.motorTorque = 0;
            wheelFR.motorTorque = 0;
        }

    }

    private void CheckWaypointDistance()
    {
        if((Vector3.Distance(transform.position, nodes[currentNode].position)) < 3.0f)
        {
            if(currentNode == nodes.Count - 1)
            {
                currentNode = 0;
            }
            else
            {
                currentNode++;
            }
            print(currentNode);
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
}
