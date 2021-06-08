using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; 

public class pickUpPug : MonoBehaviour
{
    Collider playerCollider; 
    Animator playerAnim;
    PlayerLocomotion playerLocomotion;
    GameObject pug; 
    public GameObject pickUpTrigger; 
    pugScript pugScript; 
    GameObject player; 
    NavMeshAgent pugAgent; 
    Animator pugAnim;
    GameObject pugPickUp;  
    public GameObject dumbass; 
    public bool inTrigger; 
    talkToDumbass talkToDumbass; 

    void Start()
    {
        playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>();
        playerAnim = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Animator>();  
        playerLocomotion = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerLocomotion>();
        pug = GameObject.Find("Pug"); 
        pickUpTrigger = GameObject.Find("pickUpTrigger");
        pugScript = pug.GetComponent<pugScript>();
        player = GameObject.FindWithTag("Player");
        pugAgent = pug.GetComponent<NavMeshAgent>();
        pugAnim = pug.GetComponent<Animator>();
        pugPickUp = GameObject.Find("pugPickUp");
        talkToDumbass = dumbass.GetComponentInChildren<talkToDumbass>();
    }

    void OnTriggerEnter(Collider playerCollider)
    {
        inTrigger = true; 
        print("pick up dog/in trigger"); 
        
        if(pugScript.isPickedUp == false)
        {
            pugScript.Text.active = true; 
            pugScript.imgBG.active = true; 
            pugScript.txt.enabled = true; 
            pugScript.imgUI.enabled = true; 
            pugScript.imgUI.color = new Color(pugScript.imgUI.color.r,pugScript.imgUI.color.g,pugScript.imgUI.color.b, 0.9f);
            pugScript.txt.color = new Color(pugScript.txt.color.r,pugScript.txt.color.g,pugScript.txt.color.b, 1f);
            pugScript.txt.CrossFadeAlpha(1f,0.5f,false);
            pugScript.imgUI.CrossFadeAlpha(0.9f,0.5f,false);
            pugScript.txt.text = "Press MB1 to pick up the dog";
        }
    }

    void OnTriggerExit(Collider playerCollider)
    {
        inTrigger = false; 
        print("left trigger for dog/area");
        pugScript.txt.text = "";
        
        if(pugScript.isPickedUp == false || pugScript.txt.text != "Press MB2 to put down the dog"); 
        {
            pugScript.Text.active = false; 
            pugScript.imgBG.active = false;

        }
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0) && inTrigger && playerLocomotion.canJump && !playerLocomotion.isFalling && !playerLocomotion.inRoll && !talkToDumbass.inTalk)
        {
            if(!playerLocomotion.isHolding)
            {
                playerLocomotion.isHolding = true; 
                playerAnim.SetBool("isFall", false);
                playerAnim.SetBool("isMove", false);
                playerAnim.SetBool("isFall", false);
                playerAnim.SetBool("isRoll", false);
                playerAnim.SetBool("inSprint", false);
                playerAnim.Play("Idle");
                pugScript.isPickedUp = true;
                pickUpTrigger.GetComponent<BoxCollider>().enabled = false;  
                pugScript.Text.active = true; 
                pugScript.imgBG.active = true;
                pugScript.imgUI.color = new Color(pugScript.imgUI.color.r,pugScript.imgUI.color.g,pugScript.imgUI.color.b, 0.9f); 
                pugScript.txt.text = "Press MB2 to put down the dog";
            }
        }

        if(Input.GetMouseButtonDown(1) && pugScript.isPickedUp && !talkToDumbass.inTalk)
        {
            pugScript.isPickedUp = false; 
            pugAgent.enabled = true;
            pugAgent.speed = 0.35f; 
            pugScript.enabled = true; 
            pug.transform.parent = null; 
            playerLocomotion.isHolding = false; 
            playerAnim.SetBool("inHold", false);
            playerAnim.SetBool("isHolding&Idle", false);
            playerAnim.Play("Idle");
            pickUpTrigger.GetComponent<BoxCollider>().enabled = true; 
            pugScript.Text.active = true; 
            pugScript.imgBG.active = true; 
            pugScript.txt.text = ""; 
        }

        if(pugScript.isPickedUp && !talkToDumbass.inTalk)
        {
            DogPickedUp();
        } 
    }

    void DogPickedUp()
    {
        pugAnim.SetBool("isMoving", false);
        pugAgent.enabled = false; 
        pugScript.enabled = false;    
        pug.transform.parent = pugPickUp.transform;
        pug.transform.localPosition = Vector3.zero; 
        pug.transform.localEulerAngles = Vector3.zero; 
    }
    
    void ConvoUIVanish()
   {   
        pugScript.txt.CrossFadeAlpha(0.0f, 0.01f, false); 
        pugScript.imgUI.CrossFadeAlpha(0.0f, 0.01f, false);
   }
}
