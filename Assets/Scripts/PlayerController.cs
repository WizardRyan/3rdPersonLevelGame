using Cinemachine;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
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


    public bool isFollowingObject = false;
    private bool isJumping = false;

    private GameObject followObject = null;
    private GameObject tempFollowObject = null;

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
        Jump();
        MovePlayer();
        FallPlayer();
    }

    private void OnLook(InputValue val)
    {
        lookInput = val.Get<Vector2>();
    }

    private void OnMove(InputValue val) => moveInput = val.Get<Vector2>();

    private void Jump()
    {
        if (!PauseMenuScript.isPaused && ((Gamepad.current != null && Gamepad.current.buttonSouth.isPressed) || Keyboard.current.spaceKey.isPressed) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            //controller.slopeLimit = 90;
            beauFace.SetTexture("_MainTex", squint);
            isJumping = true;
        }
        else if (!((Gamepad.current != null && Gamepad.current.buttonSouth.isPressed) || Keyboard.current.spaceKey.isPressed))
        {
            //controller.slopeLimit = 50;
            beauFace.SetTexture("_MainTex", normal);
            isJumping = false;
        }
    }

    private void MovePlayer()
    {
        if(isFollowingObject)
        {
            FollowObject();
        }

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
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask) || isFollowingObject;
    }

    private void SetVelocity()
    {
        if (isGrounded)
        {
            velocity.y = 0f;
        }
    }
    private void FallPlayer()
    {
        if (!isFollowingObject || isJumping)
        {
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "StandardCollectible")
        {
            standardScore++;
            other.gameObject.SetActive(false);
        }

        else if(other.gameObject.tag == "MovingPlatform")
        {
            SetupFollowObject(other.gameObject);
        }
       
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "MovingPlatform")
        {
            StopFollowingObject();
        }
    }

    private void SetupFollowObject(GameObject other)
    {
        followObject = other;

        //setting up a temporary object so we only follow position and rotation, but not scale
        tempFollowObject = new GameObject();
        tempFollowObject.transform.SetPositionAndRotation(followObject.transform.position, followObject.transform.rotation);
        tempFollowObject.transform.SetParent(followObject.transform);
        transform.SetParent(tempFollowObject.transform);
        isFollowingObject = true;
    }

    private void StopFollowingObject()
    {
        followObject = null;
        tempFollowObject = null;
        transform.SetParent(null);
        isFollowingObject = false;
    }

    private void FollowObject()
    {
        tempFollowObject.transform.SetPositionAndRotation(followObject.transform.position, followObject.transform.rotation);
    }
}
