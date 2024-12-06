using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactScript : MonoBehaviour
{
    public float total_scan_particles = 200;
    float scanned_amount = 0;
    bool is_scanned = false;

    void Scan(){
        if (is_scanned){return;}
        scanned_amount++;
        if (scanned_amount >= total_scan_particles){
            Debug.Log("Scanned", gameObject);
            GameManager.Instance.ArtifactScanned();
            is_scanned = true;
            this.enabled = false;
        }
    }

}
