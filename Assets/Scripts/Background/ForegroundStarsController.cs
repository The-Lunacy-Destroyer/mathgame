using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class ForegroundStarsController : MonoBehaviour
{
    public TileBase[] tiles;
    private Tilemap _tilemap;

    public int drawPosX = 0;
    public int drawPosY = 0;
    public int drawRadiusX = 100;
    public int drawRadiusY = 100;

    public int tileCount = 100;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _tilemap = GetComponent<Tilemap>();
        if (tiles.Length > 0)
        {
            Quaternion[] rotations = {
                Quaternion.Euler(0, 0, 0),
                Quaternion.Euler(0, 0, 90),
                Quaternion.Euler(0, 0, 180),
                Quaternion.Euler(0, 0, 270),
                Quaternion.Euler(180, 0, 0),
                Quaternion.Euler(0, 180, 0),
                Quaternion.Euler(180, 0, 90),
                Quaternion.Euler(0, 180, 90)
            };
            
            int[] cellPositionsX = Enumerable.Range(-drawRadiusX, drawRadiusX * 2)
                .OrderBy(x => Random.value).ToArray();
            int[] cellPositionsY = Enumerable.Range(-drawRadiusY, drawRadiusY * 2)
                .OrderBy(x => Random.value).ToArray();
            int maxPositionsLength = math.max(cellPositionsX.Length, cellPositionsY.Length);
            for (int i = 0, x = 0, y = 0; i < tileCount; i++)
            {
                Vector3Int cellPosition = new Vector3Int
                    (drawPosX + cellPositionsX[x], drawPosY + cellPositionsY[y]);
                _tilemap.SetTile(cellPosition, tiles[Random.Range(0, tiles.Length)]);
                RotateTile(cellPosition, rotations[Random.Range(0, rotations.Length)]);

                x++;
                y++;
                if (x >= cellPositionsX.Length)
                {
                    x = 0;
                    if (maxPositionsLength == cellPositionsX.Length)
                    {
                        int[] newCellPositionsX;
                        do
                        {
                            newCellPositionsX = cellPositionsX.OrderBy(e => Random.value).ToArray();
                        } 
                        while (newCellPositionsX.SequenceEqual(cellPositionsX));
                        cellPositionsX = newCellPositionsX;
                    }
                }
                if (y >= cellPositionsY.Length)
                {
                    y = 0;
                    if (maxPositionsLength == cellPositionsY.Length)
                    {
                        int[] newCellPositionsY;
                        do
                        {
                            newCellPositionsY = cellPositionsY.OrderBy(e => Random.value).ToArray();
                        } 
                        while (newCellPositionsY.SequenceEqual(cellPositionsY));
                        cellPositionsY = newCellPositionsY;
                    }
                }
            }
        }
    }
    
    void RotateTile(Vector3Int cellPosition, Quaternion rotation)
    {
        Matrix4x4 rotationMatrix = Matrix4x4.Rotate(rotation);
        _tilemap.SetTransformMatrix(cellPosition, rotationMatrix);
    }
}
