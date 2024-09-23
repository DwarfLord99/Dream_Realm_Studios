using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CourageMeter_AV : MonoBehaviour
{
    // UI elements
    public Image courageMeter;
    public Image lowCourageBar;
    public Image midCourageBar; 
    public Image highCourageBar;

    // courage meter values
    public float courageMax = 100f; // maximum courage range
    public float courageCur = 0f; // current courage level starts at 0

    // values when courage increases or decreases
    public float courageIncrease = 10f;
    public float courageDecrease = 5f;

    public float lowCourageThreshold = 25f;
    public float midCourageThreshold = 75f;
    public float highCourageThreshold = 100f;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    public void UpdateCourageMeter()
    {
        // calculate fill amount based on current courage range
        float fillAmount = courageCur / courageMax;
        courageMeter.fillAmount = fillAmount;

        // colors will fill as player kills enemies
        if (courageCur <= lowCourageThreshold)
        {
            courageMeter.color = Color.red; // low courage
            lowCourageBar.fillAmount = fillAmount; // fill low courage bar

            // reset mid and high courage bars
            midCourageBar.fillAmount = 0; 
            highCourageBar.fillAmount = 0;
        }
        else if (courageCur <= midCourageThreshold)
        {
            courageMeter.color = Color.blue; // mid courage
            lowCourageBar.fillAmount = 1; // fill low courage bar
            midCourageBar.fillAmount = (courageCur -  lowCourageThreshold) / (midCourageThreshold - lowCourageThreshold); // fill mid courage bar
            highCourageBar.fillAmount = 0; // reset high courage bar
        }
        else
        {
            courageMeter.color = Color.green; // high courage 
            lowCourageBar.fillAmount = 1; // fill low courage bar
            midCourageBar.fillAmount = 1; // fill mid courage bar
            highCourageBar.fillAmount = (courageCur - midCourageThreshold) / (highCourageThreshold - midCourageThreshold); // fill high courage bar
        }

    }

    // method to increase courage value as enemies are killed
    public void IncreaseCourage()
    {
        courageCur += courageIncrease;
        courageCur = Mathf.Clamp(courageCur, 0f, courageMax);
        UpdateCourageMeter();
    }

}
