using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectFactory
{
    // Start is called before the first frame update
    private GameObject StarParticle;

    private static EffectFactory instance;

    public static EffectFactory getInstance()
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
    }

    public void MakeStarParticleEffect(Vector3 pos, int Num)
    {
        for(int i = 0; i < Num; i++)
        {
            Vector2 MoveVector = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
            MoveVector.Normalize();

            //Todo : Make StarParticleEffect
            GameObject tempObj = GameObject.Instantiate(StarParticle);

            tempObj.GetComponent<StarParticle>().SetMoveVector(MoveVector);
            tempObj.transform.position = new Vector3(pos.x, pos.y, -1.0f);
        }
    }
}
