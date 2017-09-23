using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptShowRegionName : MonoBehaviour {

    public GameObject UIManager;
    public string Propery;
    private ScriptSelectRegion _uiManager;
    private Text _text;

    // Use this for initialization
    void Start () {
        _uiManager = UIManager.GetComponent<ScriptSelectRegion>();
        _text = GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {

        if (_uiManager.SelectedRegion != null)
        {           
            _text.text = _uiManager.SelectedRegion.GetProperty(Propery);
        }
        else
        {
            _text.text = "";
        }
    }
}
