using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipController : MonoBehaviour
{
    // 3D movement
    // *******************
    public float _forwardSpeed = 25f, _strafeSpeed = 7.5f, _hoverSpeed = 5f;
    private float activeForwardSpeed, activeStrafeSpeed, activeHoverSpeed;
    private float forwardAcceleration = 1f, strafeAcceleration = 1f, hoverAcceleration = 1f;

    public float _lookRateSpeed = 90f;
    // Where mouse, distance - how far is the mouse
    private Vector2 lookInput, screenCenter, mouseDistance;

    private float rollInput;
    public float rollSpeed = 90f, rollAccelaration = 3.5f;
    // ********************

    private int boost = 1;
    private float timer = 0f;

    public HealthBar healthBar;
    public GameObject explosion;

    // Start is called before the first frame update
    void Start()
    {
        screenCenter.x = Screen.width / 2;
        screenCenter.y = Screen.height / 2;

        // Keep mouse in screen
        Cursor.lockState = CursorLockMode.Confined;

        // Health bar setup
        healthBar.SetMaxHealth(100);
    }

    // Update is called once per frame
    void Update()
    {
        if (healthBar.slider.value == 0f)
        {
            // End game
            Instantiate(explosion, transform.position, transform.rotation);
            gameObject.SetActive(false);
            // Unlock the cursor
            Cursor.lockState = CursorLockMode.None;
        }

        lookInput.x = Input.mousePosition.x;
        lookInput.y = Input.mousePosition.y;

        // -1 and 1
        mouseDistance.x = (lookInput.x - screenCenter.x) / screenCenter.y; // Always using the smaller value
        mouseDistance.y = (lookInput.y - screenCenter.y) / screenCenter.y;

        // Limiter: It can only be 1 in length, no matter how much we move our mouse
        mouseDistance = Vector2.ClampMagnitude(mouseDistance, 1f);

        rollInput = Mathf.Lerp(rollInput, Input.GetAxisRaw("Roll"), rollAccelaration * Time.deltaTime);

        // Rotating around self
        transform.Rotate(-mouseDistance.y * _lookRateSpeed * Time.deltaTime, mouseDistance.x * _lookRateSpeed * Time.deltaTime, rollInput * rollSpeed * Time.deltaTime, Space.Self);

        activeForwardSpeed = Mathf.Lerp(Input.GetAxisRaw("Vertical") * _forwardSpeed, forwardAcceleration, Time.deltaTime);
        activeStrafeSpeed = Mathf.Lerp(Input.GetAxisRaw("Horizontal") * _strafeSpeed, strafeAcceleration, Time.deltaTime);
        activeHoverSpeed = Mathf.Lerp(Input.GetAxisRaw("Hover") * _hoverSpeed, hoverAcceleration, Time.deltaTime);

        // Boosting
        if (Input.GetKey(KeyCode.LeftShift))
        {
            // Make a booster bar here
            // healthBar.Damage(5);

            // Time to keep track if space is hit and 2 second boost have passed
            timer += Time.deltaTime;
            if (Input.GetKey(KeyCode.Space) && timer >= 2f) boost = 10;
            else boost = 3;
        }
        else
        {
            boost = 1;
            timer = 0f;
        }

        transform.position += transform.forward * activeForwardSpeed * Time.deltaTime * boost;
        transform.position += (transform.right * activeStrafeSpeed * Time.deltaTime) + (transform.up * activeHoverSpeed * Time.deltaTime);

        // Collisions, they work buggy
        /*
        Vector3 forward = transform.forward * activeForwardSpeed * Time.deltaTime;
        Vector3 strafe = transform.right * activeStrafeSpeed * Time.deltaTime;
        Vector3 hover = transform.up * activeHoverSpeed * Time.deltaTime;

        Vector3 movement = forward + strafe + hover;
        gameObject.GetComponent<Rigidbody>().MovePosition(transform.position + movement);
        */
    }

    // Take damage whenever the player is hit by an asteroid
    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == "Asteroid") 
        {
            healthBar.Damage(5);
        }
    }
}