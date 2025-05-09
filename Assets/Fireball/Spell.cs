using UnityEngine;

public class Spell : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 5f;
    public int damage = 10;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        // Apply damage or logic
        Debug.Log($"Hit: {other.name}");
        Destroy(gameObject); // destroy on impact
    }
}
