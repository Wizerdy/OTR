using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;
using UnityEngine.VFX;

public class EntityPowerUp : MonoBehaviour, IEntityAbility {
    [SerializeField] EntityAbilities _abilities;
    [SerializeField] bool _stackable = false;
    [SerializeField] VisualEffect _boostParticles;

    List<PowerUp> _powerUps = new List<PowerUp>();

    public EntityAbilities Abilities => _abilities;

    Token _particlesToken = new Token();
    bool Particles { get => _particlesToken.HasToken; set { _particlesToken.AddToken(value); PlayParticles(Particles); } }

    private void Start() {
        PlayParticles(false);
    }

    public PowerUp Add(PowerUp powerUp) {
        PowerUp up = powerUp.SetTarget(_abilities);
        if (!_stackable) {
            _powerUps.Find((PowerUp pu) => { return pu.GetType() == up.GetType(); } )?.Disable();
        }
        _powerUps.Add(up);

        up.Enable();
        up.OnDisable += _Remove;

        if (up.PlayParticles) {
            Particles = true;
        }
        return up;
    }

    public void Stock(PowerUp powerUp) {
        _powerUps.Add(powerUp);
    }

    void _Remove(PowerUp up) {
        if (up.PlayParticles) {
            Particles = false;
        }

        _powerUps.Remove(up);
        up.SetTarget(null);
    }

    void PlayParticles(bool state) {
        if (state) {
            _boostParticles.gameObject.SetActive(true);
            _boostParticles.Play();
        } else {
            _boostParticles.Stop();
        }
    }
}
