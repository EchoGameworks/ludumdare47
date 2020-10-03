using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class DebugToolbox : MonoBehaviour
{
    public Controls input;
    public TextMeshProUGUI gameTimeText;
    public RectTransform InputList;
    public GameObject prefabInputText;

    public float GameSpeed;
    float id;

    void Start()
    {
        //input = InputController.instance.Input;
        GameSpeed = 1f;

        //input.Debug.SpeedUp.performed += SpeedUp_performed;
        //input.Debug.SlowDown.performed += SlowDown_performed;
        //input.Debug.DefaultSpeed.performed += DefaultSpeed_performed;
        //input.Debug.SetUltraSlow.performed += SetUltraSlow_performed;

        //input.Player.Action1.performed += TrackInput;
        //input.Player.Action2.performed += TrackInput;
        //input.Player.Action3.performed += TrackInput;
        //input.Player.Action4.performed += TrackInput;
        //input.Player.Movement.performed += TrackInput;

        gameTimeText.alpha = 0f;
    }

    public void FlashSpeed()
    {
        LeanTween.cancel(gameObject);
        Time.timeScale = GameSpeed;
        gameTimeText.text = GameSpeed.ToString("F2");
        gameTimeText.alpha = 1f;
        id = LeanTween.value(gameObject, a => gameTimeText.color = a, new Color(1, 1, 1, 1), new Color(1, 1, 1, 0), 0.5f).setDelay(1f).id;
    }

    private void SetUltraSlow_performed(InputAction.CallbackContext obj)
    {
        GameSpeed = 0.05f;
        FlashSpeed();
    }

    private void DefaultSpeed_performed(InputAction.CallbackContext obj)
    {
        GameSpeed = 1f;
        FlashSpeed();
    }

    private void SlowDown_performed(InputAction.CallbackContext obj)
    {
        GameSpeed /= 2f;
        GameSpeed = Mathf.Clamp(GameSpeed, 0.01f, 1f);
        FlashSpeed();
    }

    private void SpeedUp_performed(InputAction.CallbackContext obj)
    {
        GameSpeed *= 2f;
        GameSpeed = Mathf.Clamp(GameSpeed, 0.01f, 1f);
        FlashSpeed();
    }

    public void TrackInput(InputAction.CallbackContext obj)
    {
        GameObject textInst = Instantiate(prefabInputText, InputList);
        TextMeshProUGUI tmp = textInst.GetComponent<TextMeshProUGUI>();
        tmp.text = obj.action.activeControl.displayName;
        LeanTween.value(textInst, a => tmp.color = a, new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), 0.3f);
        Destroy(textInst, 3f);
    }
}

/// <summary>
/// An auxiliary class containing methods that help change only one of the structure values.
/// </summary>
/// <example>
/// Usage:
/// <code>
/// gameObject.transform.localPosition = Replace.Y(gameObject.transform.localPosition, f*0.10f);
/// </code>
/// or
/// <code>
/// gameObject.transform.localPosition = gameObject.transform.localPosition.Y(f*0.10f);
/// </code>
/// </example>
public static class VectorChange
{

    public static Vector2 ReplaceX(this Vector2 v, float x)
    {
        v.x = x;
        return v;
    }

    public static Vector2 ReplaceY(this Vector2 v, float y)
    {
        v.y = y;
        return v;
    }

    public static Vector3 ReplaceX(this Vector3 v, float x)
    {
        v.x = x;
        return v;
    }

    public static Vector3 ReplaceY(this Vector3 v, float y)
    {
        v.y = y;
        return v;
    }

    public static Vector3 ReplaceZ(this Vector3 v, float z)
    {
        v.z = z;
        return v;
    }


    public static Vector3Int ReplaceX(this Vector3Int v, int x)
    {
        v.x = x;
        return v;
    }

    public static Vector3Int ReplaceY(this Vector3Int v, int y)
    {
        v.y = y;
        return v;
    }

    public static Vector3Int ReplaceZ(this Vector3Int v, int z)
    {
        v.z = z;
        return v;
    }

    public static Vector3Int ConvertToVector3Int(this Vector3 v)
    {
        return new Vector3Int(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y), Mathf.RoundToInt(v.z));
    }

    public static Vector3Int ToVector3Int(this Vector3 v)
    {
        return new Vector3Int((int)v.x, (int)v.y, (int)v.z);
    }

    public static Color ReplaceR(this Color c, float r)
    {
        c.r = r;
        return c;
    }

    public static Color ReplaceG(this Color c, float g)
    {
        c.g = g;
        return c;
    }

    public static Color ReplaceB(this Color c, float b)
    {
        c.b = b;
        return c;
    }

    public static Color ReplaceA(this Color c, float a)
    {
        c.a = a;
        return c;
    }

    public static Vector2 AddX(this Vector2 v, float x)
    {
        v.x += x;
        return v;
    }

    public static Vector2 AddY(this Vector2 v, float y)
    {
        v.y += y;
        return v;
    }

    public static Vector3 AddX(this Vector3 v, float x)
    {
        v.x += x;
        return v;
    }

    public static Vector3 AddY(this Vector3 v, float y)
    {
        v.y += y;
        return v;
    }

    public static Vector3 AddZ(this Vector3 v, float z)
    {
        v.z += z;
        return v;
    }

    public static Vector3Int AddX(this Vector3Int v, int x)
    {
        v.x += x;
        return v;
    }

    public static Vector3Int AddY(this Vector3Int v, int y)
    {
        v.y += y;
        return v;
    }

    public static Vector3Int AddZ(this Vector3Int v, int z)
    {
        v.z += z;
        return v;
    }


    public static Color AddR(this Color c, float r)
    {
        c.r += r;
        return c;
    }

    public static Color AddG(this Color c, float g)
    {
        c.g += g;
        return c;
    }

    public static Color AddB(this Color c, float b)
    {
        c.b += b;
        return c;
    }

    public static Color AddA(this Color c, float a)
    {
        c.a += a;
        return c;
    }

}
