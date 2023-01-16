using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerController : MonoBehaviour
{
    //public
    
    //private
    public Camera cam;
    private CharacterController controller;
    private Vector3 playerVelocity, move;
    public bool groundedPlayer, climbable, isClimbing;
    private float playerSpeed = 6.0f;
    private float jumpHeight = 3.0f;
    public float gravityValue = 0, gravityDefault = -9.81f;

    private void Awake()
    {
        if (!controller)
        {
            controller = this.gameObject.GetComponent<CharacterController>();
        }

        if (!cam)
        {
            cam = Camera.main;
        }

        gravityValue = gravityDefault;

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        cam.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 5.5f, this.transform.position.z + 13f);
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        if (!isClimbing)
        {
            gravityValue = gravityDefault;
            move = new Vector3(-Input.GetAxis("Horizontal"), 0, 0);
            controller.Move(move * Time.deltaTime * playerSpeed);
            controller.Move(playerVelocity * Time.deltaTime);
        }
        else
        {
            gravityValue = 0;
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * Input.GetAxis("Vertical"));

            if (Input.GetButtonDown("Jump") || groundedPlayer)
            {
                playerVelocity.y += gravityValue * Time.deltaTime;
                isClimbing = false;
            }
        }

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        if (climbable && Input.GetKeyDown(KeyCode.UpArrow))
        {
            isClimbing = true;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "death_box")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (other.tag == "climbable")
        {
            climbable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "climbable")
        {
            climbable = false;
        }
    }
}
