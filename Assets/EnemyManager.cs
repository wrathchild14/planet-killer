using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public Transform enemy;
    public int fieldRadius = 100;
    public int enemyCount = 10;

    private Transform temp;
    void Start()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            temp = Instantiate(enemy,
                               (Random.insideUnitSphere * fieldRadius) + gameObject.transform.position,
                               Quaternion.identity);
            temp.parent = gameObject.transform;
        }
    }

    void Update()
    {
        /*if (gameObject.transform.childCount < 4)
        {
            for (int i = 0; i < Random.Range(3, 8); i++)
            {
                // Addition so they spawn around the astroid field vector
                temp = Instantiate(enemy,
                                   (Random.insideUnitSphere * fieldRadius) + gameObject.transform.position,
                                   Quaternion.identity);
                temp.parent = gameObject.transform;
            }
        }*/
    }
}
