using System;
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
    public GameObject laserPrefab;

    void Start()
    {
        screenCenter.x = Screen.width / 2;
        screenCenter.y = Screen.height / 2;

        // Keep mouse in screen
        Cursor.lockState = CursorLockMode.Confined;

        // Health bar setup
        healthBar.SetMaxHealth(100);
    }

    void Update()
    {
        if (healthBar.slider.value == 0f) EndGame();

        /*
         * RAYCAST (Lasers go brr..)
         */
        int maxRaycastDistance = 300;
        Vector3 position = transform.position;
        // Add to the source of the raycast the forward so that we don't hit the spaceship
        position += transform.forward;
        Debug.DrawRay(position, transform.TransformDirection(Vector3.forward) * maxRaycastDistance, Color.cyan);
        // Mouse button down for one ray cast at a click of a button
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Vector3 fwd = transform.TransformDirection(Vector3.forward) * maxRaycastDistance; // 300 - maxDistance
            if (Physics.Raycast(position, fwd, out hit))
            {
                Debug.Log("We hit: " + hit.transform.name + " with a point: " + hit.point);
                // Destroy(hit.transform.gameObject);

                // Laser
                GameObject laser = Instantiate(laserPrefab, transform.position, transform.rotation);
                laser.GetComponent<ShotBehavior>().SetTarget(hit.transform.position);

                // Force
                hit.transform.GetComponent<Rigidbody>().AddForce(transform.forward * 1000f);
            }
            else
            {
                // Works with SetTarget(target) function

                GameObject laserMiss = Instantiate(laserPrefab, transform.position, transform.rotation);
                laserMiss.GetComponent<ShotBehavior>().SetTarget((transform.position + transform.forward));
                Debug.Log("We missed, shooting laser from " + laserMiss.transform.position + " with rotation " + (transform.forward * maxRaycastDistance));

            }
        }

        /*
         * MOVEMENT
         */

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

        // Don't use collisions for now
        // Collisions, they work buggy
        /*
        Vector3 forward = transform.forward * activeForwardSpeed * Time.deltaTime * boost;
        Vector3 strafe = transform.right * activeStrafeSpeed * Time.deltaTime;
        Vector3 hover = transform.up * activeHoverSpeed * Time.deltaTime;

        Vector3 movement = forward + strafe + hover;
        gameObject.GetComponent<Rigidbody>().MovePosition(transform.position + movement);
        */
    }

    private void EndGame()
    {
        Instantiate(explosion, transform.position, transform.rotation);
        gameObject.SetActive(false);
        // Unlock the cursor
        Cursor.lockState = CursorLockMode.None;
        // Apply explosion force
        float radius = 100f;
        float power = 1000f;
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
                rb.AddExplosionForce(power, explosionPos, radius, 3.0F);
        }
    }

    // Take damage whenever the player is hit by an asteroid
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Asteroid")
        {
            healthBar.Damage(1);
        }
    }
}