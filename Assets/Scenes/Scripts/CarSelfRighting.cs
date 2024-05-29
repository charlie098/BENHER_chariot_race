using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSelfRighting : MonoBehaviour
{
    [SerializeField] private float m_WaitTime = 3.0f;
    [SerializeField] private float m_VelocityThreshold = 1.0f;

    private float m_LastOkTime;
    private Rigidbody m_Rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.up.y > 0.0f || m_Rigidbody.velocity.magnitude > m_VelocityThreshold)
        {
            m_LastOkTime = Time.time;
        }

        if(Time.time > m_LastOkTime + m_WaitTime)
        {
            RightCar();
        }
    }

    private void RightCar()
    {
        transform.position += Vector3.up;
        transform.rotation = Quaternion.LookRotation(transform.forward);
    }

}
