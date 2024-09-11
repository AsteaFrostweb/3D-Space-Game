using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterCameraController : MonoBehaviour
{
  

    // Update is called once per frame
    void Update()
    {
        //Only allow camera change if in flight focus
        if (GameData.currentFocus.inputFocus != InputFocus.FLIGHT) return;

        if (Input.GetKeyDown(KeyCode.V))
        {
            if (GameData.cameraState == CameraState.FIRST_PERSON)
            {
                GameData.firstPersonCamera.SetActive(false);
                GameData.thirdPersonCamera.SetActive(true);
                GameData.cameraState = CameraState.THIRD_PERSON;
            }
            else if (GameData.cameraState == CameraState.THIRD_PERSON) 
            {
                GameData.thirdPersonCamera.SetActive(false);
                GameData.firstPersonCamera.SetActive(true);
                GameData.cameraState = CameraState.FIRST_PERSON;
            }

        }
    }

    
}
