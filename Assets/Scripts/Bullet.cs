using System.Collections;
using UnityEngine;


public class Bullet : MonoBehaviour
{

	[SerializeField] private float _speed = 1f;
	private float _damage = 10f;

	private int _direction = 1;

	private RaycastHit2D _raycastHit2d;

	[SerializeField] private LayerMask _layerMask;

	private float _timeToDestroyBullet = 3f;

	[SerializeField] private GameObject _hitObj;

	private bool _colided = false;


	private void Start()
	{
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
		if (_colided)
		{
			return;
		}

		_raycastHit2d = Physics2D.Raycast(transform.position, Vector2.right * _direction, _speed * Time.deltaTime, _layerMask);

		transform.Translate((Vector3)(Vector2.right * _direction) * _speed * Time.deltaTime);

		if (_raycastHit2d.transform != null)
		{
			Debug.Log($"{name} collided with: {_raycastHit2d.transform.name}");
			_colided = true;
			_raycastHit2d.transform.SendMessage("TakeDamage", _damage, SendMessageOptions.DontRequireReceiver);
			DestroyEffect();
		}
		

	}

	private void DestroyEffect()
	{
		_hitObj.SetActive(true);
		_hitObj.transform.parent = null;

		Destroy(gameObject);
		Destroy(_hitObj, _timeToDestroyBullet);
	}

}
