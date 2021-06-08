using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fadeInCameraOnEnable : MonoBehaviour
{
    void OnEnable()
    {
       FadeCamera fc = gameObject.GetComponent<FadeCamera>();
       fc.opacity = 0;
       fc.FadeIn(5f);
    }
}
