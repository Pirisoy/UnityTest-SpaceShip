using UnityEngine;

[CreateAssetMenu(fileName = "New Ship", menuName = "Objects/Ship")]
public class ShipData : ScriptableObject
{
    public float moveSpeed = 10;
    public float rotateSpeed = 1000;
    public float fireRate = 1;
    public float bulletForce = 10;
}