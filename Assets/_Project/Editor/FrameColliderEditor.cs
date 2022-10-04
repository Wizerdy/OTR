using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using ToolsBoxEngine;
using System.Linq;

[CustomEditor(typeof(FrameCollider))]
public class FrameColliderEditor : Editor {
    private static class Colors {
        public static Color NOT_SELECTED = new Color(1f, 1f, 1f, 0.1f);
        public static Color IN_GROUP = new Color(0.7f, 0.2f, 0.2f, 0.8f);
        public static Color SELECTED = Color.yellow;
    }

    enum ColliderType {
        NULL, CIRCLE, RECTANGLE
    }

    bool _init = false;
    SerializedProperty p_overlapCircle;
    SerializedProperty p_overlapRectangle;

    Vector2Int _index = Vector2Int.zero;
    int _fakeIndex = 0;

    float _sceneAspect = 0f;

    SortedDictionary<int, List<FrameCollider.IOverlapCollider2D>> _colliders = new SortedDictionary<int, List<FrameCollider.IOverlapCollider2D>>();
    Dictionary<FrameCollider.IOverlapCollider2D, PrimitiveBoundsHandle> _boundsHandle = new Dictionary<FrameCollider.IOverlapCollider2D, PrimitiveBoundsHandle>();
    Dictionary<FrameCollider.IOverlapCollider2D, SerializedProperty> _properties = new Dictionary<FrameCollider.IOverlapCollider2D, SerializedProperty>();

    public override void OnInspectorGUI() {
        if (!_init) { Init(); }

        EditorGUI.BeginChangeCheck();

        EditorGUI.BeginChangeCheck();

        EditorGUI.BeginDisabledGroup(_colliders == null || _colliders.Count == 0);
        EditorGUILayout.PrefixLabel("Group");
        _fakeIndex = _index.x + 1;
        _fakeIndex = EditorGUILayout.IntSlider(_fakeIndex, 0, _colliders.Count == 0 ? 0 : _colliders.Keys.Last() + 1);
        _index.x = _fakeIndex - 1;
        EditorGUI.EndDisabledGroup();

        if (EditorGUI.EndChangeCheck()) {
            _index.y = 0;
            serializedObject.FindProperty("_currentIndex").intValue = _index.x;
            serializedObject.ApplyModifiedProperties();
        }

        if (_index.x < 0) {
        }

        if (_colliders.ContainsKey(_index.x)) {

            EditorGUILayout.Space();
            GUIStyle style = new GUIStyle(GUI.skin.window);
            style.padding.top = style.padding.bottom;
            style.padding = new RectOffset(style.padding.left * 2, style.padding.right * 2, style.padding.top * 2, style.padding.bottom * 2);
            EditorGUILayout.BeginVertical(style);

            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUI.BeginDisabledGroup(_colliders[_index.x] == null || _colliders[_index.x].Count == 0 || _colliders[_index.x].Count == 1);
            EditorGUILayout.PrefixLabel("Index");
            _index.y = EditorGUILayout.IntSlider(_index.y + 1, 1, _colliders[_index.x].Count) - 1;
            if (_index.y >= _colliders[_index.x].Count) { _index.y = _colliders[_index.x].Count - 1; }
            if (_index.y < 0) { _index.y = 0; }
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndVertical();

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.Separator();

            FrameCollider.IOverlapCollider2D collider = _colliders[_index.x][_index.y];

            EditorGUI.BeginChangeCheck();
            int newIndex = EditorGUILayout.IntField("Group", Mathf.Max(_fakeIndex, 0));
            if (EditorGUI.EndChangeCheck()) {
                if (newIndex - 1 >= 0) {
                    ChangeIndex(collider, newIndex - 1);
                }
            }

            EditorGUILayout.Separator();

            if (collider is FrameCollider.Circle circle) { CircleField(circle); }
            else if (collider is FrameCollider.Rectangle box) { RectangleField(box); }
            EditorGUILayout.EndVertical();

            if (EditorGUI.EndChangeCheck()) {
                if (collider is FrameCollider.Circle c) { SerializeCircle(_properties[c], c, FindIndex(_properties[c])); NewSphereBounds(c); } 
                else if (collider is FrameCollider.Rectangle b) { SerializeRectangle(_properties[b], b, FindIndex(_properties[b])); NewBoxBounds(b); }
                SceneView.RepaintAll();
            }
        } else if (_index.x < 0) {
            EditorGUILayout.HelpBox(" Disabled", MessageType.Info);
        } else {
            EditorGUILayout.HelpBox(" Empty", MessageType.Warning);
        }

        if (EditorGUI.EndChangeCheck()) {
            SceneView.RepaintAll();
        }

        EditorGUILayout.BeginHorizontal();

        GUILayout.FlexibleSpace();
        if (GUILayout.Button(" +O ")) {
            _index.x = Mathf.Max(_index.x, 0);
            AddCircle(_index.x);
            _index.y = _colliders[_index.x].Count - 1;
        }
        if (GUILayout.Button(" +[] ")) {
            _index.x = Mathf.Max(_index.x, 0);
            AddRectangle(_index.x);
            _index.y = _colliders[_index.x].Count - 1;
        }
        EditorGUI.BeginDisabledGroup(!_colliders.ContainsKey(_index.x) || _index.y >= _colliders[_index.x].Count);
        if (GUILayout.Button(" X ")) {
            DeleteCollider(_colliders[_index.x][_index.y]);
        }
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.EndHorizontal();

    }

    private void OnSceneGUI() {
        if (!_init) { Init(); }

        _sceneAspect = SceneView.lastActiveSceneView.size;
        Matrix4x4 baseMatrix = Handles.matrix;
        FrameCollider target = (serializedObject.targetObject as FrameCollider);
        Handles.matrix = target?.transform.localToWorldMatrix ?? Handles.matrix;

        PrimitiveBoundsHandle handle;

        foreach (KeyValuePair<int, List<FrameCollider.IOverlapCollider2D>> pair in _colliders) {
            for (int i = 0; i < pair.Value.Count; i++) {
                EditorGUI.BeginChangeCheck();
                handle = _boundsHandle[pair.Value[i]];
                Color color = _index.x == pair.Key ? Colors.IN_GROUP : Colors.NOT_SELECTED;

                if (_index.x == pair.Key && i == _index.y) {
                    color = Colors.SELECTED;
                }

                Color alphed = color;
                alphed.a *= 0.1f;
                if (handle is SphereBoundsHandle sphere) {
                    Color temp = Handles.color;
                    Handles.color = alphed;
                    Handles.DrawSolidDisc(sphere.center, Vector3.forward, sphere.radius);
                    Handles.color = color;
                    Handles.DrawWireDisc(sphere.center, Vector3.forward, sphere.radius);
                    Handles.color = temp;
                } else if (handle is BoxBoundsHandle box) {
                    Handles.DrawSolidRectangleWithOutline(new Rect(box.center - box.size / 2f, box.size), alphed, color);
                    if (_index.x == pair.Key && i == _index.y) {
                        FrameCollider.Rectangle rectangle = pair.Value[i] as FrameCollider.Rectangle;
                        Quaternion nion = Handles.FreeRotateHandle(Quaternion.AngleAxis(rectangle.rotation, Vector3.forward), box.center, 1f);
                        Debug.DrawLine(Vector3.zero, nion * Vector3.right, Color.blue, 5f);
                        //Debug.Log(Quaternion.Angle(Quaternion.AngleAxis(0f, Vector3.forward), nion));
                        (pair.Value[i] as FrameCollider.Rectangle).rotation = Quaternion.Angle(Quaternion.AngleAxis(0f, Vector3.forward), nion);
                    }
                }

                if (_index.x == pair.Key && i == _index.y) {
                    handle.DrawHandle();
                    handle.center = Handles.FreeMoveHandle(handle.center, Quaternion.identity, 0.05f * _sceneAspect, Vector3.zero, Handles.CircleHandleCap);
                }

                if (EditorGUI.EndChangeCheck()) {
                    if (handle is SphereBoundsHandle s) {
                        FrameCollider.Circle circle = BoundsToCircle(s);
                        SerializeCircle(_properties[pair.Value[i]], circle, _index.x);
                        _properties.SwapKey(pair.Value[i], circle);
                        _boundsHandle.SwapKey(pair.Value[i], circle);
                        pair.Value[i] = circle;
                    } else if (handle is BoxBoundsHandle b) {
                        FrameCollider.Rectangle rectangle = BoundsToRectangle(b);
                        SerializeRectangle(_properties[pair.Value[i]], rectangle, _index.x);
                        _properties.SwapKey(pair.Value[i], rectangle);
                        _boundsHandle.SwapKey(pair.Value[i], rectangle);
                        pair.Value[i] = rectangle;
                    }
                }

            }
        }

        Handles.matrix = baseMatrix;

        Repaint();
    }

    private void Init() {
        _colliders = new SortedDictionary<int, List<FrameCollider.IOverlapCollider2D>>();
        _properties = new Dictionary<FrameCollider.IOverlapCollider2D, SerializedProperty>();
        _boundsHandle = new Dictionary<FrameCollider.IOverlapCollider2D, PrimitiveBoundsHandle>();
        _index.x = serializedObject.FindProperty("_currentIndex").intValue;

        _sceneAspect = SceneView.lastActiveSceneView.size;

        serializedObject.Update();
        //p_overlapColliders = serializedObject.FindProperty("_overlapColliders");
        p_overlapCircle = serializedObject.FindProperty("_overlapCircle");
        p_overlapRectangle = serializedObject.FindProperty("_overlapRectangle");
        if (p_overlapCircle == null) { Debug.LogError("Property not found ! : _overlapCircle"); return; }
        if (p_overlapRectangle == null) { Debug.LogError("Property not found ! : _overlapRectangle"); return; }
        _init = true;

        for (int i = 0; i < p_overlapCircle.arraySize; i++) {
            SerializedProperty element = p_overlapCircle.GetArrayElementAtIndex(i);
            Replicate(element);
        }

        for (int i = 0; i < p_overlapRectangle.arraySize; i++) {
            SerializedProperty element = p_overlapRectangle.GetArrayElementAtIndex(i);
            Replicate(element);
        }
    }

    private void CircleField(FrameCollider.Circle circle) {
        circle.position = EditorGUILayout.Vector2Field("Center", circle.position);
        circle.radius = EditorGUILayout.FloatField("Radius", circle.radius);
    }

    private void RectangleField(FrameCollider.Rectangle rectangle) {
        rectangle.position = EditorGUILayout.Vector2Field("Center", rectangle.position);
        rectangle.size = EditorGUILayout.Vector2Field("Size", rectangle.size);
        rectangle.rotation = EditorGUILayout.FloatField("Rotation", rectangle.rotation);
    }

    private void AddRectangle(int index) {
        FrameCollider.Rectangle rectangle = new FrameCollider.Rectangle(Vector2.zero, Vector3.one * 0.1f * _sceneAspect, 0f);

        NewBoxBounds(rectangle);
        AddCollider(_index.x, rectangle);
        int size = ++p_overlapRectangle.arraySize;
        SerializeRectangle(p_overlapRectangle.GetArrayElementAtIndex(size - 1), rectangle, 0);
        _properties[rectangle] = p_overlapRectangle.GetArrayElementAtIndex(size - 1);

        SceneView.RepaintAll();
    }

    private void AddCircle(int index) {
        FrameCollider.Circle circle = new FrameCollider.Circle(Vector2.zero, 0.1f * _sceneAspect);

        NewSphereBounds(circle);
        AddCollider(_index.x, circle);
        int size = ++p_overlapCircle.arraySize;
        SerializeCircle(p_overlapCircle.GetArrayElementAtIndex(size - 1), circle, index);
        _properties[circle] = p_overlapCircle.GetArrayElementAtIndex(size - 1);

        SceneView.RepaintAll();
    }

    private void Replicate(SerializedProperty element) {
        int index = FindIndex(element);

        switch (FindType(element)) {
            case ColliderType.NULL:
                break;
            case ColliderType.CIRCLE:
                FrameCollider.Circle circle = DeserializeCircle(element.FindPropertyRelative("value"));
                AddCollider(index, circle);
                _properties[circle] = element;
                NewSphereBounds(circle);
                break;
            case ColliderType.RECTANGLE:
                FrameCollider.Rectangle rectangle = DeserializeRectangle(element.FindPropertyRelative("value"));
                AddCollider(index, rectangle);
                _properties[rectangle] = element;
                NewBoxBounds(rectangle);
                break;
            default:
                break;
        }
    }

    private void NewSphereBounds(FrameCollider.Circle circle) {
        SphereBoundsHandle circHandle = new SphereBoundsHandle();
        circHandle.wireframeColor = Colors.SELECTED;
        circHandle.center = circle.position;
        circHandle.radius = circle.radius;
        circHandle.axes = PrimitiveBoundsHandle.Axes.X | PrimitiveBoundsHandle.Axes.Y;

        if (!_boundsHandle.ContainsKey(circle)) { _boundsHandle.Add(circle, circHandle); }
        else { _boundsHandle[circle] = circHandle; }
    }

    private void NewBoxBounds(FrameCollider.Rectangle rectangle) {
        BoxBoundsHandle rectHandle = new BoxBoundsHandle();
        rectHandle.wireframeColor = Colors.SELECTED;
        rectHandle.center = rectangle.position;
        rectHandle.size = rectangle.size;
        rectHandle.axes = PrimitiveBoundsHandle.Axes.X | PrimitiveBoundsHandle.Axes.Y;

        if (!_boundsHandle.ContainsKey(rectangle)) { _boundsHandle.Add(rectangle, rectHandle); }
        else { _boundsHandle[rectangle] = rectHandle; }
    }

    private ColliderType FindType(SerializedProperty property) {
        if (property.FindPropertyRelative("value").type == "Circle") {
            return ColliderType.CIRCLE;
        } else if (property.FindPropertyRelative("value").type == "Rectangle") {
            return ColliderType.RECTANGLE;
        }
        return ColliderType.NULL;
    }

    private void SerializeCircle(SerializedProperty property, FrameCollider.Circle value, int index) {
        property.FindPropertyRelative("index").intValue = index;
        property = property.FindPropertyRelative("value");
        property.FindPropertyRelative("position").vector2Value = value.position;
        property.FindPropertyRelative("radius").floatValue = value.radius;
        serializedObject.ApplyModifiedProperties();
    }

    private void SerializeRectangle(SerializedProperty property, FrameCollider.Rectangle value, int index) {
        property.FindPropertyRelative("index").intValue = index;
        property = property.FindPropertyRelative("value");
        property.FindPropertyRelative("position").vector2Value = value.position;
        property.FindPropertyRelative("size").vector2Value = value.size;
        property.FindPropertyRelative("rotation").floatValue = value.rotation;
        serializedObject.ApplyModifiedProperties();
    }

    private int FindIndex(SerializedProperty indexed) {
        return indexed.FindPropertyRelative("index").intValue;
    }

    private FrameCollider.Circle DeserializeCircle(SerializedProperty property) {
        Vector2 position = property.FindPropertyRelative("position").vector2Value;
        float radius = property.FindPropertyRelative("radius").floatValue;
        return new FrameCollider.Circle(position, radius);
    }

    private FrameCollider.Rectangle DeserializeRectangle(SerializedProperty property) {
        Vector2 position = property.FindPropertyRelative("position").vector2Value;
        Vector2 size = property.FindPropertyRelative("size").vector2Value;
        return new FrameCollider.Rectangle(position, size, 0f);
    }

    private FrameCollider.Circle BoundsToCircle(SphereBoundsHandle handle) {
        return new FrameCollider.Circle(handle.center, handle.radius);
    }

    private FrameCollider.Rectangle BoundsToRectangle(BoxBoundsHandle handle) {
        return new FrameCollider.Rectangle(handle.center, handle.size, 0f);
    }

    private void AddCollider(int index, FrameCollider.IOverlapCollider2D collider) {
        if (!_colliders.ContainsKey(index)) { _colliders.Add(index, new List<FrameCollider.IOverlapCollider2D>()); }
        _colliders[index].Add(collider);
    }

    private void DeleteCollider(FrameCollider.IOverlapCollider2D collider) {
        if (collider == null) { return; }

        int index = FindIndex(_properties[collider]);
        _colliders[index].Remove(collider);
        _index.y = 0;
        if (_colliders[index].Count == 0) { _colliders.Remove(index); _index.x = 0; }

        if (collider is FrameCollider.Circle circle) {
            SerializedProperty p_last = p_overlapCircle.GetArrayElementAtIndex(p_overlapCircle.arraySize - 1);
            SerializeCircle(_properties[circle], DeserializeCircle(p_last.FindPropertyRelative("value")), index);
            --p_overlapCircle.arraySize;
        } else if (collider is FrameCollider.Rectangle rectangle) {
            SerializedProperty p_last = p_overlapCircle.GetArrayElementAtIndex(p_overlapCircle.arraySize - 1);
            SerializeCircle(_properties[rectangle], DeserializeCircle(p_last.FindPropertyRelative("value")), index);
            --p_overlapCircle.arraySize;
        }

        serializedObject.ApplyModifiedProperties();

        Init();

        SceneView.RepaintAll();
        Repaint();
    }

    private void ChangeIndex(FrameCollider.IOverlapCollider2D collider, int index) {
        foreach (var pair in _colliders) {
            if (pair.Value.Contains(collider)) {
                pair.Value.Remove(collider);
                if (pair.Value.Count == 0) {
                    _colliders.Remove(pair.Key);
                }
                break;
            }
        }

        AddCollider(index, collider);

        _properties[collider].FindPropertyRelative("index").intValue = index;

        //serializedObject.ApplyModifiedProperties();

        _index.x = index;
        _index.y = _colliders[index].Count - 1;

        SceneView.RepaintAll();
        Repaint();
    }
}
