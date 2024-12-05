using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using System;
using Unity.VisualScripting;
//using System.Numerics;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed = 1;
    [SerializeField] float gravityValue = 9.8f;
    [SerializeField] float GroundCheckRadius;
    [SerializeField] CharacterController controller;
    [SerializeField] Transform GroundCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float soundDuration = 2;
    [SerializeField] float echoRadius = 3;
    [SerializeField] GameObject EcholocationPrefab;
    [SerializeField] float alertRadius = 10;
    [SerializeField] float enemyLayer = 2;
    [SerializeField] float punchWaveRadius = 25;
    [SerializeField] float armLength = 2f;
    [SerializeField] float punchDelay = 2f;
    [SerializeField] float punchWaveDuration = 4f;

    [SerializeField] public Camera leCam;
    [SerializeField] public bool safe;

    private bool isGrounded;    
    private float lastSoundStart;

    private float lastPunchTime;
    void Start()
    {
        safe = false;
        controller  = GetComponent<CharacterController>();
        lastSoundStart = -soundDuration - 1;
        lastPunchTime = -punchDelay - 1;

    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        //Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        Vector3 move = transform.right * horizontal + transform.forward * vertical;
        move = move.normalized;
        
        controller.Move(move * speed * Time.deltaTime);

        // Ground Check
        if (Physics.CheckSphere(GroundCheck.position, GroundCheckRadius, groundLayer))
        {
           isGrounded = true;
           Debug.Log("Ground");
        } else {
           controller.Move(-Vector3.up * gravityValue * Time.deltaTime);
        }

        if (move != Vector3.zero){
            if (Time.realtimeSinceStartup > lastSoundStart + soundDuration){
                lastSoundStart = Time.realtimeSinceStartup;
                SpawnSoundWave();
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && Time.realtimeSinceStartup > lastPunchTime + punchDelay){
            lastPunchTime = Time.realtimeSinceStartup;
            //Debug.Log("Something was punched bro");
            Punch();

        }

    }

    void SpawnSoundWave(){
        GameObject echoLocator = Instantiate(EcholocationPrefab, gameObject.transform.position, Quaternion.identity) as GameObject;
        ParticleSystem echoWave = echoLocator.transform.GetChild(0).GetComponent<ParticleSystem>();

        if (echoWave != null){
            var main = echoWave.main;
            main.startLifetime = soundDuration;
            main.startSize = echoRadius;
        }else{
            Debug.Log("Empty/Erroneous echolocator\n");
        }

        //Checking enemies
        if (!safe){
            Collider[] colliders= Physics.OverlapSphere(echoLocator.transform.position, alertRadius);
            foreach (var col in colliders){
                //Debug.Log(col.gameObject.tag);
                if (col.gameObject.tag == "Enemy Body"){
                    EcholocationSignal signalScript = col.gameObject.GetComponentInParent<EcholocationSignal>();
                    //Debug.Log("Enemy Found!");
                    if (signalScript != null){
                        Debug.Log("adding sound");
                        signalScript.objectsDetected.Add(new Vector2 (echoLocator.GetInstanceID(), Time.realtimeSinceStartup));
                    }
                }
            }
        }



        Destroy(echoLocator, soundDuration + 1);
    }

    void Punch(){
        RaycastHit hit;
        Ray ray = leCam.ViewportPointToRay(Vector2.one * 0.5f);
        
        bool punchWaveSmall = false;


        if (Physics.Raycast(ray, out hit, armLength)) {
            Transform objectHit = hit.transform;
            if (objectHit.gameObject.tag == "BreakableWall"){
                Destroy(objectHit.gameObject);
                punchWaveSmall = true;
            }else if(objectHit.gameObject.tag == "BreakableWallBack"){
                Debug.Log("So i hit the child");
                Destroy(objectHit.transform.parent.gameObject);
                punchWaveSmall = true;
            }else if(objectHit.gameObject.GetComponent<MeshRenderer>() == null){ 
                //Debug.Log("searching in children");
                    if (objectHit.GetComponentInChildren<MeshRenderer>() != null){
                        //Debug.Log("Found one child satisfying this");
                        if (objectHit.GetComponentInChildren<MeshRenderer>().enabled == false){
                            punchWaveSmall = false;
                            //Debug.Log("Child's meshRenderer is off");
                        }
                        
                    }
            }else if (objectHit.gameObject.GetComponent<MeshRenderer>().enabled == false){
                //sound absorbent non breakable
                //Spawn small echo wave
                //Debug.Log("MeshRenderer is off\n");
                punchWaveSmall = true;
                
            }else{
                //Debug.Log("Not Sound Absorbing "+objectHit.name);
                punchWaveSmall = false;
            }
            
            //Debug.Log("Spawning Wave");
            SpawnPunchWave(hit.transform.position, punchWaveSmall);
            
            // Do something with the object that was hit by the raycast.
        }
    }

    void SpawnPunchWave(Vector3 originatingPoint, bool mode){
        GameObject echoLocator = Instantiate(EcholocationPrefab, originatingPoint, Quaternion.identity) as GameObject;
        ParticleSystem echoWave = echoLocator.transform.GetChild(0).GetComponent<ParticleSystem>();

        if (echoWave != null){
            var main = echoWave.main;
            main.startLifetime = soundDuration;
            main.startSize = punchWaveRadius;
            if (mode) main.startSize = echoRadius/2;
        }else{
            Debug.Log("Empty/Erroneous echolocator\n");
        }

        Debug.Log("Spawning object at position "+ originatingPoint + "is Small: "+ mode);

        float tagRadius = mode ? echoRadius/2 : punchWaveRadius; //inform enemies loaded by audio

        //Checking enemies
        if (!safe){
            Collider[] colliders= Physics.OverlapSphere(echoLocator.transform.position, punchWaveRadius);
            foreach (var col in colliders){
                //Debug.Log(col.gameObject.tag);
                if (col.gameObject.tag == "Enemy Body"){
                    EcholocationSignal signalScript = col.gameObject.GetComponentInParent<EcholocationSignal>();
                    //Debug.Log("Enemy Found!");
                    if (signalScript != null){
                        Debug.Log("adding sound");
                        signalScript.objectsDetected.Add(new Vector2 (echoLocator.GetInstanceID(), Time.realtimeSinceStartup));
                    }
                }
            }
        }



        Destroy(echoLocator, soundDuration + 1);
    }

}


