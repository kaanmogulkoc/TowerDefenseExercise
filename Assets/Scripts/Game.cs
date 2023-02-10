using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField]
    Vector2Int boardSize = new Vector2Int(11,11);

    [SerializeField]
    GameBoard board = default;

    [SerializeField]
    GameTileContentFactory gameTileContentFactory = default;

    [SerializeField]
    EnemyFactory enemyFactory = default;

    [SerializeField, Range(0.1f, 10f)]
    float enemySpawnRate = 1f;

    float spawnProgress;

    EnemyCollection enemies = new EnemyCollection();

    Ray TouchRay => Camera.main.ScreenPointToRay(Input.mousePosition);

    void Awake(){
        board.Initialize(boardSize, gameTileContentFactory);
        board.ShowGrid = true;
        board.ShowPaths = false;
    }

    void Update(){
        if(Input.GetMouseButtonDown(0)){
            HandleTouch();
        }
        else if(Input.GetMouseButtonDown(1)){
            HandleAlternativeTouch();
        }

        if(Input.GetKeyDown(KeyCode.V)){
            board.ShowPaths = !board.ShowPaths;
        }

        if(Input.GetKeyDown(KeyCode.G)){
            board.ShowGrid = !board.ShowGrid;
        }

        spawnProgress += enemySpawnRate * Time.deltaTime;
        while(spawnProgress >= 1f){
            spawnProgress -= 1f;
            SpawnEnemy();
        }
        enemies.GameUpdate();
    }

    void OnValidate(){
        if(boardSize.x < 2)
            boardSize.x = 2;

        if(boardSize.y < 2)
            boardSize.y = 2;
    }

    void HandleTouch(){
        GameTile tile = board.GetTile(TouchRay);
        if(tile != null){
            board.ToggleWall(tile);
        }
    }

    void HandleAlternativeTouch(){
        GameTile tile = board.GetTile(TouchRay);
        if(tile != null){
            if(Input.GetKey(KeyCode.LeftShift)){
                board.ToggleDestination(tile);
            }
            else{
                board.ToggleSpawnPoint(tile);
            }
        }
    }

    void SpawnEnemy(){
        GameTile spawnPoint = board.GetSpawnPoint(Random.Range(0, board.SpawnPointCount));
        Enemy enemy = enemyFactory.Get();
        enemy.SpawnOn(spawnPoint);
        enemies.Add(enemy);
    }
}
