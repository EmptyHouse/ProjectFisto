using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class EHTransitionUI : MonoBehaviour
{
    [SerializeField]
    private float TransitionDuration = 1f;
    [SerializeField] 
    private float BlackoutHold = 1f;
    [SerializeField]
    private Image TransitionImage;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void StartTransitionAnimation()
    {
        gameObject.SetActive(true);
    }

    private IEnumerator TransitionAnimationCoroutine()
    {
        float TimeThatHasPassed = 0;
        while (TimeThatHasPassed < TransitionDuration)
        {
            Color col = Color.black;
            col.a = TimeThatHasPassed / TransitionDuration;
            TransitionImage.color = col;
            TimeThatHasPassed += EHTime.DeltaTime;
            yield return null;
        }

        TransitionImage.color = Color.black;
        yield return new WaitForSeconds(BlackoutHold);
        TimeThatHasPassed = 0;

        while (TimeThatHasPassed < TransitionDuration)
        {
            Color col = Color.black;
            col.a = (1 - (TimeThatHasPassed / TransitionDuration));
            TransitionImage.color = col;
            TimeThatHasPassed += EHTime.DeltaTime;
            yield return null;
        }

        gameObject.SetActive(false);
    }
}
