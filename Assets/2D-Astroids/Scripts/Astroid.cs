using UnityEngine;

public class Astroid : MonoBehaviour
{
    public float size = 1f, speed = 50f;
    public float minSize = 0.5f, maxSize = 1.5f;
    private float halfSize;
    public float maxLifeTime = 30.0f;
    private Rigidbody2D _myRigidbody2D;

    private void Awake()
    {
        _myRigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        transform.eulerAngles = new Vector3(0.0f, 0.0f, Random.value * 360.0f);
        transform.localScale = Vector3.one * size;
        _myRigidbody2D.mass = size;
        halfSize = size / 2;
    }

    public void SetTrajectory(Vector2 direction)
    {
        _myRigidbody2D.AddForce(direction * speed);
        Destroy(gameObject, maxLifeTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Bullet")) return;
        if (halfSize >= minSize)
        {
            CreateSplit();
            CreateSplit();
        }
        // Play explosion on death.
        GameManager.instance.AsteroidDestroyed(this);
        Destroy(gameObject);
        
    }

    private void CreateSplit()
    {
        Vector2 position = this.transform.position;
        position += Random.insideUnitCircle / 2;

        Astroid half = Instantiate(this, position, this.transform.rotation);
        half.size = halfSize;
        half.SetTrajectory(Random.insideUnitCircle.normalized * speed * 0.8f);
    }
}
