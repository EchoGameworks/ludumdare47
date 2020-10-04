using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTransition : MonoBehaviour
{
    public GameObject TransitionTo;
    public GameObject Blocker;

    public List<GameObject> DungeonCameras;
    // Start is called before the first frame update
    void Start()
    {
        Blocker.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SetCamera();
    }

    private void SetCamera()
    {
        TransitionTo.SetActive(true);
        Blocker.SetActive(true);
    }

    public void RespawnReset()
    {
        foreach(GameObject go in DungeonCameras)
        {
            go.SetActive(false);
        }
        SetCamera();
    }
}
