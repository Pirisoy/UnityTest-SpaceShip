using UnityEngine;

public class OurOfViewDiabler : MonoBehaviour
{
    private float lastCheckOutOfView = 0;

    private void Update()
    {
        if (Time.realtimeSinceStartup - lastCheckOutOfView < 0.1f)
            return;

        lastCheckOutOfView = Time.realtimeSinceStartup;

        if (CheckOutOfView(transform))
        {
            OutOfView();
        }
    }
    protected virtual void OutOfView()
    {
        gameObject.SetActive(false);
    }

    private bool CheckOutOfView(Transform transform)
    {
        var viewPoint = GameManager.Singelton.MainCamera.WorldToViewportPoint(transform.position);

        if (viewPoint.x > 1.2f || viewPoint.x < -.2f)
        {
            return true;
        }

        if (viewPoint.y > 1.2f || viewPoint.y < -.2f)
        {
            return true;
        }

        return false;
    }
}