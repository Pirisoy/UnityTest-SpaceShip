using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : OurOfViewDiabler, IPoolObject
{
    public Rigidbody2D Rb { get; private set; }

    private PlayerWeapon weapon;
    private float shootTime;

    // this will calls when spawn from pool
    public void OnSpawn()
    {
        Rb.velocity = Vector2.zero;
    }

    private void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
    }
    // this will calls when asteroid go out of view
    protected override void OutOfView()
    {
        base.OutOfView();

        weapon.BackBulletToPull(this);
    }
    public void SetWeapon(PlayerWeapon weapon)
    {
        this.weapon = weapon;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "asteroid")
        {
            OutOfView();

            collision.gameObject.GetComponent<Asteroid>().Hit();
        }
        else if (collision.gameObject.tag == "playerShip" && (Time.realtimeSinceStartup - shootTime) > 0.1f)
        {
            OutOfView();

            GameManager.Singelton.PlayerGotHit();
        }
    }
    public void Shoot(Vector2 side, float force)
    {
        shootTime = Time.realtimeSinceStartup;

        Rb.AddForce(side * force, ForceMode2D.Impulse);
    }
}