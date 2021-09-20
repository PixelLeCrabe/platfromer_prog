using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eventargsuscriber : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        eventargstest testingEvents = GetComponent<eventargstest>();
        testingEvents.onspacepressed += TestingEvents_onspacepressed;
    }

    private void TestingEvents_onspacepressed(object sender, System.EventArgs e)
    {
        Debug.Log("aled");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
