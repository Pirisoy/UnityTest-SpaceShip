using UnityEngine;

public class MyVfx : MonoBehaviour, IPoolObject
{
    private void OnEnable()
    {
        Invoke(nameof(Disable), 1);
    }
    private void Disable()
    {
        gameObject.SetActive(false);

        GameManager.Singelton.BackVfxToPool(this);
    }
    public void OnSpawn()
    {
    }
}