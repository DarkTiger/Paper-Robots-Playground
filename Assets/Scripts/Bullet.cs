using UnityEngine;

public class Bullet : MonoBehaviour
{
    public enum Type { Default, Shotgun, SMG, Sniper }
    public Type type = Type.Default;
    public Sprite weaponSprite = null;
    [SerializeField] float gravityForce = 1;
    public int damage = 1;
    public int startAmmo = 1;
    public float force = 1;
    public float fireDelay = 1;
    public float recoil = 1;
    Planet planet;
    Rigidbody rigidbody;
    float currentLifeTime = 0;
    public PlayerStats owner { get; set; } = null;


    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        planet = FindObjectOfType<Planet>();
        Vector3 randomOffset = new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.05f, 0.05f), 0);
        rigidbody.AddForce((transform.forward + (type == Type.Shotgun? transform.TransformDirection(randomOffset) : Vector3.zero)) * force, ForceMode.VelocityChange);
        transform.Rotate(Vector3.up * Random.Range(-180, 180));
        transform.Rotate(Vector3.right * Random.Range(-180, 180));
        transform.Rotate(Vector3.forward * Random.Range(-180, 180));
    }

    private void Update()
    {
        currentLifeTime += Time.deltaTime;

        if (type == Type.Shotgun)
        {
            if (currentLifeTime > 0.02f)
            {
                GetComponent<SphereCollider>().enabled = true;
            }
        }
    }

    private void FixedUpdate()
    {
        Vector3 direction = (planet.transform.position - transform.position).normalized;
        transform.rotation = Quaternion.FromToRotation(transform.up, -direction) * transform.rotation;
        rigidbody.AddForce(direction * gravityForce, ForceMode.Acceleration);
    }

    private void OnCollisionEnter(Collision collision)
    {
        PlayerStats playerStatus;
        if (playerStatus = collision.collider.GetComponent<PlayerStats>())
        {
            playerStatus.Damage(damage, this);
        }

        if (currentLifeTime > 1f)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.GetComponent<Planet>())
        {
            Destroy(gameObject);
        }
    }
}
