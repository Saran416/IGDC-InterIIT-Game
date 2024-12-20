using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class grabbables : MonoBehaviour
{
    private Rigidbody grabTransfrom;
    private MeshCollider meshCollider;
    private Transform grab_point;
    private MeshRenderer meshRenderer;
    
    private void Awake()
    {
        grab_point = null;
        grabTransfrom = GetComponent<Rigidbody>();
        meshCollider = GetComponent<MeshCollider>();
        meshRenderer = GetComponent<MeshRenderer>();
    }
    public void Grab(Transform grab_point_transform){
        this.grab_point = grab_point_transform;
        grabTransfrom.isKinematic = true;
        grabTransfrom.useGravity = false;
        meshCollider.enabled = false;
        meshRenderer.enabled = false;

    }
    public void Drop(){
        this.grab_point = null;
        grabTransfrom.useGravity = true;
        meshCollider.enabled = true;
        meshRenderer.enabled = true;

        grabTransfrom.isKinematic=false;
        meshCollider.convex = true;
        {
            StartCoroutine(makeKinematic());
        }
    }

    IEnumerator makeKinematic(){
        yield return new WaitForSeconds(1);
        grabTransfrom.isKinematic = true;
        meshCollider.convex = false;
    }

    private void FixedUpdate()
    {
        if (grab_point != null){
            float pickspeed = 0.2f;
            Vector3 newposition = Vector3.Lerp(grabTransfrom.position,grab_point.position,pickspeed);
            grabTransfrom.MovePosition(newposition);
        }
    }
}
