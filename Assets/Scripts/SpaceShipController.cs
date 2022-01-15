using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipController : MonoBehaviour
{
    public float _forwardSpeed = 25f, _strafeSpeed = 7.5f, _hoverSpeed = 5f;
    private float activeForwardSpeed, activeStrafeSpeed, activeHoverSpeed;
    private float forwardAcceleration = 2f, strafeAcceleration = 2f, hoverAcceleration = 2f;

    public float _lookRateSpeed = 90f;
    // Where mouse, distance - how far is the mouse
    private Vector2 lookInput, screenCenter, mouseDistance;

    private float rollInput;
    public float rollSpeed = 90f, rollAccelaration = 3.5f;

    // Start is called before the first frame update
    void Start()
    {
        screenCenter.x = Screen.width / 2;
        screenCenter.y = Screen.height / 2;

        // Keep mouse in screen
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    void Update()
    {
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

        transform.position += transform.forward * activeForwardSpeed * Time.deltaTime;
        transform.position += (transform.right * activeStrafeSpeed * Time.deltaTime) + (transform.up * activeHoverSpeed * Time.deltaTime);

        // Collisions, they work buggy
        Vector3 forward = transform.forward * activeForwardSpeed * Time.deltaTime;
        Vector3 strafe = transform.right * activeStrafeSpeed * Time.deltaTime;
        Vector3 hover = transform.up * activeHoverSpeed * Time.deltaTime;

        Vector3 movement = forward + strafe + hover;
        gameObject.GetComponent<Rigidbody>().MovePosition(transform.position + movement);
    }
}
