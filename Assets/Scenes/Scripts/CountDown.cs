using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountDown : MonoBehaviour
{
    public TMP_Text Count;
    public TMP_Text FirstMessage;
    private int second;

    // Start is called before the first frame update
    void Start()
    {
        second = 3;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator CountStart()
    {
        if (second > 1)
        {
            Count.text = "< " + second + " >";
            second--;
        }
        yield return new WaitForSecondsRealtime(3.0f); 
    }

    private void GameStart()
    {
        


    }


    

}
