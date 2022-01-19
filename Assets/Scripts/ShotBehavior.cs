using UnityEngine;

public class ShotBehavior : MonoBehaviour
{
    public Vector3 target;
    public GameObject explosion;
    public float speed;

    public void SetTarget(Vector3 tTarget)
    {
        target = tTarget;
    }

    void Update()
    {
        float step = speed * Time.deltaTime;
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, step);

            if (transform.position == target)
            {
                Explode();
                return;
            }
        }
    }

    private void Explode()
    {
        if (explosion != null)
        {
            GameObject tempExplosion = Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject);
            Destroy(tempExplosion, 2f);
        }
    }
}
