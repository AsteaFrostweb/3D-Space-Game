using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCameraController : MonoBehaviour
{
    public Transform player;
    public float PitchYawSmooth = 3f;
    public float RollSmooth = 2f;
    public Vector2 cameraPanSpeed = Vector2.zero * 3f;
    public Vector2 minMaxFOV = Vector2.zero;
    public float scrollSpeed = 10f;
    public Camera cam;

    private Vector2 xyOffset = Vector2.zero;
    private Vector2 xyDelta = Vector2.zero;
    private float scrollDelta = 0f;


    void Start()
    {
        GameData.firstPersonCamera = gameObject;
        xyOffset = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0f);

        if (minMaxFOV == Vector2.zero) minMaxFOV = new Vector2(10, 80);

        gameObject.SetActive(false);
    }

    private void Update()
    {

        if (Input.GetKey(KeyCode.LeftAlt))
        {
            GameData.playerRotationInputLocked = true;
            xyDelta.x += -Input.GetAxis("Mouse Y") * cameraPanSpeed.y * Time.deltaTime;
            xyDelta.y += Input.GetAxis("Mouse X") * cameraPanSpeed.x * Time.deltaTime;
            scrollDelta += Input.mouseScrollDelta.y * scrollSpeed * Time.deltaTime;
        }
        else GameData.playerRotationInputLocked = false;
    }

    void FixedUpdate()
    {    

        xyOffset += xyDelta * Time.fixedDeltaTime;
        xyDelta = Vector2.zero;
        transform.localRotation = Quaternion.Euler(xyOffset.x, xyOffset.y, 0f);

        float newFOV = cam.fieldOfView - scrollDelta;
        scrollDelta = 0f;

        newFOV = Mathf.Clamp(newFOV, minMaxFOV.x, minMaxFOV.y);
        cam.fieldOfView =newFOV;

    }
}
