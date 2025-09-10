using System;
using UnityEngine;

public class GridController : MonoBehaviour
{
    [Header("Grid Settings")] [SerializeField]
    private GameObject cellPrefab;

    [SerializeField] public int gridWidth;
    [SerializeField] public int gridHeight;
    [SerializeField] public float cellSize;

    public Transform[,] grid;
    public static GridController instance;

    private void Awake()
    {
        instance = this;
    }

    public void CreateGrid()
    {
        grid = new Transform[gridWidth, gridHeight];

        GenerateGridCells();
        CenterGrid();
    }



    private void GenerateGridCells()
    {
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                GameObject newCell = Instantiate(cellPrefab, transform);

                // Pozisyonu ayarla
                newCell.transform.localPosition = new Vector3(
                    x * cellSize,
                    y * cellSize,
                    0
                );

                // Array'e kaydet
                grid[x, y] = newCell.transform;
            }
        }
    }

    private void CenterGrid()
    {
        float centerX = (gridWidth - 1) * cellSize * 0.5f;
        float centerY = (gridHeight - 1) * cellSize * 0.5f;
        Vector3 centerOffset = new Vector3(-centerX, -centerY, 0);

        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                if (grid[x, y] != null)
                {
                    grid[x, y].localPosition += centerOffset;
                }
            }
        }
    }

}
    