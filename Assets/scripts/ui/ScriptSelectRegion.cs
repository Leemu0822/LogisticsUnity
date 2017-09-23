using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptSelectRegion : MonoBehaviour {


    private bool _isDown;
    private Plane _groundPlane;
    public GameObject WorldManager;
    private ScriptWorldManager _worldManager;

    public ScriptRegion SelectedRegion;

    // Use this for initialization
    void Start () {
        _isDown = false;
        _groundPlane = new Plane(new Vector3(0, 0, 1), 0);
        _worldManager = WorldManager.GetComponent<ScriptWorldManager>();
    }


    // Update is called once per frame

    void Update() {
        if (Input.GetMouseButtonDown(0))
        {
            _isDown = true;
        }

        else if (Input.GetMouseButtonUp(0))
        {
            //TODO use different raycasting to also hit ui
            if (_isDown)
            {
                _isDown = false;
                float rayDistance;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (_groundPlane.Raycast(ray, out rayDistance))                {
                    SelectedRegion = _worldManager.GetRegion(ray.GetPoint(rayDistance));
                }
            }

        }
    }
}
