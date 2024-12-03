using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EcholocationSignal : MonoBehaviour
{

    [SerializeField]public List<Vector2> objectsDetected;
    private List<Vector2> objectsToDelete;
    [SerializeField] float soundMemoryDuration = 15;
    [SerializeField] float maximumSounds = 3;

    [SerializeField]public bool isAggro = false;

    [SerializeField] GameObject EcholocationPrefabTag;
    
    [SerializeField] GameObject EcholocationPrefabHeart;
    [SerializeField] float heartBeatRadius = 3;
    [SerializeField] float heartBeatDuration = 1;
    [SerializeField] float tagWaveRadius = 30;
    [SerializeField] float tagWaveDuration = 4;
    private float lastHeartBeatTime;



    // Start is called before the first frame update
    void Start()
    {

        lastHeartBeatTime = -heartBeatDuration - 1;
        objectsToDelete = new List<Vector2>();
        objectsDetected = new List<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var obj in objectsDetected){
            if (obj[1] + soundMemoryDuration < Time.realtimeSinceStartup){
                objectsToDelete.Add(obj);
                Debug.Log("deleting obj");
            }
        }

        //Debug.Log("Before If");
        if (objectsDetected.Count - objectsToDelete.Count > maximumSounds){
            Debug.Log("if works\n");
            if (!isAggro){
                SpawnTagWave();
            }
            isAggro = true;
        }

        foreach(var obj in objectsToDelete){
            objectsDetected.Remove(obj);
        }

        if (isAggro){
            if (Time.realtimeSinceStartup > lastHeartBeatTime + heartBeatDuration){
                lastHeartBeatTime = Time.realtimeSinceStartup;
                SpawnHeartWave();
            }
        }
    }

    void SpawnHeartWave(){
        GameObject heart = Instantiate(EcholocationPrefabHeart, gameObject.transform.position, Quaternion.identity) as GameObject;
        ParticleSystem heartWave = heart.transform.GetChild(0).GetComponent<ParticleSystem>();

        if (heartWave != null){
            var main = heartWave.main;
            main.startLifetime = heartBeatDuration;
            main.startSize = heartBeatRadius;
        }else{
            Debug.Log("Empty/Erroneous heartBeat\n");
        }

        Destroy(heart, heartBeatDuration + 1);
    }

    void SpawnTagWave(){
        GameObject tagger = Instantiate(EcholocationPrefabTag, gameObject.transform.position, Quaternion.identity) as GameObject;
        ParticleSystem tagWave = tagger.transform.GetChild(0).GetComponent<ParticleSystem>();

        if (tagWave != null){
            var main = tagWave.main;
            main.startLifetime = tagWaveDuration;
            main.startSize = tagWaveRadius;
        }else{
            Debug.Log("Empty/Erroneous tagWave\n");
        }

        Destroy(tagger, tagWaveDuration + 1);
    }
}
