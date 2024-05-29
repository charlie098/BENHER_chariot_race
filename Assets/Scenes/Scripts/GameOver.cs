using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOver : MonoBehaviour
{
    [SerializeField] private float m_WaitTime = 2.0f;
    [SerializeField] private float m_VelocityThreshold = 1.0f;


    //private Rigidbody m_Rigidbody;
    public TMP_Text BedEnd;
    public GameObject panel;
    //private float m_lastTime;

    // Start is called before the first frame update
    void Start()
    {
        //m_Rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (transform.up.y > 0.0f || m_Rigidbody.velocity.magnitude > m_VelocityThreshold)
        if(gameObject.transform.rotation.y > 0)
        {
            BedEnd.text = "Game Over";
            End();
        }


    }

    private void End()
    {
        panel.SetActive(true);
    }

}
