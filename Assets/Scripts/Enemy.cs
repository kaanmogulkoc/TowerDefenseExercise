using UnityEngine;

public class Enemy : MonoBehaviour
{
    EnemyFactory originFactory;
    GameTile tileFrom, tileTo;
    Vector3 positionFrom, positionTo;
    float progress, progressFactor;
    Direction direction;
    DirectionChange directionChange;
    float directionAngleTo, directionAngleFrom;
    float pathOffset;
    float speed;

    [SerializeField]
    Transform model = default;

    public EnemyFactory OriginFactory{
        get => originFactory;
        set{
            Debug.Assert(originFactory == null, "Origin Factory Redefined");
            originFactory = value;
        }
    }

    public void Initialize(float scale, float pathOffset, float speed){
        model.localScale = new Vector3(scale, scale, scale);
        this.pathOffset = pathOffset;
        this.speed = speed;
    }

    public void SpawnOn(GameTile spawnPoint){
        //transform.localPosition = spawnPoint.transform.localPosition;
        Debug.Assert(spawnPoint.NextOnPath != null, "Nowhere to go!", this);
        tileFrom = spawnPoint;
        tileTo = spawnPoint.NextOnPath;
        progress = 0f;
        PrepareIntro();
    }

    public bool GameUpdate(){
        progress += Time.deltaTime;
        while(progress >= 1f){
            if(tileTo == null){
                OriginFactory.Reclaim(this);
                return false;
            }
            progress = (progress - 1f) / progressFactor;
            PrepareNextState();
            progress *= progressFactor;
        }
        if(directionChange == DirectionChange.None){
            transform.localPosition = Vector3.LerpUnclamped(positionFrom, positionTo, progress);
        }
        else{
            float angle = Mathf.LerpUnclamped(directionAngleFrom, directionAngleTo, progress);
            transform.localRotation = Quaternion.Euler(0f, angle, 0f);
        }
        return true;
    }

    void PrepareIntro(){
        positionFrom = tileFrom.transform.localPosition;
        positionTo = tileFrom.ExitPoint;
        direction = tileFrom.PathDirection;
        directionChange = DirectionChange.None;
        directionAngleFrom = directionAngleTo = direction.GetAngle();
        model.localPosition = new Vector3(pathOffset, 0f);
        transform.localRotation = direction.GetRotation();
        progressFactor = 2f * speed;
    }

    void PrepareOutro(){
        positionTo = tileFrom.transform.localPosition;
        directionChange = DirectionChange.None;
        directionAngleTo = direction.GetAngle();
        model.localPosition = new Vector3(pathOffset, 0f);
        transform.localRotation = direction.GetRotation();
        progressFactor = 2f * speed;
    }

    void PrepareNextState(){
        tileFrom = tileTo;
        tileTo = tileTo.NextOnPath;
        positionFrom = positionTo;
        if(tileTo == null){
            PrepareOutro();
            return;
        }
        positionTo = tileFrom.ExitPoint;
        directionChange = direction.GetDirectionChangeTo(tileFrom.PathDirection);
        direction = tileFrom.PathDirection;
        directionAngleFrom = directionAngleTo;

        switch (directionChange){
            case DirectionChange.None: PrepareForward(); break;
            case DirectionChange.TurnRight: PrepareTurnRight(); break;
            case DirectionChange.TurnLeft: PrepareTurnLeft(); break;
            default: PrepareTurnAround(); break;
        }
    }

    void PrepareForward(){
        transform.localRotation = direction.GetRotation();
        directionAngleTo = direction.GetAngle();
        model.localPosition = new Vector3(pathOffset, 0f);
        progressFactor = speed;
    }

    void PrepareTurnRight(){
        directionAngleTo = directionAngleFrom + 90f;
        model.localPosition = new Vector3(pathOffset - 0.5f, 0f);
        transform.localPosition = positionFrom + direction.GetHalfVectors();
        progressFactor = speed / (Mathf.PI * 0.5f * (0.5f - pathOffset));
    }
    
    void PrepareTurnLeft(){
        directionAngleTo = directionAngleFrom - 90f;
        model.localPosition = new Vector3(pathOffset + 0.5f, 0f);
        transform.localPosition = positionFrom + direction.GetHalfVectors();
        progressFactor = speed / (Mathf.PI * 0.5f * (0.5f + pathOffset));
    }

    void PrepareTurnAround(){
        directionAngleTo = directionAngleFrom + (pathOffset < 0f ? 180f : -180f);
        model.localPosition = new Vector3(pathOffset, 0f);
        transform.localPosition = positionFrom;
        progressFactor = speed / (Mathf.PI * Mathf.Max(Mathf.Abs(pathOffset), 0.2f));
    }
}
