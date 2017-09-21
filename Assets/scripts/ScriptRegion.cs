using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptRegion : MonoBehaviour {

    public IList<ScriptRegion> Neighbors;
    public IList<ScriptRegionTile> RegionTileList;
    public GameObject TransportPrefab;

    public float GrainProduction;
    public float Grain;
    public float GrainTransport;


    private 

    // Use this for initialization
    void Start () {
         GrainProduction = 1;
         Grain = 0;
         GrainTransport = 60;

        //Add Activities
    }
	
	// Update is called once per frame
	void Update () {


        //Demand
        //Price
        //Consume and trade
        //Produce
       


        Grain += GrainProduction;
        if (GrainTransport < Grain)
        {
            Grain -= GrainTransport;

            var transport = Instantiate(TransportPrefab, this.gameObject.transform);
            var transportScript = transport.GetComponent<ScriptTransport>();
            transportScript.Target = Neighbors[0].gameObject;

        }
    }
}
