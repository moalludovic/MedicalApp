using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TimerMastication : MonoBehaviour {

    Image img_timer;
    public Text timeText;
    bool isActive = false;
    public float timeAmount;
    public float time;
    public float multiplier = 1.0f;

    void Start()
    {
        img_timer = this.GetComponent<Image>();
        img_timer.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            if (time > 0f)
            {
                img_timer.fillAmount = time / timeAmount;
                timeText.text = (time*multiplier).ToString("F");
                time -= Time.deltaTime;
            }
            else
            {
                time = 0f;
                timeText.text = 0.0.ToString("F");
                img_timer.fillAmount = 0;
                isActive = false;
                MealManager.Instance().TimerUI.SetActive(false);
            }
        }
    }

    public void initializeTimer(float newTimeAmount){
        timeAmount = newTimeAmount;
        time = timeAmount;
        isActive = true;
    }
}
