using UnityEngine;

/// <summary>
/// ビルボード クラス
/// このスクリプトのアタッチ先は対象オブジェクトの親にしてください
/// 製作者：実川
/// </summary>
public class Billboard : MonoBehaviour
{
	private Transform _camera = null;

	void Start ()
	{
		_camera = Camera.main.transform;
	}
	
	void Update ()
	{
		Vector3 p = _camera.position;
		p.y = transform.position.y;
		transform.LookAt(p);
	}
}
