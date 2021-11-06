using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PayLoadController : MonoBehaviour
{

    public NavMeshAgent agent;
    public GameObject target;
    void Update()
    {
        agent.SetDestination(target.transform.position);
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            agent.speed = 0.3f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            agent.speed = 0;
        }
    }


}
