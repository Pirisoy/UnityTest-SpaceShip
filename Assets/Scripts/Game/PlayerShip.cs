using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerWeapon))]
public class PlayerShip : MonoBehaviour
{
    //SerializeFields
    [SerializeField] private ShipData currentShip;
    [SerializeField] private GameObject vfxMove;
    [SerializeField] private AudioSource sourceEngin;
    [SerializeField] private AudioClip sfxDestroy;

    public ShipData GetShipData() => currentShip;
    // private data
    private Vector2 moveInputData;

    // components
    private Rigidbody2D rb;

    public PlayerWeapon Weapon { get; private set; }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Weapon = GetComponent<PlayerWeapon>();
    }
    private void Update()
    {
        Move();
        Rotate();
    }
    private void LateUpdate()
    {
        CheckOutOfView();
    }
    private void Move()
    {
        if (moveInputData == Vector2.zero)
            return;

        rb.velocity += moveInputData * currentShip.moveSpeed * Time.deltaTime;
    }
    private void Rotate()
    {
        if (moveInputData == Vector2.zero)
            return;

        var newRotation = Quaternion.LookRotation(moveInputData, Vector3.back);
        newRotation.x = 0f;
        newRotation.y = 0f;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, currentShip.rotateSpeed * Time.deltaTime);
    }
    private float lastCheckOutOfView = 0;
    private void CheckOutOfView()
    {
        if (Time.realtimeSinceStartup - lastCheckOutOfView < 0.1f)
            return;

        var viewPoint = GameManager.Singelton.MainCamera.WorldToViewportPoint(transform.position);

        if (viewPoint.x > 1.05f || viewPoint.x < -.05f)
        {
            transform.position = new Vector3(transform.position.x * -1, transform.position.y);
            lastCheckOutOfView = Time.realtimeSinceStartup;
        }

        if (viewPoint.y > 1.05f || viewPoint.y < -.05f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y * -1);
            lastCheckOutOfView = Time.realtimeSinceStartup;
        }
    }
    public void SetInput(Vector2 input)
    {
        moveInputData = input;

        vfxMove.SetActive(input != Vector2.zero);
        if (input != Vector2.zero)
        {
            if (!sourceEngin.isPlaying)
                sourceEngin.Play();
        }
        else
            sourceEngin.Pause();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "asteroid")
        {
            SoundManager.Singelton.PlayActionClip(sfxDestroy);
            GameManager.Singelton.PlayerGotHit();
        }
    }
    public void ResetVelocity()
    {
        vfxMove.SetActive(false);
        moveInputData = Vector2.zero;
        rb.velocity = Vector2.zero;
    }
}