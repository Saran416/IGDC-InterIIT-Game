using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed = 1;
    [SerializeField] float gravityValue = 9.8f;
    [SerializeField] float GroundCheckRadius;
    [SerializeField] CharacterController controller;
    [SerializeField] Transform GroundCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float soundDuration = 2;
    [SerializeField] float size = 3;
    [SerializeField] GameObject EcholocationPrefab;

    private bool isGrounded;    
    private float lastSoundStart;
    void Start()
    {
        controller  = GetComponent<CharacterController>();
        lastSoundStart = -soundDuration - 1;

    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
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


    }

    void SpawnSoundWave(){
        GameObject echoLocator = Instantiate(EcholocationPrefab, gameObject.transform.position, Quaternion.identity) as GameObject;
        ParticleSystem echoWave = EcholocationPrefab.transform.GetChild(0).GetComponent<ParticleSystem>();

        if (echoWave != null){
            var main = echoWave.main;
            main.startLifetime = soundDuration;
            main.startSize = size;
        }else{
            Debug.Log("Empty/Erroneous echolocator\n");
        }


        Destroy(echoLocator, soundDuration + 1);
    }

}


