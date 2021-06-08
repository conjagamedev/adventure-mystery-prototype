using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIWander : MonoBehaviour
{
    public Transform[] points;
        private int destPoint = 0;
        private NavMeshAgent agent;
        private Animator animator; 


        void Start () 
        {
            agent = GetComponent<NavMeshAgent>();

            // Disabling auto-braking allows for continuous movement
            // between points (ie, the agent doesn't slow down as it
            // approaches a destination point).
            agent.autoBraking = false;
            animator = GetComponent<Animator>();
            

            GotoNextPoint();
        }


        void GotoNextPoint() 
        {
            // Returns if no points have been set up
            if (points.Length == 0)
                return;

            // Set the agent to go to the currently selected destination.
            agent.destination = points[destPoint].position;

            // Choose the next point in the array as the destination,
            // cycling to the start if necessary.
            destPoint = (destPoint + Random.Range(1,3)) % points.Length;
        }


        void Update () 
        {
            // Choose the next destination point when the agent gets
            // close to the current one.
            if (!agent.pathPending && agent.remainingDistance < 0.1f)
                GotoNextPoint();
        }
}