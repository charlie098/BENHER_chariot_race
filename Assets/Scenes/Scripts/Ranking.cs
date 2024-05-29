using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Ranking : MonoBehaviour
{
    public TMP_Text Lap;
    private int count;
    private int NPCcount;
    private int firstLap;
    private int NPCLap;
    public GameObject panal;
    public GameObject NPCpanal;

    // Start is called before the first frame update
    void Start()
    {
        firstLap = -1;
        count = 0;
        NPCcount = 0;
        NPCLap = 0;
        //Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        Lap.text = "·¦ " + count + " / 1";
        End();
        NPCwin();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerChariot"))
        {
            firstLap++;
            RealLap();
        }
        else if (other.gameObject.CompareTag("NPCFrontBumper"))
        {
            NPCLap++;
            //NPCRealLap();
        }

    }

    private void RealLap()
    {
        if (firstLap > 0)
        {
            count++;
        }
    }
    
    /*
    private void NPCRealLap()
    {
        if(NPCLap > 0)
        {
            NPCcount++;
        }
    }
    */

    private void End()
    {
        if (count == 1)
        {
            panal.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
    }

    private void NPCwin()
    {
        if(NPCLap == 5)
        {
            NPCpanal.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
    }


}
