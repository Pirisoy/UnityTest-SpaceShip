using UnityEngine;

public class PlayerShip : Ship
{
    public Weapon Weapon { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        Weapon = GetComponent<Weapon>();
    }
    protected override void Move()
    {
        if (moveDirection == Vector2.zero)
            return;

        rb.velocity += moveDirection * currentShip.moveSpeed * Time.deltaTime;
    }
    public void SetInput(Vector2 input)
    {
        moveDirection = input;

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
}