using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptRegion : MonoBehaviour
{

    public IList<ScriptRegion> Neighbors;
    public IList<ScriptRegionTile> RegionTileList;
    public GameObject TransportPrefab;

    public string Name;

    public static int Tax = 0;
    public static int Grain = 1;
    public static int Labor = 2;
    public static int Authority = 3;

    public static int Population = 0;
    public static int Farming = 1;
    public static int Administration = 2;

    public static float AbundancyLow  = 500;
    public static float AbundancyHigh = 3000;




    private IList<ResourceStock> ResourceList;
    private IList<Activity> ActivityList;

    // Use this for initialization
    void Start()
    {
        //Add Activities
        ResourceList = new List<ResourceStock>();
        ResourceList.Add(new ResourceStock(1, 1));//Tax
        ResourceList.Add(new ResourceStock(1,0.99f));//Grain
        ResourceList.Add(new ResourceStock(1, 0));//Labor
        ResourceList.Add(new ResourceStock(1, 1));//Authority

        ActivityList = new List<Activity>();
        ActivityList.Add(new Activity()); //Population
        ActivityList.Add(new Activity()); //Farming
        ActivityList.Add(new Activity()); //Administration

        ActivityList[Population].Size = 1;
        ActivityList[Population].Wealth = 200;
        ActivityList[Population].UpkeepInputPerSize.Add(new ResourceAmount(Grain, 1));
        ActivityList[Population].ProductionOutputPerSize.Add(new ResourceAmount(Labor, 1));

        ActivityList[Farming].Size = 1;
        ActivityList[Farming].Wealth = 200;
        ActivityList[Farming].ProductionInputPerSize.Add(new ResourceAmount(Labor, 1));
        ActivityList[Farming].ProductionOutputPerSize.Add(new ResourceAmount(Grain, 2));

        ActivityList[Administration].Size = 1;
        ActivityList[Administration].Wealth = 200;
        ActivityList[Administration].ProductionInputPerSize.Add(new ResourceAmount(Labor, 1));
        ActivityList[Administration].ProductionOutputPerSize.Add(new ResourceAmount(Grain, 2));
    }

    // Update is called once per frame
    void Update()
    {

        //Clear
        foreach (ResourceStock stock in ResourceList)
        {
            stock.Clear();
        }

        //Upkeep
        foreach (Activity activity in ActivityList)
        {
            activity.Upkeep(this);
        }

        //Production
        foreach (Activity activity in ActivityList)
        {
            activity.Production(this);
        }

        //Resolve
        foreach (ResourceStock stock in ResourceList)
        {
            stock.Resolve();
        }

    }

    public class ResourceStock
    {
        public float Stock;
        public float Consumption;
        public float Trade;
        public float Production;


        public float Abundancy;
        public float Price;
        public float Carry;

        public ResourceStock(float price, float decay)
        {
            Price = price;
            Carry = decay;
        }

        public void Clear()
        {
            Stock *= Carry;
            Consumption = 0;
            Trade = 0;
            Production = 0;
        }

        public void Resolve()
        {
            Stock = Stock - Consumption - Trade + Production;
            Abundancy = Stock / Consumption;
            if (Abundancy < AbundancyLow)
            {
                Price += 0.01f;
            } else if (AbundancyHigh < Abundancy)
            {
                Price -= 0.01f;
            }
        }
    }

    public struct ResourceAmount
    {
        public int Resource;
        public float Amount;

        public ResourceAmount(int resource, float amount)
        {
            Resource = resource;
            Amount = amount;
        }
    }

    public class Activity
    {
        public IList<ResourceAmount> UpkeepInputPerSize;
        public IList<ResourceAmount> UpkeepOutputPerSize;
        public IList<ResourceAmount> ProductionInputPerSize;
        public IList<ResourceAmount> ProductionOutputPerSize;

        public float Size;
        public float SizeMax;
        public float Wealth;
        public float Growth;
        public float GrowthBase = 0.99f;
        public float GrowthMultiplier = 0.01f;
        public float GrowthLimit = 1.5f;

        public float UpkeepBudget = 0.01f;
        public float ActivityUpkeep;
        public float ActivityProduction;

        public float AbundancyUpkeep;
        public float AbundancyProduction;

        public Activity()
        {
            UpkeepInputPerSize = new List<ResourceAmount>();
            UpkeepOutputPerSize = new List<ResourceAmount>();
            ProductionInputPerSize = new List<ResourceAmount>();
            ProductionOutputPerSize = new List<ResourceAmount>();
        }


        public void Upkeep(ScriptRegion region)
        {
            float budget = Wealth * 0.01f;
            float cost = 0;
            foreach (ResourceAmount resourceAmount in UpkeepInputPerSize)
            {
                cost += region.ResourceList[resourceAmount.Resource].Price * resourceAmount.Amount * Size;
            }
     
            ActivityUpkeep = Mathf.Min(budget / cost, GrowthLimit);

            Wealth -= cost * ActivityUpkeep;
            foreach (ResourceAmount resourceAmount in UpkeepInputPerSize)
            {
                region.ResourceList[resourceAmount.Resource].Consumption += resourceAmount.Amount * Size * ActivityUpkeep;

            }

            foreach (ResourceAmount resourceAmount in UpkeepOutputPerSize)
            {
                region.ResourceList[resourceAmount.Resource].Consumption += resourceAmount.Amount * Size * ActivityUpkeep;
            }

            Growth = GrowthBase + (GrowthMultiplier * ActivityUpkeep);
            Size *= Growth;
        }

        public void Production(ScriptRegion region)
        {
            float budget = Wealth * 0.01f;
            float cost = 0;
            foreach (ResourceAmount resourceAmount in ProductionInputPerSize)
            {
                cost += region.ResourceList[resourceAmount.Resource].Price * resourceAmount.Amount * Size;
            }
            ActivityProduction = Mathf.Min(budget / cost, 1);


            Wealth -= cost * ActivityProduction;
            foreach (ResourceAmount resourceAmount in ProductionInputPerSize)
            {
                region.ResourceList[resourceAmount.Resource].Consumption += resourceAmount.Amount * Size * ActivityProduction;
            }
            foreach (ResourceAmount resourceAmount in ProductionOutputPerSize)
            {
                float production = resourceAmount.Amount * Size * ActivityProduction;
                print(region.ResourceList.Count);
                region.ResourceList[resourceAmount.Resource].Production += production;
                Wealth += region.ResourceList[resourceAmount.Resource].Price * production;
            }

        }
    }



    public void Deliver(ResourceAmount resourceAmount)
    {
        ResourceList[resourceAmount.Resource].Stock += resourceAmount.Amount;
    }


    public string GetProperty(string propertName)
    {
        switch (propertName)
        {
            case "Name": return Name;
            case "Grain": return "Grain: " + GetStock(Grain) + " C: " + GetConsumption(Grain) + " p: "  + GetProduction(Grain) + " $: " + GetPrice(Grain) + " A : " + GetAbundancy(Grain);
            case "Tax": return "Tax: " + GetStock(Tax) + " C: " + GetConsumption(Tax) + " p: " + GetProduction(Tax) + " $: " + GetPrice(Tax) + " A : " + GetAbundancy(Tax);
            case "Labor": return "Labor: " + GetStock(Labor) + " C: " + GetConsumption(Labor) + " p: " + GetProduction(Labor) + " $: " + GetPrice(Labor) + " A : " + GetAbundancy(Labor);
            case "Population": return "Population: " + GetSize(Population) + " W: " + GetWealth(Population);
            case "Farming": return "Farming: " + GetSize(Population) + " W: " + GetWealth(Population);
            default: return "unknownProperty: " + propertName;
        }
    }


    public string GetStock(int resourceIndex)
    {
        return ResourceList[resourceIndex].Stock.ToString("0.00");
    }
    public string GetConsumption(int resourceIndex)
    {
        return ResourceList[resourceIndex].Consumption.ToString("0.00");
    }

    public string GetProduction(int resourceIndex)
    {
        return ResourceList[resourceIndex].Production.ToString("0.00");
    }

    public string GetPrice(int resourceIndex)
    {
        return ResourceList[resourceIndex].Price.ToString("0.00");
    }

    public string GetAbundancy(int resourceIndex)
    {
        return ResourceList[resourceIndex].Abundancy.ToString("0.00");
    }

    public string GetSize(int activityIndex)
    {
        return ActivityList[activityIndex].Size.ToString("0.00");
    }

    public string GetWealth(int activityIndex)
    {
        return ActivityList[activityIndex].Wealth.ToString("0.00");
    }

}
