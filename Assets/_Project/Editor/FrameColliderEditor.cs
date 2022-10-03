using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using ToolsBoxEngine;

[CustomEditor(typeof(FrameCollider))]
public class FrameColliderEditor : Editor {
    private static class Colors {
        public static Color NOT_SELECTED = new Color(1f, 1f, 1f, 0.1f);
        //public static Color IN_GROUP = new Color(0.7f, 0.7f, 0.7f, 0.8f);
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

    Dictionary<int, List<FrameCollider.IOverlapCollider2D>> _colliders = new Dictionary<int, List<FrameCollider.IOverlapCollider2D>>();
    Dictionary<FrameCollider.IOverlapCollider2D, PrimitiveBoundsHandle> _boundsHandle = new Dictionary<FrameCollider.IOverlapCollider2D, PrimitiveBoundsHandle>();
    Dictionary<FrameCollider.IOverlapCollider2D, SerializedProperty> _properties = new Dictionary<FrameCollider.IOverlapCollider2D, SerializedProperty>();

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        if (!_init) { Init(); }

        EditorGUI.BeginChangeCheck();

        EditorGUI.BeginChangeCheck();

        EditorGUI.BeginDisabledGroup(_colliders == null || _colliders.Count == 0 || _colliders.Count == 1);
        EditorGUILayout.PrefixLabel("Group");
        _index.x = EditorGUILayout.IntSlider(_index.x, 0, _colliders.Count - 1);
        EditorGUI.EndDisabledGroup();

        if (EditorGUI.EndChangeCheck()) {
            _index.y = 0;
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

            FrameCollider.IOverlapCollider2D collider = _colliders[_index.x][_index.y];

            if (collider is FrameCollider.Circle circle) { CircleField(circle); }
            else if (collider is FrameCollider.Rectangle box) { RectangleField(box); }
            EditorGUILayout.EndVertical();

            if (EditorGUI.EndChangeCheck()) {
                if (collider is FrameCollider.Circle c) { SerializeCircle(_properties[c], c, _index.y); NewSphereBounds(c); } 
                else if (collider is FrameCollider.Rectangle b) { SerializeRectangle(_properties[b], b, _index.y); NewBoxBounds(b); }
                SceneView.RepaintAll();
            }
        }

        if (EditorGUI.EndChangeCheck()) {
            SceneView.RepaintAll();
        }

        EditorGUILayout.BeginHorizontal();

        GUILayout.FlexibleSpace();
        if (GUILayout.Button(" +O ")) {
            AddCircle();
        }
        if (GUILayout.Button(" +[] ")) {
            AddRectangle();
        }
        EditorGUI.BeginDisabledGroup(!(_index.x >= 0 && _index.x < _colliders.Count && _index.y < _colliders[_index.x].Count));
        if (GUILayout.Button(" X ")) {
            DeleteCollider(_colliders[_index.x][_index.y]);
        }
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.EndHorizontal();
    }

    private void OnSceneGUI() {
        if (!_init) { Init(); }

        float aspect = SceneView.lastActiveSceneView.size;
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
                    handle.center = Handles.FreeMoveHandle(handle.center, Quaternion.identity, 0.1f * aspect, Vector3.zero, Handles.CircleHandleCap);
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
        Debug.Log("Init !");

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

    private void AddRectangle() {
        FrameCollider.Rectangle rectangle = new FrameCollider.Rectangle(Vector2.zero, Vector3.one * 5f, 0f);

        NewBoxBounds(rectangle);
        _colliders[_index.x].Add(rectangle);
        int size = ++p_overlapRectangle.arraySize;
        SerializeRectangle(p_overlapRectangle.GetArrayElementAtIndex(size - 1), rectangle, 0);
        _properties[rectangle] = p_overlapRectangle.GetArrayElementAtIndex(size - 1);

        SceneView.RepaintAll();
    }

    private void AddCircle() {
        FrameCollider.Circle circle = new FrameCollider.Circle(Vector2.zero, 5f);

        NewSphereBounds(circle);
        _colliders[_index.x].Add(circle);
        int size = ++p_overlapCircle.arraySize;
        SerializeCircle(p_overlapCircle.GetArrayElementAtIndex(size - 1), circle, 0);
        _properties[circle] = p_overlapCircle.GetArrayElementAtIndex(size - 1);

        SceneView.RepaintAll();
    }

    private void Replicate(SerializedProperty element) {
        int index = FindIndex(element);
        if (!_colliders.ContainsKey(index)) { _colliders.Add(index, new List<FrameCollider.IOverlapCollider2D>()); }

        switch (FindType(element)) {
            case ColliderType.NULL:
                break;
            case ColliderType.CIRCLE:
                FrameCollider.Circle circle = DeserializeCircle(element.FindPropertyRelative("value"));
                _colliders[index].Add(circle);
                _properties[circle] = element;
                NewSphereBounds(circle);
                break;
            case ColliderType.RECTANGLE:
                FrameCollider.Rectangle rectangle = DeserializeRectangle(element.FindPropertyRelative("value"));
                _colliders[index].Add(rectangle);
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

    //private void SerializeNewObject(PrimitiveBoundsHandle handle) {
    //    int size = ++p_overlapCircle.arraySize;

    //    if (handle is SphereBoundsHandle sphere) {
    //        SerializeCircle(p_overlapCircle.GetArrayElementAtIndex(size - 1), new FrameCollider.Circle(sphere.center, sphere.radius), 0);
    //    } else if (handle is BoxBoundsHandle box) {
    //        SerializeRectangle(p_overlapCircle.GetArrayElementAtIndex(size - 1), new FrameCollider.Rectangle(box.center, box.size, 0f), 0);
    //    }
    //}

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

    // TODO
    private void DeleteCollider(FrameCollider.IOverlapCollider2D collider) {
        if (collider == null) { return; }

        if (collider is FrameCollider.Circle circle) {
            SerializedProperty indexed = p_overlapCircle.GetArrayElementAtIndex(p_overlapCircle.arraySize - 1);
            FrameCollider.IOverlapCollider2D last = DeserializeCircle(indexed.FindPropertyRelative("value"));
            SerializeCircle(_properties[circle], DeserializeCircle(indexed.FindPropertyRelative("value")), FindIndex(indexed));
            _properties.SwapKey(circle, last);
            --p_overlapCircle.arraySize;
        }

        _properties.Remove(collider);
        _boundsHandle.Remove(collider);
    }
}
