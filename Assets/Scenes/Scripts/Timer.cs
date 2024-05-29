using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public float CurrentTime;
    //public float LeftTime;
    public TMP_Text timer;
    public GameObject panel;


    // Start is called before the first frame update
    void Start()
    {
        //Time.timeScale = 1;
        CurrentTime = 100.0f;
        //LeftTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        CurrentTime -= Time.deltaTime;
        timer.text = Mathf.Round(CurrentTime) + " left time ";
        if (CurrentTime <= 0)
        {
            panel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    

}
