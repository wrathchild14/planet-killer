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

    // Start is called before the first frame update
    void Start()
    {
        // Generating astroids, TODO: change into child of the field
        for (int i = 0; i < _asteroidCount; i++)
        {
            int choice = Random.Range(0, 4);
            switch (choice)
            {
                case 0:
                    temp = Instantiate(_astroid1Prefab, Random.insideUnitSphere * _fieldRadius, Quaternion.identity);
                    temp.parent = gameObject.transform;
                    break;
                case 1:
                    temp = Instantiate(_astroid1Prefab, Random.insideUnitSphere * _fieldRadius, Quaternion.identity);
                    temp.parent = gameObject.transform;
                    break;
                case 2:
                    temp = Instantiate(_astroid1Prefab, Random.insideUnitSphere * _fieldRadius, Quaternion.identity);
                    temp.parent = gameObject.transform;
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
