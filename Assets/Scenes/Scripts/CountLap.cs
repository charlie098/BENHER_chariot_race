using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountLap : MonoBehaviour
{

    public TMP_Text Lap;
    private int count;
    private int firstLap;
    public GameObject panal;

    // Start is called before the first frame update
    void Start()
    {
        firstLap = -1;
        count = 0;
        //Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        Lap.text = "·¦ " + count + " / 3";
        End();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerChariot"))
        {
            firstLap++;
            RealLap();
        }

    }

    private void RealLap()
    {
        if(firstLap > 0)
        {
            count++;
        }
    }

    private void End()
    {
        if(count == 3)
        {
            panal.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
    }


}
