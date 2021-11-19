using UnityEngine;

public class Astroid : MonoBehaviour
{
    public float size = 1f;
    public float speed = 50f;
    public float minSize = 0.5f;
    public float maxSize = 1.5f;
    public float maxLifetime = 30.0f;

    private SpriteRenderer _mySpriteRenderer;
    private Rigidbody2D _myRigidbody2D;

    private void Awake()
    {
        _mySpriteRenderer = GetComponent<SpriteRenderer>();
        _myRigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        this.transform.eulerAngles = new Vector3(0.0f, 0.0f, Random.value * 360.0f);
        this.transform.localScale = Vector3.one * this.size;
        _myRigidbody2D.mass = this.size;
    }

    public void SetTrajectory(Vector2 direction)
    {
        _myRigidbody2D.AddForce(direction * speed);
        // Destroy this after 'maxLifetime'.
        Destroy(this.gameObject, this.maxLifetime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            if ((this.size * 0.5f) >= this.minSize)
            {
                CreateSplit();
                CreateSplit();
            }
            // Play explosion on death.
            GameManager.instance.AstroidDestroyed(this);
            Destroy(this.gameObject);
        }
    }

    private void CreateSplit()
    {
        Vector2 position = this.transform.position;
        position += Random.insideUnitCircle * 0.5f;

        Astroid half = Instantiate(this, position, this.transform.rotation);
        half.size = this.size * 0.5f;
        half.SetTrajectory(Random.insideUnitCircle.normalized * this.speed * 0.8f);
    }

}
