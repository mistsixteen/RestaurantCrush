using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarParticle : MonoBehaviour
{
    // Start is called before the first frame update

    public Vector3 MoveVector;
    public int FrameLeft = 3000;

    public float moveSpeed = 0.005f;
    public float gValue = 0.001f;

    void Start()
    {

    }

    void FixedUpdate()
    {
        FrameLeft--;
        transform.position += MoveVector * moveSpeed;
        MoveVector.y -= gValue;

        if (FrameLeft <= 0)
            Destroy(this.gameObject);
    }

    public void SetMoveVector(Vector2 mov)
    {
        MoveVector = new Vector3(mov.x, mov.y, 0.0f);
    }

}
