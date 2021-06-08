using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI; 

public class pugScript : MonoBehaviour
{
    public float wanderRadius;
    public float wanderTimer;
 
    private Transform target;
    private NavMeshAgent agent;
    private float timer;

    public bool isPickedUp; 
    public bool hasReturned; 

    PlayerLocomotion playerLocomotion;

    Animator pugAnim; 

    public Image imgUI; 
    public Text txt; 
    public GameObject Text; 
    public GameObject imgBG; 

    //random pug load positions 
    Vector3 pugLoadPos1 = new Vector3(160.23f, 0.276f, 74.365f); 
    Vector3 pugLoadPos2 = new Vector3(128.22f, 0.462f, 81.86f);
    Vector3 pugLoadPos3 = new Vector3(166.289f, 0.325f, 39.473f); 

    [SerializeField]
    AudioClip[] barkClips; 
    AudioSource audioSource;  

    //for enable/disable - kind of like start()/awake(){}
    void OnEnable () 
    {
        pugAnim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent> ();
        timer = wanderTimer;
        playerLocomotion = GameObject.FindWithTag("Player").GetComponent<PlayerLocomotion>();  
        audioSource = GetComponent<AudioSource>();
    }

    void Awake()
    {
        int rand = Random.Range(1,4);
        if(rand == 1)
        {
            print("rand 1");
            gameObject.transform.position = pugLoadPos1;

        }
        else if(rand == 2)
        {
            print("rand 2");
            gameObject.transform.position = pugLoadPos2;  
        }
        else if(rand == 3)
        {
            print("rand 3");
            gameObject.transform.position = pugLoadPos3;  
        }

        InvokeRepeating("Bark", 1f, Random.Range(8f, 13f));
    }

    void Bark()
    {
        if(!isPickedUp)
        {
            AudioClip barkClip = GetBarkClip();
            audioSource.PlayOneShot(barkClip);
        }
    }

    AudioClip GetBarkClip()
    {
        return barkClips[UnityEngine.Random.Range(0, barkClips.Length)];
    }

 
    // Update is called once per frame
    void Update () 
    {

        if(!isPickedUp || !hasReturned)
        {
            timer += Time.deltaTime;
 
            if (timer >= wanderTimer) 
            {
                Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                agent.SetDestination(newPos);
                timer = 0;
            }
        }

        if(agent.velocity.magnitude > 0)
        {
            pugAnim.SetBool("isMoving", true);
        }
        else
        {
            pugAnim.SetBool("isMoving", false);
        }

        if(playerLocomotion.isHolding)
        {
            pugAnim.SetBool("isMoving", false);
            isPickedUp = true;
            agent.speed = 0; 
        }
        else
        {
            isPickedUp = false;
            agent.speed = 0.35f;  
        }

    }
 
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask) 
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
 
        randDirection += origin;
 
        NavMeshHit navHit;
 
        NavMesh.SamplePosition (randDirection, out navHit, dist, layermask);
 
        return navHit.position;
    }

}
