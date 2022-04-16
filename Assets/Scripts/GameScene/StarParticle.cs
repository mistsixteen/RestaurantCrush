using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarParticle : MonoBehaviour
{
    // Start is called before the first frame update

    private Vector3 moveVector;
    
    private int frameLeft = 20;
    private float moveSpeed = 0.45f;
    private float gValue = 0.15f;

    private SpriteRenderer Renderer;
    private Coroutine corutineFadeOut;

    void Start()
    {
        Renderer = this.GetComponent<SpriteRenderer>();

        corutineFadeOut = StartCoroutine(FadeOutRoutine());
    }


    public void SetMoveVector(Vector2 mov)
    {
        moveVector = new Vector3(mov.x, mov.y, 0.0f);
    }

    IEnumerator FadeOutRoutine()
    {
        Color myColor = Renderer.color;
        while(frameLeft >= 0)
        {
            frameLeft--;

            transform.position += moveVector * moveSpeed;
            moveVector.y -= gValue;
            myColor.a -= 0.05f;
            Renderer.color = myColor;

            yield return new WaitForSeconds(0.05f);
        }

        Destroy(this.gameObject);

    }

}
