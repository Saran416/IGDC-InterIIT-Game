using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundproof_area : MonoBehaviour
{
    //[SerializeField]
    //private EcholocationSignal signal;
    BoxCollider soundprood_collider;
    private void Awake(){
        soundprood_collider = GetComponent<BoxCollider>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player"){
            other.GetComponent<PlayerMovement>().safe = true;
            //signal.isAggro = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player"){
            other.GetComponent<PlayerMovement>().safe = true;
            //signal.isAggro = false;
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.tag == "Player"){
            other.GetComponent<PlayerMovement>().safe = false;
        }
    }
}
