using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Camera camera;
    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private float shakeMagnitude = 1.5f;

    Coroutine _routine_shake = null;
    Vector3 _startPos = Vector3.zero;

    void Start()
    {
        camera = GetComponent<Camera>();
        _startPos = camera.transform.position;
    }

    public IEnumerator IShake(float duration, float magnitude) {
        //Vector3 startPos = transform.position;
        float elapsed = 0f;

        while (elapsed < duration) {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            camera.transform.position = new Vector3(x, y, -10f);
            elapsed += Time.deltaTime;
            yield return 0;
        }
        camera.transform.position = _startPos;
    }

    public IEnumerator IShake() {
        //Vector3 startPos = transform.position;
        float elapsed = 0f;

        while (elapsed < shakeDuration) {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            camera.transform.position = new Vector3(x, y, -10f);
            elapsed += Time.deltaTime;
            yield return 0;
        }
        camera.transform.position = _startPos;
    }

    public void Shake() {
        if (_routine_shake != null) { StopCoroutine(_routine_shake); }
        _routine_shake = StartCoroutine(IShake());
    }
}
