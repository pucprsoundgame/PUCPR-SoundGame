using System.Collections;
using UnityEngine;


public class Bullet : MonoBehaviour { 

	[SerializeField] private float _speed = 1f;
	private float _damage = 10f;

	private int _direction = 1;

	private RaycastHit2D _raycastHit2d;

	[SerializeField] private LayerMask _layerMask;

	private WaitForSeconds _wait;

	[SerializeField] private GameObject _hitObj;

	private bool _colided = false;


	private void Start()
	{
		_wait = new WaitForSeconds(0.1f);
		if (transform.localScale.x > 0)
		{
			_direction = 1;
		}
		else
		{
			_direction = -1;
		}
	}
	

	private void Update()
	{

		_raycastHit2d = Physics2D.Raycast(transform.position, Vector2.right * _direction, _speed * Time.deltaTime, _layerMask);

		if (!_colided)
		{
			if (_raycastHit2d.transform != null)
			{
				_colided = true;
				_raycastHit2d.transform.SendMessage("TakeDamage", _damage, SendMessageOptions.DontRequireReceiver);
				StartCoroutine(DestroyEffect());
			}
			else
			{
				transform.Translate((Vector3)(Vector2.right * _direction) * _speed * Time.deltaTime);
			}
		}

	}

	private IEnumerator DestroyEffect()
	{
		_hitObj.SetActive(true);
		yield return _wait;
		Destroy(gameObject);
	}

}
