using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptSelectRegion : MonoBehaviour {


    private bool _isDown;

    // Use this for initialization
    void Start () {
        _isDown = false;
    }


    // Update is called once per frame

    void Update() {
        if (Input.GetMouseButtonDown(0))
        {
            _isDown = true;
        }

        else if (Input.GetMouseButtonUp(0))
        {
            if (_isDown)
            {
                _isDown = false;
                print("Click");
                RaycastHit hit1;
                Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit1);
                Vector3 v1 = hit1.point;
                print(hit1.point);

                //RaycastHit hit2;
                //Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit2);
                //Vector3 v2 = hit2.point;
                //Unit[] allUnits = GetAllUnits();
                //foreach (Unit unit in allUnits)
                //{
                //    Vector3 pos = unit.transform.position;
                //    //is inside the box
                //    if (Mathf.Max(v1.x, v2.x) & gt;= pos.x & amp; &amp; Mathf.Min(v1.x, v2.x) & lt;= pos.x
                //             & amp; &amp; Mathf.Max(v1.z, v2.z) & gt;= pos.z & amp; &amp; Mathf.Min(v1.z, v2.z) & lt;= pos.z) {
                //        //selecte the unit
                //        Selecte(unit);
                //    }
                //}
            }

        }
    }
}
