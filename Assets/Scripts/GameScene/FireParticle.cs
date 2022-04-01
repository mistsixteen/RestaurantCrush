using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireParticle : MonoBehaviour
{
    private int FrameLeft = 10;

    private SpriteRenderer Renderer;
    private Coroutine particleCorutine;


    // Start is called before the first frame update
    void Start()
    {
        Renderer = this.GetComponent<SpriteRenderer>();
        particleCorutine = StartCoroutine(ParticleFadeOut());
    }

    IEnumerator ParticleFadeOut()
    {
        Color color = Renderer.color;
        while (FrameLeft >= 0)
        {
            FrameLeft--;
            color.a -= 0.05f;
            Renderer.color = color;

            yield return new WaitForSeconds(0.05f);
        }
        Destroy(this.gameObject);
    }
}
