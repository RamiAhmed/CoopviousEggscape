using System.Collections;
using UnityEngine;

public class ScreenShaker : MonoBehaviour
{
    public float angle = 0.8f;
    public float speed = 0.05f;

    private bool _shaking;

    public void ShakeScreen(int duration)
    {
        StartCoroutine(shakeScreen(duration));
    }

    private IEnumerator shakeScreen(int duration)
    {
        if (_shaking)
        {
            yield break;
        }

        _shaking = true;
        this.transform.Rotate(0f, 0f, -angle);

        for (int i = 1; i <= duration; i++)
        {
            this.transform.Rotate(0f, 0f, 2f * angle);
            yield return new WaitForSeconds(speed);

            this.transform.Rotate(0f, 0f, 2f * angle);
            yield return new WaitForSeconds(speed);
        }

        this.transform.Rotate(0f, 0f, angle);
        _shaking = false;
        yield break;
    }
}