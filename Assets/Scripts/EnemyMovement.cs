using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform target;
    public float rotationalDamp = 1f;
    public float speed = 30f;

    public GameObject laserPrefab;
    private float timer = 0;

    private Rigidbody rb;
    public float torque = 500f;
    public float thrust = 1000f;

    public HealthBar playerHealthbar;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        /*
         MOVEMENT
         */
        // transform.LookAt(target); Smart rotation
        Vector3 targetLocation = target.position - transform.position;

        // "Dumb" rotation for "dumber" aiming (dumb = good)
        Quaternion rotation = Quaternion.LookRotation(targetLocation);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationalDamp * Time.deltaTime);

        float distance = targetLocation.magnitude;
        // If enemy is close, it doesn't have thurst, if far it does (min 0, max 10)
        rb.AddRelativeForce(Vector3.forward * Mathf.Clamp((distance - 10) / 50, 0f, 100f) * thrust);

        /*
         RAYCAST
         */
        // If player is in his FOV
        Vector3 directionToTarget = transform.position - target.position;
        float angle = Vector3.Angle(transform.forward, directionToTarget);
        if (Mathf.Abs(angle) > 90 && Mathf.Abs(angle) < 270 && distance <= 300)
        {
            Debug.DrawLine(transform.position, target.position, Color.green);

            int maxRaycastDistance = 300;
            Vector3 position = transform.position;
            // Add to the source of the raycast the forward so that we don't hit the spaceship
            position += transform.forward;
            Debug.DrawRay(position, transform.TransformDirection(Vector3.forward) * maxRaycastDistance, Color.red);

            // Every 2 seconds shoot a raycast
            if (timer >= 2f)
            {
                timer = 0;
                RaycastHit hit;
                Vector3 fwd = transform.TransformDirection(Vector3.forward) * maxRaycastDistance; // 300 - maxDistance
                if (Physics.Raycast(position, fwd, out hit))
                {
                    Debug.Log("We hit: " + hit.transform.name + " with a point: " + hit.point);
                    
                    // Enemy can shoot the Earth >:)
                    // if (hit.transform.name != "EarthHigh")
                    GameObject laser = Instantiate(laserPrefab, transform.position, transform.rotation);
                    laser.GetComponent<ShotBehavior>().SetTarget(hit.transform.position);

                    // Force
                    hit.transform.GetComponent<Rigidbody>().AddForce(transform.forward * 7500f);

                    // Damage the player if hit
                    if (hit.transform.CompareTag("Ship"))
                        playerHealthbar.Damage(10);
                }
                else
                {
                    // Works with SetTarget(target) function (this took me a while :)
                    GameObject laserMiss = Instantiate(laserPrefab, transform.position, transform.rotation);
                    laserMiss.GetComponent<ShotBehavior>().SetTarget(transform.position + transform.forward * maxRaycastDistance);
                    Debug.Log("We missed, shooting laser from " + laserMiss.transform.position + " with rotation " + (transform.forward * maxRaycastDistance));
                }
            }
            timer += Time.deltaTime;

        }
    }
}
