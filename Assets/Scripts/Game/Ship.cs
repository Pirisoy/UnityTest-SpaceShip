using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Weapon))]
public class Ship : MonoBehaviour
{
    //SerializeFields
    [SerializeField] protected ShipData currentShip;
    [SerializeField] protected GameObject vfxMove;
    [SerializeField] protected AudioSource sourceEngin;
    [SerializeField] protected AudioClip sfxDestroy;

    // components
    protected Rigidbody2D rb;
    protected Vector2 moveDirection;

    public ShipData GetShipData() => currentShip;


    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected void Update()
    {
        Move();
        Rotate();
    }
    protected void LateUpdate()
    {
        CheckOutOfView();
    }

    protected virtual void Move()
    {
    }
    protected virtual void Rotate()
    {
        if (moveDirection == Vector2.zero)
            return;

        var newRotation = Quaternion.LookRotation(moveDirection, Vector3.back);
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
    public void ResetVelocity()
    {
        vfxMove.SetActive(false);
        moveDirection = Vector2.zero;
        rb.velocity = Vector2.zero;
    }
}
