using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DeathBanner : MonoBehaviour
{
    public Image Background;
    public RectTransform DiedRT;
    public RectTransform InstructionsRT;
    Controls input;
    private bool IsShowingDead = false;

    private void Start()
    {
        input = GameManager.instance.Controls;
        input.Player.Jump.performed += Jump_performed;
        DiedRT.anchoredPosition = new Vector2(DiedRT.anchoredPosition.x, 500f);
        InstructionsRT.anchoredPosition = new Vector2(InstructionsRT.anchoredPosition.x, -500f);
        Background.color = new Color(Background.color.r, Background.color.g, Background.color.b, 0);
    }

    private void Jump_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (IsShowingDead)
        {
            HideScreen();
        }
    }

    public void ShowScreen()
    {
        IsShowingDead = true;
        LeanTween.value(gameObject, val => DiedRT.anchoredPosition = val,
            DiedRT.anchoredPosition, new Vector2(DiedRT.anchoredPosition.x, 30f), 0.3f)
            .setEaseOutBounce()
            .setDelay(0.6f);
        LeanTween.value(gameObject, val => InstructionsRT.anchoredPosition = val,
            InstructionsRT.anchoredPosition, new Vector2(InstructionsRT.anchoredPosition.x, -35f), 0.3f)
            .setEaseOutBounce()
            .setDelay(0.6f); ;
        LeanTween.value(gameObject, a => Background.color = a, 
            new Color(Background.color.r, Background.color.g, Background.color.b, Background.color.a), 
            new Color(Background.color.r, Background.color.g, Background.color.b, 1), 0.3f);
    }

    public void HideScreen()
    {
        GameManager.instance.playerController.ResetPlayerHealth();
        IsShowingDead = false;
        LeanTween.value(gameObject, val => DiedRT.anchoredPosition = val,
            DiedRT.anchoredPosition, new Vector2(DiedRT.anchoredPosition.x, 500f), 0.3f)
            .setEaseOutBounce();
        LeanTween.value(gameObject, val => InstructionsRT.anchoredPosition = val,
            InstructionsRT.anchoredPosition, new Vector2(InstructionsRT.anchoredPosition.x, -500f), 0.3f)
            .setEaseOutBounce();
        LeanTween.value(gameObject, a => Background.color = a,
            new Color(Background.color.r, Background.color.g, Background.color.b, Background.color.a),
            new Color(Background.color.r, Background.color.g, Background.color.b, 0), 0.5f)
            .setDelay(0.6f);
    }
}
