using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
public class MoveTable_nav : MonoBehaviour
{
    private enum States { IDLE, AGRO };
    private enum IDLE_STATES { TOPICK, PICK, TODROP, DROP };
    States state;
    IDLE_STATES idle_state;
    [SerializeField] private Transform PlayerTransform;
    [SerializeField] private GameObject[] Moveables;
    private Transform grab_point;
    private NavMeshAgent Nav_Agent;
    private GameObject grabbed;
    private bool isaggro = false;
    private void Awake()
    {
        Nav_Agent = GetComponent<NavMeshAgent>();
        grab_point = transform.GetChild(0);
    }
    private void Start()
    {
        state = States.IDLE;
        idle_state = IDLE_STATES.DROP;
    }
    private void Update()
    {
        isaggro = GetComponentInParent<EcholocationSignal>().isAggro;
        if (isaggro && !PlayerTransform.GetComponent<PlayerMovement>().safe)
        {
            if (idle_state == IDLE_STATES.TODROP)
            {
                grabbed.GetComponent<grabbables>().Drop();
            }
            state = States.AGRO;
        }
        else
        {
            state = States.IDLE;
        }
        if (state == States.AGRO)
        {
            Nav_Agent.SetDestination(PlayerTransform.position);
        }
        else if (state == States.IDLE)
        {
            System.Random rnd = new System.Random();
            int rndindex = rnd.Next(Moveables.Length);
            GameObject rndGameObject = grabbed;
            if (Moveables.Length != 0)
            {
                rndGameObject = Moveables[rndindex];
            }
            //check if someone picked it up
            /*if(grabbed != null && grabbed.GetComponent<grabbables>().grab_point != null){
                if (transform.child[0].position == grabbed.GetComponent<grabbables>().grab_point.position){
                    //we have it
                }else{
                    //someone else has it
                    Nav_Agent.SetDestination(rndGameObject.transform.position);
                    grabbed = rndGameObject;
                    idle_state = IDLE_STATES.TOPICK;
                }
            }*/
            if (!Nav_Agent.pathPending && rndGameObject != null)
            {
                if (Nav_Agent.remainingDistance <= Nav_Agent.stoppingDistance)
                {
                    if (!Nav_Agent.hasPath || Nav_Agent.velocity.sqrMagnitude == 0f)
                    {
                        if (idle_state == IDLE_STATES.TOPICK && rndGameObject != null)
                        {
                            Nav_Agent.SetDestination(rndGameObject.transform.position);
                            idle_state = IDLE_STATES.PICK;
                        }
                        else if (idle_state == IDLE_STATES.PICK)
                        {
                            grabbed.transform.TryGetComponent(out grabbables target);
                            target.Grab(grab_point);
                            RandMeshPoint(10, out Vector3 destination);
                            Nav_Agent.SetDestination(destination);
                            idle_state = IDLE_STATES.TODROP;
                        }
                        else if (idle_state == IDLE_STATES.TODROP)
                        {
                            Debug.Log("drop");
                            grabbed.transform.TryGetComponent(out grabbables target);
                            target.Drop();
                            idle_state = IDLE_STATES.DROP;

                        }
                        else if (idle_state == IDLE_STATES.DROP)
                        {
                            Nav_Agent.SetDestination(rndGameObject.transform.position);
                            grabbed = rndGameObject;
                            idle_state = IDLE_STATES.TOPICK;
                        }
                    }
                }
            }
        }
    }
    public bool RandMeshPoint(float radius, out Vector3 rand_pos)
    {
        for (int i = 0; i < 20; i++)
        {
            Vector2 xz = UnityEngine.Random.insideUnitCircle * radius;
            Vector3 randomDirection = new Vector3(xz.x, 0, xz.y);
            randomDirection += transform.position;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
            {
                rand_pos = hit.position;
                return true;
            }
        }
        rand_pos = transform.position;
        return false;
    }
}