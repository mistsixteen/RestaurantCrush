using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectFactory
{
    // Start is called before the first frame update
    private GameObject StarParticle, IceParticle, FireParticle;

    private static EffectFactory instance;

    public static EffectFactory GetInstance()
    {
        if (instance == null)
        {
            instance = new EffectFactory();
        }
        return instance;
    }

    public EffectFactory()
    {
        StarParticle = Resources.Load<GameObject>("Prefab/StarParticle");
        IceParticle = Resources.Load<GameObject>("Prefab/IceParticle");
        FireParticle = Resources.Load<GameObject>("Prefab/FireParticle");
    }

    public void MakeFireEffect(Vector3 pos)
    {
        GameObject tempObj = GameObject.Instantiate(FireParticle);

        tempObj.transform.position = new Vector3(pos.x, pos.y, -1.0f);
    }

    public void MakeStarParticleEffect(Vector3 pos, int Num)
    {
        for(int i = 0; i < Num; i++)
        {
            Vector2 MoveVector = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
            MoveVector.Normalize();

            GameObject tempObj = GameObject.Instantiate(StarParticle);

            tempObj.GetComponent<StarParticle>().SetMoveVector(MoveVector);
            tempObj.transform.position = new Vector3(pos.x, pos.y, -1.0f);
        }
    }
    public void MakeIceParticleEffect(Vector3 pos, int Num)
    {
        for (int i = 0; i < Num; i++)
        {
            Vector2 MoveVector = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
            MoveVector.Normalize();

            GameObject tempObj = GameObject.Instantiate(IceParticle);

            tempObj.GetComponent<StarParticle>().SetMoveVector(MoveVector);
            tempObj.transform.position = new Vector3(pos.x, pos.y, -1.0f);
        }
    }
}
