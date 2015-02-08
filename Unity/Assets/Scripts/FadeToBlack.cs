using System.Collections;
using UnityEngine;

public class FadeToBlack : MonoBehaviour
{
    private Material _fadeMaterial = null;
    private bool _fading = false;

    private void Start()
    {
        _fadeMaterial = new Material("Shader \"Plane/No zTest\" { SubShader { Pass { Blend SrcAlpha OneMinusSrcAlpha ZWrite Off Cull Off Fog { Mode Off } BindChannels { Bind \"Color\",color } } } }");
    }

    private void DrawQuad(Color aColor, float aAlpha)
    {
        aColor.a = aAlpha;
        _fadeMaterial.SetPass(0);
        GL.Color(aColor);
        GL.PushMatrix();
        GL.LoadOrtho();
        GL.Begin(GL.QUADS);
        GL.Vertex3(0, 0, -1);
        GL.Vertex3(0, 1, -1);
        GL.Vertex3(1, 1, -1);
        GL.Vertex3(1, 0, -1);
        GL.End();
        GL.PopMatrix();
    }

    private IEnumerator Fade(float aFadeOutTime, float aFadeInTime, Color aColor)
    {
        float t = 0.0f;
        while (t < 1.0f)
        {
            yield return new WaitForEndOfFrame();
            t = Mathf.Clamp01(t + Time.deltaTime / aFadeOutTime);
            DrawQuad(aColor, t);
        }

        while (t > 0.0f)
        {
            yield return new WaitForEndOfFrame();
            t = Mathf.Clamp01(t - Time.deltaTime / aFadeInTime);
            DrawQuad(aColor, t);
        }

        _fading = false;
    }

    public void StartFade(float aFadeOutTime, float aFadeInTime, Color aColor)
    {
        _fading = true;
        StartCoroutine(Fade(aFadeOutTime, aFadeInTime, aColor));
    }
}