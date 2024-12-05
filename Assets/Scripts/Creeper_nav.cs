using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class Creeper_nav : MonoBehaviour
{
    private Transform player_Transform;
    [SerializeField]
    protected float Walk_radius;
    [SerializeField]
    protected float wait_time;
    private enum State {idle,active}; // idle -> player haven't detected,active -> player has been detected.
    private State state; 
    private SphereCollider deducting_sphere;
    private NavMeshAgent Nav_Agent;
    protected Vector3 Destination;
    private void Awake()
    {
        Nav_Agent = GetComponent<NavMeshAgent>();
        deducting_sphere = GetComponent<SphereCollider>();
        Destination = transform.position;
    }
    private void Start()
    {
        state = State.idle;
        // Nav_Agent.SetDestination(Destination);
    }
    private void Update()
    {
        // Random walk
        if (state == State.idle){
            if (!Nav_Agent.pathPending){
                if (Nav_Agent.remainingDistance <= Nav_Agent.stoppingDistance){
                    if (!Nav_Agent.hasPath || Nav_Agent.velocity.sqrMagnitude == 0f){
                        StopCoroutine(Random_walk());
                        StartCoroutine(Random_walk());
                    }
                }
            }
        }
        // follow player through the end
        else if (state == State.active){
            Nav_Agent.SetDestination(player_Transform.position);

        }
    }
    //coroutine for random walk
    IEnumerator Random_walk(){
        if (RandMeshPoint(Walk_radius,out Destination)){
            Nav_Agent.Warp(transform.position);
            yield return new WaitForSeconds(wait_time);
            Nav_Agent.SetDestination(Destination);
        }
    }
    //to check if the player is visible while entering the detection colloider
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player"){
            Ray ray = new Ray(transform.position,(other.transform.position - transform.position).normalized);
            Physics.Raycast(ray,out RaycastHit hit);
            if (hit.transform.tag == "Player"){
                state = State.active;
                player_Transform = other.transform;
            }
        }
    }
    //to check if the player is visible while inside the detection colloider
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player"){
            Ray ray = new Ray(transform.position,(other.transform.position - transform.position).normalized);
            Physics.Raycast(ray,out RaycastHit hit);
            if (hit.transform.tag == "Player"){
                state = State.active;
                player_Transform = other.transform;
            }
        }
    }
    //Generates random points in the navmesh at a max distance radius. returns true if a point is found else false
    // out rand_pos -> random vector for destination.
    public bool RandMeshPoint(float radius, out Vector3 rand_pos) {
        for (int i = 0;i < 20;i++){
            Vector2 xz = UnityEngine.Random.insideUnitCircle * radius;
            Vector3 randomDirection = new Vector3(xz.x,0,xz.y);
            randomDirection += transform.position;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1)) {
                rand_pos = hit.position;            
                return true;
            }
        }
        rand_pos = transform.position;
        return false;
    }
}
