using UnityEngine;
using System.Collections;

public class RandomRotator : MonoBehaviour
{
    private float minTumble = 1f;
    private float maxTumble = 3f;

    void Start()
    {
        GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * Random.Range(minTumble, maxTumble);
    }

    void Update()
    {
    }
}