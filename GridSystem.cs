using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GridSystem<TGridType>{

  private int width;
  private int height;
  private TGridType[,] gridArray;
  private float cellSize;
  private Vector3 originPosition;

  public GridSystem(int width, int height, float cellSize, Vector3 originPosition, System.Func<TGridType> createGridType){
    this.width = width;
    this.height = height;
    this.cellSize = cellSize;
    this.originPosition = originPosition;

    gridArray = new TGridType[width, height];

    for(int x=0; x< gridArray.GetLength(0); x++){
      for(int y=0; y < gridArray.GetLength(1); y++){
        gridArray[x,y] = createGridType();
      }
    }

    for(int x=0; x< gridArray.GetLength(0); x++){
      for(int y=0; y < gridArray.GetLength(1); y++){
        Debug.DrawLine(GridToWorld(new Vector2Int(x,y)),GridToWorld(new Vector2Int(x,y+1)), Color.green, 100f);
        Debug.DrawLine(GridToWorld(new Vector2Int(x,y)),GridToWorld(new Vector2Int(x+1,y)), Color.green, 100f);
      }
    }
  }

  public Vector3 GridToWorld(Vector2Int gridposition){
    int x = gridposition.x;
    int y = gridposition.y;
    return new Vector3(x,0,y)*cellSize + originPosition;
  }

  public Vector2Int WorldToGrid(Vector3 worldPosition){
    int x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
    int y = Mathf.FloorToInt((worldPosition - originPosition).z / cellSize);
    return new Vector2Int(x,y);
  }

  public void Set2DGridValue(Vector2Int gridposition, TGridType value){
    int x = gridposition.x;
    int y = gridposition.y;
    if(x >= 0 && y >= 0 && x < width && y < height){
      gridArray[x,y] = value;
    }
  }

public void Set3DGridValue(Vector3 worldPosition, TGridType value){
  Vector2Int GridPos = WorldToGrid(worldPosition);
  Set2DGridValue(GridPos, value);
}

public TGridType Get2DGridValue(Vector2Int gridposition){
  int x = gridposition.x;
  int y = gridposition.y;
  if(x >= 0 && y >= 0 && x < width && y < height){
    return gridArray[x,y];
  } else {
    return default(TGridType);
  }
}

public bool ValidateGridSquare(Vector2Int gridposition){
  int x = gridposition.x;
  int y = gridposition.y;
  if(x >= 0 && y >= 0 && x < width && y < height){
    return true;
  } else {
    return false;
  }
}

// right, up, left, down, up-left, up-right, down-right, down-left
public TGridType[] Get2DGridAjacentValues(Vector2Int gridposition){
  TGridType[] data = new TGridType[8] ;
  int x = gridposition.x;
  int y = gridposition.y;
  if(x >= 0 && y >= 0 && x < width && y < height){
    data[0] = Get2DGridValue( new Vector2Int(x+1,y));
    data[1] = Get2DGridValue( new Vector2Int(x,y+1));
    data[2] = Get2DGridValue( new Vector2Int(x-1,y));
    data[3] = Get2DGridValue( new Vector2Int(x,y-1));
    data[4] = Get2DGridValue( new Vector2Int(x-1,y+1));
    data[5] = Get2DGridValue( new Vector2Int(x+1,y+1));
    data[6] = Get2DGridValue( new Vector2Int(x+1,y-1));
    data[7] = Get2DGridValue( new Vector2Int(x-1,y-1));
  }
  return data;
}

public TGridType Get3DGridValue(Vector3 worldPosition){
  Vector2Int GridPos = WorldToGrid(worldPosition);
  return gridArray[GridPos.x,GridPos.y];
}

}
