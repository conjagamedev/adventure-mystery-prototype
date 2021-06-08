using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerFootsteps : MonoBehaviour
{
    [SerializeField]
    AudioClip[] footStepClips;
    [SerializeField]
    AudioClip[] rollClips;  
    AudioSource audioSource; 
       void Awake()
    {
        audioSource = GetComponentInParent<AudioSource>();
    }

    public void Step()
    {
        AudioClip stepClip = GetFootstepClip();
        audioSource.PlayOneShot(stepClip);
    }

    public void Roll()
    {
        AudioClip rollClip = GetRollClip();
        audioSource.PlayOneShot(rollClip);
    }

    private AudioClip GetFootstepClip()
    {
        return footStepClips[UnityEngine.Random.Range(0,footStepClips.Length)];
    }

    private AudioClip GetRollClip()
    {
        return rollClips[UnityEngine.Random.Range(0,rollClips.Length)];
    }
}
