using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; 
using UnityEngine.SceneManagement; 

public class diatrymaScript : MonoBehaviour
{
    GameObject playerObject; 
    Transform playerTrans; 

    float distance; 
    bool closeEnoughToPlayer; 
    bool lookingAtPlayer;
    [SerializeField]
    bool isChasing;
    bool isAttacking;   
    NavMeshAgent agent; 
    [SerializeField]
    float lookAngle;
    [SerializeField]
    float distanceAmount; 

    AIWander AIWander;
    private Ray sight; 

    Animator animator; 
    Animator playerAnimator; 
    PlayerLocomotion playerLocomotion; 
    [SerializeField]
    Camera mainCamera;

    void Start()
    {
        playerObject = GameObject.Find("Player");
        playerTrans = playerObject.GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        AIWander = GetComponent<AIWander>();
        animator = GetComponent<Animator>();
        playerAnimator = playerObject.GetComponentInChildren<Animator>();
        playerLocomotion = GameObject.Find("Player").GetComponent<PlayerLocomotion>();
    }

    void Update() 
    {
        print(distance);

        LookForPlayer();

        EatPlayer();
    }
    
    #region ---LOOKING FOR PLAYER---
    void LookForPlayer() 
    {
        distance = Vector3.Distance(playerTrans.position, transform.position);
        closeEnoughToPlayer = distance <= distanceAmount; // distance <= amount

        sight.origin = new Vector3(transform.position.x, transform.position.y + 0.25f, transform.position.z);
        sight.direction = transform.forward; 
        RaycastHit hit; 
        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        if(Physics.Linecast(transform.position, playerTrans.position))
        {
            lookingAtPlayer = false; 
        }
        else
        {
            lookingAtPlayer = true;
        }

        if(closeEnoughToPlayer && lookingAtPlayer && !isAttacking)
        {
            agent.destination = playerTrans.position; 
            isChasing = true; 
            AIWander.enabled = false; 
        } 
        else if(!isAttacking)
        {
            AIWander.enabled = true; 
            isChasing = false; 
        }

        if(isChasing && !isAttacking)
        {
            animator.SetBool("isMoving", false);
            animator.SetBool("inChase", true);
            agent.speed = 8f;  
        }
        if(!isChasing && agent.velocity.magnitude > 0 && !isAttacking)
        {
            animator.SetBool("inChase", false);
            animator.SetBool("isMoving", true);
            agent.speed = 2f; 
        }
        if(!isChasing && agent.velocity.magnitude == 0 && !isAttacking) 
        {
            animator.SetBool("inChase", false);
            animator.SetBool("isMoving", false);
        }
    }
    #endregion 

    void EatPlayer()
    {
        if(distance < 1.5f && !isAttacking)
        {
            isAttacking = true; 
            playerLocomotion.isActive = false;
            AIWander.enabled = false; 
            agent.enabled = false;
            isChasing = false; 
            animator.SetBool("isMoving", false);
            animator.SetBool("inChase", false);
            animator.SetBool("inAttack", true);
            playerAnimator.SetBool("isDead", true);
            mainCamera.GetComponent<FadeCamera>().FadeOut(1.5f);
            StartCoroutine(ReloadScene());
        }
    }

    IEnumerator ReloadScene()
    {
        yield return new WaitForSeconds(2f);
        Scene thisScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(thisScene.name);
    }
}
