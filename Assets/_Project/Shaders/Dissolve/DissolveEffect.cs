using UnityEngine;
using System.Collections;

public class DissolveEffect : MonoBehaviour 
{
	private float _value = 1.0f;
	private bool _isRunning = false;
	private Material _dissolveMaterial = null;
	public float timeScale = 1.0f;
	void Start()
	{
		//float maxVal = 0.0f;
		_dissolveMaterial = GetComponent<Renderer>().material;
		//var verts = GetComponent<MeshFilter>().mesh.vertices;
		//for (int i = 0; i < verts.Length; i++)
		//{
		//	var v1 = verts[i];
		//	for (int j = 0; j < verts.Length; j++)
		//	{
		//		if (j == i) continue;
		//		var v2 = verts[j];
		//		float mag = (v1-v2).magnitude;
		//		if ( mag > maxVal ) maxVal = mag;
				
		//	}
		//}
		 
		_dissolveMaterial.SetFloat("_LargestVal", 2.0f);
	}

	public void Reset()
	{
		_value = 1.0f;
		_dissolveMaterial.SetFloat("_DissolveValue", _value);
	}

	public void TriggerReborn(Vector3 hitPoint)
	{
		_value = 0.0f;
		_dissolveMaterial.SetVector("_HitPos", (new Vector4(hitPoint.x, hitPoint.y, hitPoint.z, 1.0f)));
		_isRunning = true;
	}
    public void TriggerReborn() {
        _value = 0.0f;
        _isRunning = true;
        SoundManager.Instance.PlaySfx(AudioName.UI_Fire);
    }

    void Update()
	{
		if (_isRunning && _dissolveMaterial != null)
		{
			_value += Time.deltaTime * timeScale;
			if (_value > 1.0f) {
				_value = 1.0f;
                _dissolveMaterial.SetFloat("_DissolveValue", _value);
                SoundManager.Instance.StopAudioWithReduceVolume(AudioName.UI_Fire);
                SoundManager.Instance.PlaySfx(AudioName.UI_Bird);

                _isRunning = false;
            }
            //_value = Mathf.Max(_value + Time.deltaTime * timeScale, 1.0f);
            _dissolveMaterial.SetFloat("_DissolveValue", _value);
		}

	}
}
