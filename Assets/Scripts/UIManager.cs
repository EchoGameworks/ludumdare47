using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public RectTransform TimerRT;
    public TextMeshProUGUI Timer;
    public RectTransform BossHealthRT;
    public DeathBanner DeathBanner;

    public Image PlayerHealth;
    public Image BossHealth;


    // Start is called before the first frame update
    void Start()
    {
        TimerRT.anchoredPosition = new Vector2(TimerRT.anchoredPosition.x, 50f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowDeathBanner()
    {
        DeathBanner.ShowScreen();
    }

    public void Timer_PopIn()
    {
        LeanTween.value(gameObject, val => TimerRT.anchoredPosition = val, 
            TimerRT.anchoredPosition, new Vector2(TimerRT.anchoredPosition.x, -33f), 0.3f)
            .setEaseOutBounce();
    }

    public void Timer_PopOut()
    {
        LeanTween.value(gameObject, val => TimerRT.anchoredPosition = val,
            TimerRT.anchoredPosition, new Vector2(TimerRT.anchoredPosition.x, 50f), 0.3f)
            .setEaseOutBounce();
    }

    public void BossHealth_PopIn()
    {
        LeanTween.value(gameObject, val => BossHealthRT.anchoredPosition = val,
        BossHealthRT.anchoredPosition, new Vector2(BossHealthRT.anchoredPosition.x, -33f), 0.3f)
        .setEaseOutBounce();

    }

    public void BossHealth_PopOut()
    {
        LeanTween.value(gameObject, val => BossHealthRT.anchoredPosition = val,
            BossHealthRT.anchoredPosition, new Vector2(BossHealthRT.anchoredPosition.x, 50f), 0.3f)
            .setEaseOutBounce();
    }

    public void UpdateTimer(float time)
    {
        Timer.text = time.ToString("N0");
    }

    public void UpdatePlayerHealth(float healthPerc)
    {
        //if(healthPerc == 1)
        //{
        //    print("new health");
        //}
        PlayerHealth.fillAmount = healthPerc;
    }

    public void UpdateBossHealth(float healthPerc)
    {
        BossHealth.fillAmount = healthPerc;
    }

}
