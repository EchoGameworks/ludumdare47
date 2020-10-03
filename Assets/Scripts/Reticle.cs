using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reticle : MonoBehaviour
{
    Controls input;
    Camera mainCam;
    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        input = GameManager.instance.Controls;

#if !UNITY_EDITOR
        Cursor.visible = false;
#endif
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = mainCam.ScreenToWorldPoint(input.Player.Mouse.ReadValue<Vector2>());
        this.transform.position = new Vector3(mousePos.x, mousePos.y, 0f);
    }
}
