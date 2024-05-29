using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartInput : MonoBehaviour
{
    public GameObject FirstPannel;
    public GameObject BeepCube;
    public TMP_Text Go;
    public GameObject StartPanel;

    //private float num;


    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0.0f;
        //StartBeep = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //num = 3;
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(SoundCorutine());
        }
        StartCoroutine(DeleteStart());
        //num -= Time.deltaTime;
        //Count.text = Mathf.Round(num) + "";

    }

    IEnumerator SoundCorutine()
    {
        //Time.timeScale = 1;

        FirstPannel.SetActive(false);
        BeepCube.SetActive(true);

        yield return new WaitForSecondsRealtime(3.0f);
        Time.timeScale = 1.0f;
        Go.text = "Start!";
        
    }

    IEnumerator DeleteStart()
    {
        yield return new WaitForSeconds(2.0f);
        StartPanel.SetActive(false);
    }

}
