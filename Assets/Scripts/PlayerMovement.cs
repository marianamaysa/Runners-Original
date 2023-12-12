using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
{
    public Transform personalCamera;
    private bool isInLobby = true;
    [SerializeField] Animator animator;
    public NetworkVariable<int> life =
        new NetworkVariable<int>(3, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

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


    [Header("Movement")]
    public float playerSpeed;
    public float jumpHeight;
    public float gravity;
    [SerializeField] float velocity;

    [Header("Lanes")]
    [SerializeField] float laneDistance = 4; //distancia entre duas lanes
    private int desiredLane = 1; //0:esquerda 1:meio 2:direita
    private float desiredHeight = 1;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInLobby)
        {
            ChangeLane();
        }
    }
    public void IsInLobby(bool value)
    {
        isInLobby = value;
    }

    void ChangeLane()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            desiredLane = Mathf.Max(desiredLane - 1, -1);
            //animator.SetBool("running", true);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            desiredLane = Mathf.Min(desiredLane + 1, 3);
            //animator.SetBool("running", true);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && (desiredLane == -1 || desiredLane == 3))
        {
            desiredHeight = Mathf.Min(desiredHeight + 4, 10);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && (desiredLane == -1 || desiredLane == 3))
        {
            desiredHeight = Mathf.Max(desiredHeight - 4, 1);
        }

        //desiredHeight = Mathf.Clamp(desiredHeight, 2f, 10f);
        if (desiredLane >= 0 && desiredLane <= 2)
        {
            desiredHeight = 2f;
            Vector3 targetPosition = new Vector3((desiredLane - 1) * laneDistance, 0, transform.position.z + (playerSpeed * Time.deltaTime));
            transform.position = Vector3.Lerp(transform.position, targetPosition, 0.1f);

        }
        else if (desiredLane == -1)
        {
            Vector3 targetPosition = new Vector3(-1.70f * laneDistance, desiredHeight, transform.position.z + (playerSpeed * Time.deltaTime));
            transform.position = Vector3.Lerp(transform.position, targetPosition, 0.1f);
        }
        else if (desiredLane == 3)
        {
            Vector3 targetPosition = new Vector3(1.70f * laneDistance, desiredHeight, transform.position.z + (playerSpeed * Time.deltaTime));
            transform.position = Vector3.Lerp(transform.position, targetPosition, 0.1f);
        }
    }
}

