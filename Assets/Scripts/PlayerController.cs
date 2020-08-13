using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerController : MonoBehaviour
{

    public GameObject cinemachine;
    private CinemachineFreeLook cam;

    public CharacterController controller;
    public float speed = 6f;


    private Vector2 lookInput;
    private Vector2 moveInput;

    public float turnSmoothTime = .1f;
    private float turnSmoothVelocity;

    public float gravity = -9.81f;
    Vector3 velocity;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = .4f;
    public LayerMask groundMask;

    private bool isGrounded;
    public InputAction jump;

    public InputActionAsset myActions;

    Material beauFace;

    public Texture squint;
    public Texture normal;

    public static int standardScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        controller = this.GetComponent<CharacterController>();
        cinemachine = GameObject.FindGameObjectWithTag("3rdPersonCamera");
        cam = cinemachine.GetComponent<CinemachineFreeLook>();
        //myActions = InputActionAsset.FromJson("ThirdPerson");
        beauFace = GameObject.FindGameObjectWithTag("BeauFace").GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        MoveCamera();
        CheckGrounded();
        SetVelocity();
      
        if(!PauseMenuScript.isPaused && ((Gamepad.current != null && Gamepad.current.buttonSouth.isPressed) || Keyboard.current.spaceKey.isPressed) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            //controller.slopeLimit = 90;
            beauFace.SetTexture("_MainTex", squint);
        }
        else if (!((Gamepad.current != null && Gamepad.current.buttonSouth.isPressed) || Keyboard.current.spaceKey.isPressed))
        {
            //controller.slopeLimit = 50;
            beauFace.SetTexture("_MainTex", normal);
        }
        MovePlayer();
        FallPlayer();
    }

    private void OnLook(InputValue val)
    {
        lookInput = val.Get<Vector2>();
    }

    private void OnMove(InputValue val) => moveInput = val.Get<Vector2>();

    private void MovePlayer()
    {
        Vector3 direction = new Vector3(moveInput.x, 0f, moveInput.y).normalized;

        if (direction.magnitude >= .1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);

        }
    }

    private void MoveCamera()
    {
        cam.m_XAxis.m_InputAxisValue = -lookInput.x;
        cam.m_YAxis.m_InputAxisValue = -lookInput.y;
    }

    private void CheckGrounded()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
    }

    private void SetVelocity()
    {
        if (isGrounded)
        {
            velocity.y = -2f;
        }
    }
    private void FallPlayer()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "StandardCollectible")
        {
            standardScore++;
            other.gameObject.SetActive(false);
        }
    }
}
