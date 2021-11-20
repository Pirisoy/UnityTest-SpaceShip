using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Asteroid : OurOfViewDiabler, IPoolObject
{
    [SerializeField] private AsteroidData data;
    [SerializeField] private AudioClip sfxDestroy;

    public AsteroidType Type => data.type;

    public Rigidbody2D Rb { get; private set; }
    private void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
    }
    // this will calls when asteroid go out of view
    protected override void OutOfView()
    {
        base.OutOfView();

        EnemySpawner.Singelton.BackToPool(data.type, this);
    }
    // this will calls when spawn from pool
    public void OnSpawn()
    {
        Rb.velocity = Vector2.zero;
    }
    public void SetMoveTarget(Vector3 target)
    {
        // move asteroid toward a target. it does not stop after reach its target. it continue its path to go out of view
        Vector2 direction = target - transform.position;
        Vector2 newvector = direction.normalized * Random.Range(data.minMoveSpeed, data.maxMoveSpeed);
        Rb.velocity = newvector;
    }
    public void Hit()
    {
        // spawn childs if have one
        if (data.child != AsteroidType.None)
        {
            EnemySpawner.Singelton.Spawn(data.child, transform);
            EnemySpawner.Singelton.Spawn(data.child, transform);
        }

        GameManager.Singelton.AsteroidDestroyed(data.hitScore, transform);

        SoundManager.Singelton.PlayActionClip(sfxDestroy);

        OutOfView();
    }
}