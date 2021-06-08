using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.AI; 

public class talkToDumbass : MonoBehaviour
{
    Collider playerCol; 
    PlayerLocomotion playerScript; 
    pickUpPug pickUpPug; 
    Animator playerAnim; 
    Animator dumbassAnim; 
    GameObject player; 
    GameObject dumbass;
    public GameObject pug; 

    public GameObject gameImg; 
    public GameObject gameTxt; 
    public GameObject proceedObject; 
    public Text txt; 
    public Image bgImg;
    public Button proceedBtn; 

    public GameObject playerCam; 
    public GameObject dumbassCam;
    pugScript pugScript; 

    int talkCount = 0; 

    public bool inTalk;  
    bool canProceed; 
    bool inTrigger; 
    [SerializeField]
    bool playerHasPug; 
    bool gotQuest1; 
    bool completedQuest1; 
    bool gotQuest2; 
    bool completedQuest2; 
    bool inCo; 
    bool shouldSpeak; 

    [SerializeField]
    AudioClip[] dialogClips; 
    AudioSource audioSource;

    NavMeshAgent pugAgent; 
    public GameObject pugPickUpTrigger; 
    Animator pugAnim; 

    pugBroughtBackTrigger pugBroughtBackTrigger;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerCol = player.GetComponent<Collider>();
        playerScript = player.GetComponent<PlayerLocomotion>();
        playerAnim = player.GetComponentInChildren<Animator>();
        dumbassAnim = GameObject.Find("Dumbass").GetComponentInChildren<Animator>(); 
        dumbassCam.active = false; 
        pugScript = GameObject.Find("Pug").GetComponent<pugScript>();
        proceedObject.active = false; 
        pickUpPug = pug.GetComponentInChildren<pickUpPug>();
        audioSource = GetComponent<AudioSource>();
        pugBroughtBackTrigger = GameObject.Find("pugBroughtBack_Trig").GetComponent<pugBroughtBackTrigger>();
        pugAgent = GameObject.Find("Pug").GetComponent<NavMeshAgent>();
        pugAnim = GameObject.Find("Pug").GetComponent<Animator>();
        InvokeRepeating("Speak", Random.Range(0.1f,0.3f), Random.Range(0.15f,0.25f));
    }

    void Speak()
    {
        if(!canProceed && inTalk && !audioSource.isPlaying && !playerScript.isActive && shouldSpeak)
        {
            AudioClip dialogClip = GetDialogClip();
            audioSource.PlayOneShot(dialogClip);
            print(dialogClip);
        }
        else 
        {
            audioSource.Stop();
        }    
    }

    AudioClip GetDialogClip()
    {
        return dialogClips[UnityEngine.Random.Range(0, dialogClips.Length)];
    }

    void OnTriggerEnter(Collider playerCol)
    {
        if(!inTrigger)
        {
            inTrigger = true; 
            gameImg.active = true;             
            gameTxt.active = true; 
            txt.color = new Color(txt.color.r,txt.color.g,txt.color.b, 1f);
            ConvoUIVanish();
            txt.CrossFadeAlpha(1f,1f,false);
            bgImg.CrossFadeAlpha(0.9f,1f,false);
            txt.text = "Press MB1 to talk";
        }
    }

    void OnTriggerExit(Collider playerCol)
    {
        if(inTrigger)
        {
            print("out of trigger");
            playerCam.active = true; 
            dumbassCam.active = false; 
            txt.CrossFadeAlpha(0f,1f,false);
            bgImg.CrossFadeAlpha(0f,1f,false);
            inTrigger = false; 
        }
    }

    void Update()
    {
        Talk();

        print(canProceed);

        if(pugScript.isPickedUp)
        {
            playerHasPug = true;
        }
        else
        {
            playerHasPug = false; 
        }

        if(Input.GetMouseButtonDown(0) && inTrigger && !inTalk)
        {
            if(!playerHasPug && !pugBroughtBackTrigger.atHome)
            {

                playerScript.isActive = false; 
                playerAnim.SetBool("isMove", false);
                playerAnim.Play("Idle");
                StartCoroutine(CamFades());
       
                gameTxt.active = true;  
                txt.color = new Color(txt.color.r,txt.color.g,txt.color.b,1f); 
                gameImg.active = true;
                bgImg.color = new Color(bgImg.color.r,bgImg.color.g,bgImg.color.b, 1f);
                ConvoUIVanish();
                inTalk = true; 
                if(!audioSource.enabled)
                {
                    audioSource.enabled = true; 
                }
            }
        }

        if(Input.GetMouseButtonDown(0) && inTrigger && !inTalk)
        {
            if(playerHasPug || pugBroughtBackTrigger.atHome)
            {
                playerScript.isActive = false; 
                playerAnim.SetBool("isMove", false);
                playerAnim.Play("Idle");
                StartCoroutine(CamFades());
       
                gameTxt.active = true;  
                txt.color = new Color(txt.color.r,txt.color.g,txt.color.b,1f); 
                gameImg.active = true;
                bgImg.color = new Color(bgImg.color.r,bgImg.color.g,bgImg.color.b, 1f);
                ConvoUIVanish();
                inTalk = true; 
                talkCount = 111; 
                canProceed = false;
                if(!audioSource.enabled)
                {
                    audioSource.enabled = true; 
                } 
            }
        }

        if(pug.transform.position == new Vector3(120.977f,0.926f,58.805f))
        {
            dumbassAnim.SetBool("inHold", true);
            playerAnim.SetBool("isHolding&Idle", false);
            playerAnim.SetBool("inHold", false);
            playerAnim.Play("Idle");
        }
    }

    void Talk()
    {
        if(inTalk && talkCount == 0 && gotQuest1 && !completedQuest1) // CONVO BEGINNER 
        {
            dumbassAnim.SetBool("isWaving", true);
            txt.text = "Hey there again! Still no sign of the dog, huh? Oh I worry..."; 
            StartCoroutine(CamFadeTextFade());
            StartCoroutine(WaitToSpeakAudio());

           
            if(!inCo && !canProceed)
            {
                StartCoroutine(FirstDialogTime());
            }

            if(Input.GetMouseButtonDown(0) && canProceed)
            {
                canProceed = false; 
                talkCount = 4; 
                gameImg.active = false;             
                gameTxt.active = false;
                proceedObject.active = false;
                StopCoroutine(FirstDialogTime());
            }
        }

        if(inTalk && talkCount == 0 && gotQuest1 && completedQuest1 && !gotQuest2 && !completedQuest2) // CONVO BEGINNER 
        {
            dumbassAnim.SetBool("isTalking", true);
            txt.text = "Thank you again, so much! You returned little Bubbles too me, I am happy once again!"; 
            StartCoroutine(CamFadeTextFade());
            StartCoroutine(WaitToSpeakAudio());
           
            if(!inCo && !canProceed)
            {
                StartCoroutine(FirstDialogTime());
            }

            if(Input.GetMouseButtonDown(0) && canProceed)
            {
                canProceed = false; 
                talkCount = 4; 
                gameImg.active = false;             
                gameTxt.active = false;
                proceedObject.active = false;
                StopCoroutine(FirstDialogTime());
            }
        }

        if(inTalk && talkCount == 0 && !gotQuest1) // CONVO BEGINNER
        {
            dumbassAnim.SetBool("isWaving", true);
            txt.text = "Oh hi there! You just came from there? You are new right? Hmm...new"; 
            StartCoroutine(CamFadeTextFade());
            StartCoroutine(WaitToSpeakAudio());

            if(!inCo && !canProceed)
            {
                StartCoroutine(FirstDialogTime()); 
            }

            if(Input.GetMouseButtonDown(0) && canProceed)
            {
                canProceed = false; 
                talkCount = 1; 
                txt.CrossFadeAlpha(0.0f,0.01f,false);
                dumbassAnim.SetBool("isExcited", true);
                dumbassAnim.SetBool("isWaving", false);
                proceedObject.active = false; 
                StopCoroutine(FirstDialogTime());
            }
       }
   

        if(inTalk && talkCount == 1)
        {
            txt.text = "A new face! Haven't seen a new face in all this time...or that camera, those shoes, the hair! Oh so wow!";
            SpeakAudio();

            if(!inCo && !canProceed)
                StartCoroutine(DialogTime());

            if(Input.GetMouseButtonDown(0) && canProceed)
            {
                canProceed = false; 
                talkCount = 2; 
                txt.CrossFadeAlpha(0.0f,0.01f,false);
                dumbassAnim.SetBool("isLooking", true);
                dumbassAnim.SetBool("isExcited", false);
                proceedObject.active = false;
                StopCoroutine(DialogTime());
            }
        }

        if(inTalk && talkCount == 2)
        {
            txt.CrossFadeAlpha(1f,0.5f,false);
            txt.text = "I wish I had time to properly chat, but you see...the dog has ran off again!";
            SpeakAudio();
            
            if(!inCo && !canProceed)
                StartCoroutine(DialogTime());
           
            if(Input.GetMouseButton(0) && canProceed)
            {
                canProceed = false;
                talkCount = 3;
                txt.CrossFadeAlpha(0.0f, 0.01f, false);
                proceedObject.active = false;
                StopCoroutine(DialogTime());
            }
        }

        if(inTalk && talkCount == 3) // EXIT DIALOG 
        {
            txt.CrossFadeAlpha(1f,0.5f,false);
            txt.text = "Please kind lady, please! If you see my dog, bring him back to me...";
            dumbassAnim.Play("Looking");
            SpeakAudio();

            if(!inCo && !canProceed)
                StartCoroutine(DialogTime()); 

            if(Input.GetMouseButton(0) && canProceed)
            {
                canProceed = false;
                talkCount = 4;
                txt.CrossFadeAlpha(0.0f, 0.01f, false);
                dumbassAnim.SetBool("isLooking", false);
                gotQuest1 = true; 
                proceedObject.active = false;
                StopCoroutine(DialogTime());
                StopCoroutine(WaitToSpeakAudio());
            }
        } 
        if(inTalk && talkCount == 4) // LEAVE DIALOG POINT 
        {
            audioSource.Stop();
            shouldSpeak = false;
            StartCoroutine(OppCamFades());
            StopCoroutine(WaitToSpeakAudio());
            audioSource.enabled = false; 
            talkCount = -11; 
        }
        
        //QUEST 1 COMPLETED!! -- DOG BROUGHT HOME 
        if(inTalk && talkCount == 111) // CONVO BEGINNER
        {
            Destroy(pugPickUpTrigger); 
            pugScript.enabled = false;
            pugAgent.enabled = false; 
            pugAnim.SetBool("isMoving", false);               
            StartCoroutine(CamFadeTextFade());
            StartCoroutine(WaitToSpeakAudio());
            dumbassAnim.SetBool("isExcited", true);
            txt.text = "Bubble, you found Bubble! Wow, lady...thank you so much! You found him!";
           
            if(!inCo && !canProceed)
            {
                StartCoroutine(FirstDialogTime());
            }

            if(Input.GetMouseButton(0) && canProceed)
            {
                canProceed = false;
                talkCount = 112;
                txt.CrossFadeAlpha(0.0f, 0.01f, false);
                proceedObject.active = false;
                StopCoroutine(FirstDialogTime());

               
            }
        }
       
        if(inTalk && talkCount == 112)
        {
            dumbassAnim.SetBool("isTalking", true);
            txt.text = "I'll take him off your hands. Bubbles usually stays around the house, but when he wanders into that forest...";
            
            if(shouldSpeak)
                SpeakAudio();
           
            if(!inCo && !canProceed)
            {
                StartCoroutine(DialogTime());
            }

            if(Input.GetMouseButton(0) && canProceed)
            {
                dumbassAnim.SetBool("isTalking", false);
                dumbassCam.GetComponent<FadeCamera>().FadeOut(1f);

                if(!inCo)
                {
                    StartCoroutine(FadeDumbassCamIn());
                }
                canProceed = false;
                txt.CrossFadeAlpha(0.0f, 0.01f, false);
                proceedObject.active = false;
                StopCoroutine(DialogTime());
            }       
        }

        if(inTalk && talkCount == 113) // END OF CONVO 
        {
            txt.text = "Urgh...I just can't seem to walk out into there...I lose track of everything, I get...I get overwhelmed.";
            dumbassAnim.SetBool("isTalking", true);

            if(shouldSpeak)
                SpeakAudio();
            
            if(!inCo && !canProceed)
            {
                StartCoroutine(DialogTime());
            }

            if(Input.GetMouseButton(0) && canProceed)
            {
                canProceed = false;
                talkCount = 4;
                proceedObject.active = false;
                StartCoroutine(MoveDog());
                StopCoroutine(DialogTime());
                gameImg.active = false;
                gameTxt.active = false; 
                if(!gotQuest1)
                {
                 gotQuest1 = true; 
                }
                completedQuest1 = true; 
            }          
        }
    }

    IEnumerator FadeDumbassCamIn()
    {
        inCo = true; 
        InstantUIFadeOut();
        yield return new WaitForSeconds(1f);
        talkCount = -12; 
        txt.text = ""; 
        playerScript.isHolding = false;  
        pug.transform.parent = null; 
        pug.transform.localPosition = new Vector3(120.977f,0.926f,58.805f);
        pug.transform.localEulerAngles = new Vector3(3.159f, 128.391f, -2.577f);
        yield return new WaitForSeconds(1f);
        dumbassCam.GetComponent<FadeCamera>().FadeIn(1f);
        pugBroughtBackTrigger.enabled = false;
        talkCount = 113;
        InstantUIFadeIn();
        inCo = false; 
        shouldSpeak = true; 
    }

    IEnumerator CamFades()
    {
        playerCam.GetComponent<FadeCamera>().FadeOut(1f);
        yield return new WaitForSeconds(1f);
        playerCam.active = false; 
        playerCam.GetComponent<FadeCamera>().opacity = 0;
        dumbassCam.active = true; 
        dumbassCam.GetComponent<FadeCamera>().opacity = 0; 
        player.transform.position = new Vector3(120.4443f, 0.631f, 58.28118f);
        player.transform.rotation = Quaternion.Euler(0,37.692f,0);
        dumbassCam.GetComponent<FadeCamera>().FadeIn(3.0f);
        yield return new WaitForSeconds(3.0f);
        canProceed = false; 
        shouldSpeak = false; 
    }

    IEnumerator OppCamFades()
    {
        StopCoroutine(DialogTime());
        StopCoroutine(FirstDialogTime());
        bgImg.CrossFadeAlpha(0f, 0.1f, false);
        dumbassCam.GetComponent<FadeCamera>().FadeOut(1f);
        yield return new WaitForSeconds(1f);
        dumbassCam.active = false;
        playerCam.active = true; 
        playerCam.GetComponent<FadeCamera>().opacity = 0;
        playerCam.GetComponent<FadeCamera>().FadeIn(1.5f);
        yield return new WaitForSeconds(1.0f);
        playerScript.isActive = true; 
        inTalk = false; 
        talkCount = 0;
        canProceed = false;
        proceedObject.active = false; 
        shouldSpeak = false; 
    }

    IEnumerator CamFadeTextFade()
    {
        yield return new WaitForSeconds(1f);
        txt.CrossFadeAlpha(1f,2.0f,false);
        bgImg.CrossFadeAlpha(0.9f, 2.0f,false);
    }

    void InstantUIFadeIn()
    {
        txt.CrossFadeAlpha(1f,0.1f,false);
        bgImg.CrossFadeAlpha(0.9f,0.1f,false);
    }

    void InstantUIFadeOut()
    {
        txt.CrossFadeAlpha(0,0.1f,false);
        bgImg.CrossFadeAlpha(0,0.1f,false);
    }

    void ConvoUIVanish()
    {   
        txt.CrossFadeAlpha(0.0f, 0.01f, false); 
        bgImg.CrossFadeAlpha(0.0f, 0.01f, false);
    }

    IEnumerator DialogTime()
    {
        inCo = true; 
        yield return new WaitForSeconds(3f);
        canProceed = true;
        shouldSpeak = false; 
        if(inTalk)
        {
            proceedObject.active = true;
        }
        inCo = false; 
    }
   
    IEnumerator FirstDialogTime()
    {
        inCo = true;
        yield return new WaitForSeconds(4f);
        canProceed = true;
        shouldSpeak = false; 
        if(inTalk)
        {
            proceedObject.active = true;
        }
        inCo = false;

    }

    void ClearDumbassBools()
    {
        dumbassAnim.SetBool("isWaving", false);
        dumbassAnim.SetBool("isExcited", false);
        dumbassAnim.SetBool("isLooking", false);
        dumbassAnim.SetBool("isThankful", false);
        dumbassAnim.SetBool("inHold", false);
        dumbassAnim.SetBool("isTalking", false);
    }

    IEnumerator MoveDog()
    {
        yield return new WaitForSeconds(1f);
        pug.transform.localPosition = new Vector3(95.123f,0.003f,64.17f);
        pug.transform.eulerAngles = new Vector3(3.159f,141.054f,-2.577f);
        playerScript.isHolding = false; 
        pugScript.isPickedUp = false; 
        pugScript.hasReturned = true;
        Destroy(pickUpPug.pickUpTrigger);
        dumbassAnim.SetBool("inHold", false);   
    }
    
    IEnumerator WaitToSpeakAudio()
    {
        yield return new WaitForSeconds(1f);
        if(!shouldSpeak)
        {
            shouldSpeak = true; 
        }
    }

    IEnumerator WaitToSpeakAudioLonger()
    {
        yield return new WaitForSeconds(3f);
        if(!shouldSpeak)
        {
            shouldSpeak = true;
        }
    }

    void SpeakAudio()
    {
        if(!shouldSpeak)
            shouldSpeak = true; 
    }
}
