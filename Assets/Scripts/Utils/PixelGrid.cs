using UnityEditor;
using UnityEngine;

public class PixelGrid : EditorWindow
{
    [MenuItem("Window/Pixel Grid")]
    public static void ShowWindow()
    {
        var ew = GetWindow(typeof(PixelGrid));
        ew.minSize = new Vector2(200, 116);
        ew.maxSize = new Vector2(200, 116);
    }

    float _pixelsPerUnit = 48;
    float _gridSize = 8;

    bool _autoSnap = false;
    float _unitScale;
    Transform[] _trans;

    public void OnEnable()
    {
        _unitScale = 1 / _pixelsPerUnit;
        _trans = Selection.transforms;
    }

    public void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Pixels Per Unit : ");
        _pixelsPerUnit = EditorGUILayout.FloatField(_pixelsPerUnit);
        if (GUI.changed)
            _unitScale = 1 / _pixelsPerUnit;
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Grid Pixel Size : ");
        _gridSize = EditorGUILayout.FloatField(_gridSize);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Auto Snap : ");
        _autoSnap = EditorGUILayout.Toggle(_autoSnap);
        if (GUI.changed)
            _unitScale = 1 / _pixelsPerUnit;
        EditorGUILayout.EndHorizontal();

        GUILayout.Label("Unit Scale : " + _unitScale);
        GUILayout.Label("Selected : " + _trans.Length + " objects.");

        EditorGUI.BeginDisabledGroup(_autoSnap);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Snap X", "buttonleft"))
            SnapXToGrid();
        if (GUILayout.Button("Snap Y", "buttonmid"))
            SnapYToGrid();
        if (GUILayout.Button("Snap All", "buttonright"))
            SnapAllToGrid();
        EditorGUILayout.EndHorizontal();
        EditorGUI.EndDisabledGroup();
    }

    void Update()
    {
        if (Selection.transforms.Length != _trans.Length)
        {
            _trans = Selection.transforms;
            Repaint();
        }

        if (!_autoSnap)
            return;

        SnapAllToGrid();
    }

    void SnapAllToGrid()
    {
        SnapXToGrid();
        SnapYToGrid();
    }

    void SnapXToGrid()
    {
        if (_trans == null || _trans.Length == 0)
        {
            _trans = Selection.transforms;
            if (_trans == null || _trans.Length == 0)
                return;
        }

        foreach (var t in _trans)
        {
            if (t.gameObject.activeInHierarchy)
            {
                var pos = t.position;
                var newX = SnapToGrid(pos.x);
                t.position = new Vector2(newX, pos.y);
            }
        }
    }

    void SnapYToGrid()
    {
        if (_trans == null || _trans.Length == 0)
        {
            _trans = Selection.transforms;
            if (_trans == null || _trans.Length == 0)
                return;
        }

        foreach (var t in _trans)
        {
            if (t.gameObject.activeInHierarchy)
            {
                var pos = t.position;
                var newY = SnapToGrid(pos.y);
                t.position = new Vector2(pos.x, newY);
            }
        }
    }

    float SnapToGrid(float value)
    {
        var scale = _gridSize * _unitScale;
        var firstPass = Mathf.Round(value / scale) * scale;
        var newValue = Mathf.Round(firstPass / _unitScale) * _unitScale;
        return newValue;
    }
}