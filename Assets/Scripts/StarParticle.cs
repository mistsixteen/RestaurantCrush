using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarParticle : MonoBehaviour
{
    // Start is called before the first frame update

    public Vector3 MoveVector;
    public int FrameLeft = 20;

    public float moveSpeed = 0.05f;
    public float gValue = 0.01f;

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
        while(color.a > 0.0f)
        {
            FrameLeft--;
            transform.position += MoveVector * moveSpeed;
            MoveVector.y -= gValue;
            color.a -= 0.05f;
            Renderer.color = color;
            if (FrameLeft <= 0)
                Destroy(this.gameObject);

            yield return new WaitForSeconds(0.05f);
        }
        
    }

}
