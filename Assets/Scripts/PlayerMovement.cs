using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
{
    public Transform personalCamera;

    private int desiredLane = 1; //0:esquerda 1:meio 2:direita
    [SerializeField] float laneDistance = 4; //distancia entre duas lanes

    public override void OnNetworkSpawn()
    {
        CinemachineVirtualCamera vcam = personalCamera.gameObject.GetComponent<CinemachineVirtualCamera>();
        if (IsOwner)
        {
            vcam.Priority = 1;
        }
        else
        {
            vcam.Priority = 0;   
        }
    }

    private CharacterController controller;

    public float playerSpeed;
    public float jumpHeight;
    public float gravity;
    private float jumpVelocity;

    private float slideSpeed = 6f;
    private bool isSliding = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

   
    void Update()
    {

        Vector3 direction = Vector3.forward * playerSpeed;

        if (controller.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
            else if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                StartSliding();
            }
        }
        else
        {
            ApplyGravity();
        }

        if (isSliding)
        {
            Slide();
        }
        else
        {
            Move(direction);
        }

        direction.y = jumpVelocity;
        controller.Move(direction * Time.deltaTime);

        //reunir as informações sobre em qual pista devemos estar

        if (Input.GetKeyDown(KeyCode.A))
        {
            desiredLane--;
            if (desiredLane < 0)
            {
                desiredLane = 2;
            }
        }

        else if (Input.GetKeyDown(KeyCode.D))
            {
                desiredLane++;
                if (desiredLane > 2)
                {
                    desiredLane = 0;
                }
            }

        //calcular onde devemos estar no futuro
        
        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;

        if(desiredLane == 0)
        {
            targetPosition += Vector3.left * laneDistance;
        }
        else if(desiredLane == 1)
        {
            
        }
        else if (desiredLane == 2)
        {
            targetPosition += Vector3.right * laneDistance;
        }

        transform.position = Vector3.Lerp(transform.position, targetPosition, 70 * Time.deltaTime);
        
    }

    void Move(Vector3 direction)
    {
        controller.Move(direction * Time.deltaTime);
    }

    void Jump()
    {

    }
}
