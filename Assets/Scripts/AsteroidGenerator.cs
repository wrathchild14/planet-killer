using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidGenerator : MonoBehaviour
{
    public Transform _astroid1Prefab;
    public Transform _astroid2Prefab;
    public Transform _astroid3Prefab;
    public int _fieldRadius = 100;
    public int _asteroidCount = 500;

    private Transform temp;

    void Start()
    {
        // Generating astroids, TODO: change into child of the field
        for (int i = 0; i < _asteroidCount; i++)
        {
            int choice = Random.Range(0, 3);
            switch (choice)
            {
                case 0:
                    // Addition so they spawn around the astroid field vector
                    temp = Instantiate(_astroid1Prefab,
                                       (Random.insideUnitSphere * _fieldRadius) + gameObject.transform.position,
                                       Quaternion.identity);
                    temp.parent = gameObject.transform;
                    break;
                case 1:
                    temp = Instantiate(_astroid2Prefab,
                                       (Random.insideUnitSphere * _fieldRadius) + gameObject.transform.position,
                                       Quaternion.identity);
                    temp.parent = gameObject.transform;
                    break;
                case 2:
                    temp = Instantiate(_astroid3Prefab,
                                       (Random.insideUnitSphere * _fieldRadius) + gameObject.transform.position,
                                       Quaternion.identity);
                    temp.parent = gameObject.transform;
                    break;
            }
            // Perfect random size generator
            Vector3 randomSize = new Vector3(Random.Range(1f, 10f), Random.Range(1f, 10f), Random.Range(1f, 10f));
            temp.gameObject.transform.localScale = randomSize;
        }
    }
    
    void Update()
    {
        
    }
}
