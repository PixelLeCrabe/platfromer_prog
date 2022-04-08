using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandedCheck : MonoBehaviour
{

    [SerializeField] Collider2D landedCheck;
    controller_test controller;

    private void Start()
    {
        controller = GetComponent<controller_test>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
