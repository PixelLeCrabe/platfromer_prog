using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thrown_item : MonoBehaviour
{
    private Transform DmgPoint;

    [SerializeField] private LayerMask DamagableEntities;

    [SerializeField] private float DmgAmount;
    [SerializeField] private float Dmgrange;

    private Grab grab;

    private bool HasBeenGrabOnce;
    public bool ItemIsGrabed;

    private void Start()
    {
        DmgPoint = GetComponentInChildren<Transform>();
        HasBeenGrabOnce = false;
    }

    private void Isgrabbed()
    {
        // Test : Identify the grabed item by his name 
        /*if (gameObject.name == Grab.instance.GrabedItemName && Grab.instance.IsHoldingAgrabedItem)
        {
            ItemIsGrabed = true;
            gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            HasBeenGrabOnce = true;
            Debug.Log(gameObject.name + " is Grabbed and dynamik");
        }     
        */
    }
    private void Explosion()
    {
    }

    private void OnCollisionEnter2D()
    {

        // add condition To avoid hitting the MC when he grabs the barrel 
        // No hit when the barrel is realesed
        if (!HasBeenGrabOnce) return;
        
        Collider2D[] hitEntity = Physics2D.OverlapCircleAll(DmgPoint.position, Dmgrange, DamagableEntities);

        foreach (Collider2D enemy in hitEntity)
        {
            //Cinemachine_cameraShake.instance.MCAttackShake(.5f, 0.2f);
            //enemy.GetComponent<Enemy>().takeDamage(DmgAmount);

            print(enemy.tag + " Is damaged by a barrel");
        }
        Debug.Log(gameObject.name + " Hit the ground");
        
        // Not relevent for now
        //gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;        
    }
    // Update is called once per frame
    void Update()
    {
        Isgrabbed();

        /*if (!Grab.instance.IsHoldingAgrabedItem)
        {
            ItemIsGrabed = false;
        }*/
        //Explosion();
    }
}
