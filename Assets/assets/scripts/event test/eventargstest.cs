using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eventargstest : MonoBehaviour
{ 
    public event EventHandler onspacepressed;
    

    void Start()
    {
       
    }
  
    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            onspacepressed?.Invoke(this, EventArgs.Empty);
        }
    }
}
