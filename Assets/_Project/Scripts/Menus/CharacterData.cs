using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Users;

[CreateAssetMenu(fileName = "New Character Data", menuName = "Scriptable Object/Character Data")]
public class CharacterData : ScriptableObject {
    [SerializeField] string _name = "";
    [SerializeField] Texture2D _skin;
    [SerializeField] Color _color = Color.white;

    InputUser _user;

    public string Name => _name;
    public Texture2D Skin => _skin;
    public Color Color => _color;

    public InputUser User { get => _user; set => _user = value; }
}
