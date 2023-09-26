using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPoint : MonoBehaviour
{
    public Enemy enemy {get; private set;}
    public Vector3 position => transform.position;
    void Awake()
    {
        enemy = transform.root.GetComponent<Enemy>();
        Debug.Assert(enemy != null, "Target point without Enemy!", this);
        Debug.Assert(GetComponent<SphereCollider>() != null, "Target point without Sphere Collider!", this);
        Debug.Assert(gameObject.layer == 9, "Target point on wrong layer!", this);
    }

}
