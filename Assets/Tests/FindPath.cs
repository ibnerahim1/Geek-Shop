using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FindPath : MonoBehaviour
{
    public Transform CellObj;
    public Vector2Int GridSize;
    public Vector2Int A, B;
    public Vector2Int[] walls;
    public int[,] grid;
    public TextMeshPro[,] texts;

    private List<Vector2Int> currentMarker = new List<Vector2Int>();
    private List<Vector2Int> nextMarker = new List<Vector2Int>();
    private List<Vector2Int> path = new List<Vector2Int>();
    private int currentVal = 1;

    private void Start()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        grid = new int[GridSize.x, GridSize.y];
        texts = new TextMeshPro[GridSize.x, GridSize.y];
        for (int i = 0; i < GridSize.x; i++)
        {
            for (int j = 0; j < GridSize.y; j++)
            {
                texts[i, j] = Instantiate(CellObj, new Vector2(i, j), Quaternion.identity, transform).GetComponent<TextMeshPro>();
            }
        }
        for (int i = 0; i < GridSize.x; i++)
        {
            for (int j = 0; j < GridSize.y; j++)
            {
                grid[i, j] = 0;
                texts[i, j].text = "";
            }
        }
        for (int i = 0; i < walls.Length; i++)
        {
            grid[walls[i].x, walls[i].y] = -1;
            texts[walls[i].x, walls[i].y].text = "X";
        }
        grid[A.x, A.y] = -1;
        grid[B.x, B.y] = -1;
        texts[A.x, A.y].text = "A";
        texts[B.x, B.y].text = "B";

        currentMarker.Add(A);
        StartCoroutine(CheckNeighbours());
    }

    private IEnumerator CheckNeighbours()
    {
        for (int i = 0; i < currentMarker.Count; i++)
        {
            yield return new WaitForSeconds(0.1f);
            for (int j = 0; j < 4; j++)
            {
                if(TryGetNeighbour(currentMarker[i], j, out Vector2Int result))
                {
                    MarkCell(result);
                }
            }
        }
        if (TargetReached())
        {
            StartCoroutine(DrawShortestPath());
        }
        else if (nextMarker.Count > 0)
        {
            currentMarker.Clear();
            currentMarker = new List<Vector2Int>(nextMarker);
            nextMarker.Clear();
            currentVal++;
            StartCoroutine(CheckNeighbours());
        }
    }

    private void MarkCell(Vector2Int val)
    {
        if (grid[val.x, val.y] == 0)
        {
            nextMarker.Add(new Vector2Int(val.x, val.y));
            grid[val.x, val.y] = currentVal;
            texts[val.x, val.y].text = currentVal.ToString();
        }
    }

    private IEnumerator DrawShortestPath()
    {
        path.Insert(0, B);
        while(currentVal>0)
        {
            path.Insert(0, PreviousCell(path[0]));
            currentVal--;
        }
        path.Insert(0, A);
        for (int i = 0; i < path.Count; i++)
        {
            yield return new WaitForSeconds(0.1f);
            texts[path[i].x, path[i].y].transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.cyan;
        }
    }

    private Vector2Int PreviousCell(Vector2Int val)
    {
        for (int i = 0; i < 4; i++)
        {
            if (TryGetNeighbour(val, i, out Vector2Int result))
            {
                if (grid[result.x, result.y] == currentVal)
                {
                    return new Vector2Int(result.x, result.y);
                }
            }
        }
        print("couldn't find previous cell");
        return new Vector2Int();
    }

    private bool TargetReached()
    {
        for (int i = 0; i < 4; i++)
        {
            if (TryGetNeighbour(B, i, out Vector2Int result))
            {
                if(grid[result.x, result.y] > 0)
                {
                    currentVal = grid[result.x, result.y];
                    return true;
                }
            }
        }
        return false;
    }

    private bool TryGetNeighbour(Vector2Int i, int dir, out Vector2Int result)
    {
        result = new Vector2Int();
        switch (dir)
        {
            case 0: //right
                if (i.x < GridSize.x - 1)
                {
                    result = new Vector2Int(i.x + 1, i.y);
                    return true;
                }
                else
                    return false;
            case 1: //left
                if (i.x > 0)
                {
                    result = new Vector2Int(i.x - 1, i.y);
                    return true;
                }
                else
                    return false;
            case 2: //up
                if (i.y < GridSize.y - 1)
                {
                    result = new Vector2Int(i.x, i.y + 1);
                    return true;
                }
                else
                    return false;
            case 3: //down
                if (i.y > 0)
                {
                    result = new Vector2Int(i.x, i.y - 1);
                    return true;
                }
                else
                    return false;
            default:
                return false;
        }
    }
}
