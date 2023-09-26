using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTileContent : MonoBehaviour
{
    [SerializeField]
    GameTileContentType type = default;
    GameTileContentFactory originFactory;

    public virtual void GameUpdate(){}

    public GameTileContentType Type => type;

    public GameTileContentFactory OriginFactory{
        get => originFactory;
        set{
            Debug.Assert(originFactory == null, "Redefined Origin Factory");
            originFactory = value;
        }
    }

    public bool BlocksPath => Type == GameTileContentType.Wall || Type == GameTileContentType.Tower;

    public void Recycle(){
        originFactory.Reclaim(this);
    }
}
