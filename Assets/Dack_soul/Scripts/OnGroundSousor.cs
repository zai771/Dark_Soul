using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGroundSousor : MonoBehaviour
{
    public CapsuleCollider capsule;
    public float offset = 0.3f;
    private Vector3 point01;
    private Vector3 point02;

    private float radius;

    // Use this for initialization
    void Awake()
    {
        radius = capsule.radius-0.01f;
    }

    void Update()
    {
        //print(transform.position);
    }
    private void FixedUpdate()
    {
        point01 = this.transform.position + transform.up * (radius-offset);
        point02 = this.transform.position + transform.up * (capsule.height-offset) - transform.up * radius;

        Collider[] outCollider = Physics.OverlapCapsule(point01, point02, radius, LayerMask.GetMask("Ground"));
        if (outCollider.Length != 0)
        {
            //foreach (var col in outCollider)
            //{
            //    //print("colloder"+col.name);  
            //}
            SendMessageUpwards("IsGround");
        }
        else
        {
            SendMessageUpwards("IsNotGround");
        }
    }
}
