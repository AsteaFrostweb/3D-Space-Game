using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCameraController : MonoBehaviour
{
    public Transform player;     
    public Transform subHolder; 
    public Transform Camera;
    public float PitchYawSmooth = 3f; 
    public float RollSmooth = 2f;
    public Vector2 cameraPanSpeed = Vector2.zero * 3f;
    public float cameraScrollSpeed = 2f;
    public Vector2 cameraMinMaxSCroll;
    private Vector2 xyOffset = Vector2.zero;
    private Vector2 xyDelta = Vector2.zero;
    private float scrollDelta;
    private Rigidbody playerRb;   

    void Start()
    {
        //If they haven't initialized the minmax scroll give it some default values
        if (cameraMinMaxSCroll == Vector2.zero) cameraMinMaxSCroll = new Vector2(5f, 30f);

        //Assign Gamedata varaible
        GameData.thirdPersonCamera = gameObject;

        //Get Initial camera offset
        xyOffset = new Vector3(subHolder.localEulerAngles.x, subHolder.localEulerAngles.y, 0f);
        playerRb = player.GetComponent<Rigidbody>();
    }

    private void Update()
    {
    
      

        if (Input.GetKey(KeyCode.LeftAlt))
        {
            GameData.playerRotationInputLocked = true;
            xyDelta .x += -Input.GetAxis("Mouse Y") * cameraPanSpeed.y * Time.deltaTime;
            xyDelta.y += Input.GetAxis("Mouse X") * cameraPanSpeed.x * Time.deltaTime; 

            scrollDelta += Input.mouseScrollDelta.y * cameraScrollSpeed * Time.deltaTime;
        }
        else GameData.playerRotationInputLocked = false;
    }

    void FixedUpdate()
    {
        if (player) transform.position = player.transform.position;
        if (playerRb != null)
        {   
            // Calculate the target rotation with the same forward axis as the player and the up axis of the current transform.
            Quaternion targetForward = Quaternion.LookRotation(player.transform.forward, transform.up);   
            //Rotate towards the players ship but keepdddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddd our current up axis
            transform.rotation = Quaternion.Slerp(transform.rotation, targetForward, Time.fixedDeltaTime * PitchYawSmooth);
            //Create a target rotation of OUR CURRENT forward and the PLAYERS up transform so we can alioght ourselves to their up axis with sa seperate scalar.
            Quaternion targetUp = Quaternion.LookRotation(transform.forward, player.transform.up);
            //Brotato Potato
            transform.rotation = Quaternion.Slerp(transform.rotation, targetUp, Time.fixedDeltaTime * RollSmooth);
        }

        xyOffset += xyDelta * Time.fixedDeltaTime;
        xyDelta = Vector2.zero;
        subHolder.localRotation = Quaternion.Euler(xyOffset.x, xyOffset.y, 0f);




        if (Mathf.Abs(scrollDelta) > 0f)
        {
            Debug.Log("ScrollDelta: " + scrollDelta.ToString());      }

        float newZ = Camera.transform.localPosition.z + scrollDelta;
        scrollDelta = 0f;

        newZ = Mathf.Clamp(newZ, -cameraMinMaxSCroll.y, -cameraMinMaxSCroll.x);
        Camera.transform.localPosition = new Vector3(Camera.transform.localPosition.x, Camera.transform.localPosition.y, newZ);
       
    }


}
