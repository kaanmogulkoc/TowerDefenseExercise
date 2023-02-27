using UnityEngine;

public class Enemy : MonoBehaviour
{
    EnemyFactory originFactory;
    GameTile tileFrom, tileTo;
    Vector3 positionFrom, positionTo;
    float progress;

    public EnemyFactory OriginFactory{
        get => originFactory;
        set{
            Debug.Assert(originFactory == null, "Origin Factory Redefined");
            originFactory = value;
        }
    }

    public void SpawnOn(GameTile spawnPoint){
        //transform.localPosition = spawnPoint.transform.localPosition;
        Debug.Assert(spawnPoint.NextOnPath != null, "Nowhere to go!", this);
        tileFrom = spawnPoint;
        tileTo = spawnPoint.NextOnPath;
        positionFrom = tileFrom.transform.localPosition;
        positionTo = tileFrom.ExitPoint;
        progress = 0f;
    }

    public bool GameUpdate(){
        progress += Time.deltaTime;
        while(progress >= 1f){
            tileFrom = tileTo;
            tileTo = tileTo.NextOnPath;
            if(tileTo == null){
                OriginFactory.Reclaim(this);
                return false;
            }
            positionFrom = positionTo;
            positionTo = tileFrom.ExitPoint;
            progress -= 1f;
        }
        transform.localPosition = Vector3.LerpUnclamped(positionFrom, positionTo, progress);
        return true;
    }
}
