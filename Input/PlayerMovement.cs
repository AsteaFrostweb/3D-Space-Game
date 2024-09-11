using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Range(-1f,1f)]
    public float simMouseX = 0f;
    [Range(-1f, 1f)]
    public float simMouseY = 0f;

    public Vector3 acceleration;

    public Vector2 mouseSensitivity = Vector2.one;
    public float rollSensitivity = 1f;  
    public bool flipY = false;

    public Transform ship;
    public TextMeshProUGUI velocityText;
    public Vector3 shipTiltDampening = Vector3.one * 10f;
    public Vector3 shipTiltFactor = Vector3.one;
    public Vector3 shipMaxTilt = Vector3.one * 45f;
    private Vector3 shipEulers = Vector3.zero;

    private Vector3 trans_impulse = new Vector3();
    private Vector3 torque_impulse = new Vector3();
    private Rigidbody rb;
 
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void HandleTranslationInput()
    {
        float xInput = Input.GetAxis("Horizontal");       
        float zInput = Input.GetAxis("Vertical");
        //Debug.Log("Horizontal: " + xInput + "  Vertical: " + zInput);

        trans_impulse += xInput * transform.right * acceleration.x * Time.deltaTime;
        trans_impulse += zInput * transform.forward * acceleration.z * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space))
        {
            trans_impulse += transform.up * acceleration.y * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            trans_impulse += -transform.up * acceleration.y * Time.deltaTime;
        }
    }


    public void HandleRotationInput()   
    {
        float sign = flipY ? -1 : 1;
        // Get mouse delta input for rotation
        float mouseX = (simMouseX + Input.GetAxis("Mouse X")) * mouseSensitivity.x * Time.deltaTime;
        float mouseY = (simMouseY + Input.GetAxis("Mouse Y")) * mouseSensitivity.y * sign * Time.deltaTime;

        //apply mouse x and y to the torque impulse used to rotate the player
        torque_impulse += transform.up * mouseX; //torque around up axis by amount mouseX
        torque_impulse += transform.right * mouseY;

     

        if (Input.GetKey(KeyCode.Q))
        {
            torque_impulse += transform.forward * Time.deltaTime * rollSensitivity;
        }
        if (Input.GetKey(KeyCode.E))
        {
            torque_impulse -= transform.forward * Time.deltaTime * rollSensitivity;
        }

   
    }

    private void ApplyForceDeltas(float time)
    {

        //Debug.Log("Applying torque of: " + torque_impulse.ToString());
        rb.AddTorque(torque_impulse * time, ForceMode.Force);
        //Debug.Log("Applying force of: " + trans_impulse.ToString());
        rb.AddForce(trans_impulse * time, ForceMode.Force);

        trans_impulse = new Vector3();
        torque_impulse = new Vector3();
    }



    // Update is called once per frame
    void Update()
    {
        if (GameData.currentFocus.inputFocus != InputFocus.FLIGHT) return;

        HandleTranslationInput();

        if (!GameData.playerRotationInputLocked) 
        {
            HandleRotationInput();
        }
    }

    

    private void FixedUpdate()
    {
        ApplyForceDeltas(Time.fixedDeltaTime);
        velocityText.text = "velocity: " + (Mathf.Round(rb.velocity.magnitude * 100) / 10).ToString() + " m/s";
    }

 
}