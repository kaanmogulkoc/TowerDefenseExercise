using UnityEngine;

public class Enemy : MonoBehaviour
{
    EnemyFactory originFactory;

    public EnemyFactory OriginFactory{
        get => originFactory;
        set{
            Debug.Assert(originFactory == null, "Origin Factory Redefined");
            originFactory = value;
        }
    }

    public void SpawnOn(GameTile spawnPoint){
        transform.localPosition = spawnPoint.transform.localPosition;
    }

    public bool GameUpdate(){
        transform.localPosition += Vector3.forward * Time.deltaTime;
        return true;
    }
}
