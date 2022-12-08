using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.Users;
using ToolsBoxEngine.BetterEvents;

[CreateAssetMenu(fileName = "New Character Data", menuName = "Scriptable Object/Character Data")]
public class CharacterData : ScriptableObject {
    [SerializeField] string _name = "";
    [SerializeField] Texture2D _skin;
    [SerializeField] Color _color = Color.white;

    [SerializeField, HideInInspector] BetterEvent<InputUser> _onNewUser = new BetterEvent<InputUser>();

    InputUser _user;

    public string Name => _name;
    public Texture2D Skin => _skin;
    public Color Color => _color;

    public InputUser User { get => _user; set { _user = value; _onNewUser.Invoke(_user); } }

    public event UnityAction<InputUser> OnNewUser { add => _onNewUser += value; remove => _onNewUser -= value; }
}
