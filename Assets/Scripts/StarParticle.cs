using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarParticle : MonoBehaviour
{
    // Start is called before the first frame update

    private Vector3 MoveVector;
    
    private int FrameLeft = 20;
    private float moveSpeed = 0.45f;
    private float gValue = 0.15f;

    private SpriteRenderer Renderer;
    private Coroutine particleCorutine;

    void Start()
    {
        Renderer = this.GetComponent<SpriteRenderer>();

        particleCorutine = StartCoroutine(ParticleFadeOut());
    }


    public void SetMoveVector(Vector2 mov)
    {
        MoveVector = new Vector3(mov.x, mov.y, 0.0f);
    }

    IEnumerator ParticleFadeOut()
    {
        Color color = Renderer.color;
        while(FrameLeft >= 0)
        {
            FrameLeft--;

            transform.position += MoveVector * moveSpeed;
            MoveVector.y -= gValue;
            color.a -= 0.05f;
            Renderer.color = color;

            yield return new WaitForSeconds(0.05f);
        }

        Destroy(this.gameObject);

    }

}
