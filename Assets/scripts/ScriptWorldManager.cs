using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptWorldManager : MonoBehaviour
{
    private static float o = (Mathf.Tan(Mathf.PI / 6)) * 0.5f;
    private static float s = 0.5f / (Mathf.Cos(Mathf.PI / 6));

    public GameObject RegionPrefab;
    public GameObject RegionTilePrefab;

    private List<ScriptRegion> RegionScriptList;
    private List<ScriptRegionTile> RegionTileScriptList;

    // Use this for initialization
    void Start()
    {
        RegionScriptList = new List<ScriptRegion>();
        RegionTileScriptList = new List<ScriptRegionTile>();
        GenerateMap();

 
        var region_0 = Instantiate(RegionPrefab, this.gameObject.transform);
        region_0.transform.position = RegionTileScriptList[1114].gameObject.transform.position;
        var script_region_0 = region_0.GetComponent<ScriptRegion>();
 

        var region_1 = Instantiate(RegionPrefab, this.gameObject.transform);
        region_1.transform.position = RegionTileScriptList[1234].gameObject.transform.position;
        var script_region_1 = region_1.GetComponent<ScriptRegion>();

        script_region_0.Neighbors = new ScriptRegion[] { script_region_1 };
        script_region_1.Neighbors = new ScriptRegion[] { script_region_0 };
        RegionScriptList.Add(script_region_0);
        RegionScriptList.Add(script_region_1);

    }

    private void GenerateMap()
    {
        for (int indexY = 0; indexY < 50; indexY++)
        {
            for (int indexX = 0; indexX < 50; indexX++)
            {
                var regionTile = Instantiate(RegionTilePrefab, this.gameObject.transform);
                RegionTileScriptList.Add(regionTile.GetComponent<ScriptRegionTile>());
                if (indexY % 2 == 0)
                {
                    regionTile.transform.Translate(new Vector3(indexX + 0.5f, indexY * (o + s)));
                }
                else
                {
                    regionTile.transform.Translate( new Vector3(indexX, indexY * (o + s)));
                }
            }
        }  
    }

    // Update is called once per frame
    void Update()
    {

    }
}
