using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FloorDestroyer : MonoBehaviour
{
    [SerializeField] private int leftmostTileX; // the x pos of the leftmost tile to destroy
    [SerializeField] private int rightmostTileX; // the x pos of the rightmost tile
    [SerializeField] private int nextRowY; // the y pos of the next row to destroy (starting with the 1st)
    [SerializeField] private List<int> immuneRanges; // ranges of y values that cannot be destroyed (for shops)
    [SerializeField] private GameObject fallingTilePrefab;
    [SerializeField] private GameObject lavaLightPrefab;
    [SerializeField] private Camera cam;
    
    private float timeOfLastDestroy = 0f;
    private float timeBetweenDestroys = 0.2f;
    private int currImmuneRangeMin;
    private int currImmuneRangeMax;
    private int immuneRangeIndex = 0;

    private Tilemap floor;
    private float destroyRadius; // how far behind the camera must be to destroy tiles

    private List<Vector2> locToDestroySoon = new List<Vector2> (); // a list of locations that will be randomly destroyed over time

    void Start()
    {
        floor = GetComponent<Tilemap> ();
        destroyRadius = -cam.transform.position.z / 5f * 6f - 2f; // destroyRadius depends on the camera's z pos
        fallingTilePrefab.GetComponent<FallingTile> ().wobbleDuration = 1f / cam.GetComponent<CameraMov> ().camSpeed; // the tile's time spent wobbling depends on the camera's speed
        currImmuneRangeMin = immuneRanges[immuneRangeIndex++];
        currImmuneRangeMax = immuneRanges[immuneRangeIndex++];
    }

    void Update()
    {
        
        // continuously destroy rows of tiles as the camera moves
        if ((float) nextRowY + destroyRadius < cam.transform.position.y)
        {
            if (nextRowY <= currImmuneRangeMin)
            {
                // destroy all remaining tiles in the last row
                DestroyTiles (locToDestroySoon);
                // add a new row of tiles to destroy
                locToDestroySoon = new List<Vector2> ();
                for (int x = leftmostTileX; x <= rightmostTileX; ++x)
                {
                    locToDestroySoon.Add (new Vector2 (x, nextRowY));
                }
            }
            else if (nextRowY > currImmuneRangeMax)
            {
                currImmuneRangeMin = immuneRanges[immuneRangeIndex++];
                currImmuneRangeMax = immuneRanges[immuneRangeIndex++];
            }
            nextRowY += 1;
        }
        else if (timeOfLastDestroy + timeBetweenDestroys < Time.time)
        {
            if (nextRowY <= currImmuneRangeMin)
            {
                // destroy a random few tiles in the current row
                List<Vector2> locations = new List<Vector2> ();
                int n = Random.Range (1, 4);
                for (int i = 0; i < n && locToDestroySoon.Count > 0; ++i)
                {
                    int j = Random.Range (0, locToDestroySoon.Count);
                    locations.Add (locToDestroySoon[j]);
                    locToDestroySoon.RemoveAt(j);
                }
                DestroyTiles (locations);
                timeOfLastDestroy = Time.time;
            }
            else if (nextRowY > currImmuneRangeMax)
            {
                currImmuneRangeMin = immuneRanges[immuneRangeIndex++];
                currImmuneRangeMax = immuneRanges[immuneRangeIndex++];
            }
        }
        
    }

    // destroys a bunch of tiles, given a list of their Vector2
    // the Vector2 of a tile is the integer coordinates of its lower left hand corner
    public void DestroyTiles (List<Vector2> locations)
    {
        /*
        foreach (Vector2 loc in locations)
        {
            Vector3Int tileCoord = floor.WorldToCell (loc);
            TileBase tileBase = floor.GetTile (tileCoord);
            if (tileBase)
            {
                GameObject fallingTile = fallingTilePrefab;
                TileData tileData = new TileData ();
                tileBase.GetTileData (tileCoord, floor, ref tileData);

                // replace the floor tile with the falling tile
                floor.SetTile (tileCoord, null);
                GameObject ft = Instantiate (fallingTile, loc + new Vector2 (0.5f, 0.5f), Quaternion.identity, transform);
                Instantiate (lavaLightPrefab, loc, Quaternion.identity, transform);

                // update the falling tile's sprite
                ft.GetComponent<SpriteRenderer> ().sprite = tileData.sprite;

            }
        }
        */
    }
}
