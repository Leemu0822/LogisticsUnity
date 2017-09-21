using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptZoom : MonoBehaviour {

    public float ZoomSpeed = 1f;
    public float MinSize = 0.5f;
    public float MaxSize = 50f;

    public float TranslateSpeed = 10f;

    private Camera _camera;

    // Use this for initialization
    void Start()
    {
        _camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        var newSize = _camera.orthographicSize - ZoomSpeed * Input.mouseScrollDelta.y;
        _camera.orthographicSize = Mathf.Clamp(newSize, MinSize, MaxSize);

        var horizontal = Input.GetAxis("Horizontal") * TranslateSpeed * Time.deltaTime;
        var vertical = Input.GetAxis("Vertical") * TranslateSpeed * Time.deltaTime;
        _camera.transform.Translate(horizontal, vertical, 0);

    }
}
