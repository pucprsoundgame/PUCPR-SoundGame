using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// This class is a simple example of how to build a controller that interacts with PlatformerMotor2D.
/// </summary>
[RequireComponent(typeof(PlatformerMotor2D), typeof(Animator))]
public class PlayerController2D : MonoBehaviour
{
    private PlatformerMotor2D _motor;
    private bool _restored = true;
    private bool _enableOneWayPlatforms;
    private bool _oneWayPlatformsAreWalls;

	[SerializeField] private Tilemap _colidableTilemap;					// tilemap that player walk and collides.
	[SerializeField] private FootstepsDatabase _footstepTilemap;		// TilemapFootsteps component int the Tilemap object.
	private Animator _animator;

	[SerializeField] private GameObject _bulletPrefab;
	[SerializeField] private Transform _bulletSpawn;

	[SerializeField] private AudioSource _jumpAudioSource;
	[SerializeField] private AudioSource _landAudioSource;


	// Use this for initialization
	void Start()
    {
        _motor = GetComponent<PlatformerMotor2D>();
		_animator = GetComponent<Animator>();
    }

    // before enter en freedom state for ladders
    void FreedomStateSave(PlatformerMotor2D motor)
    {
        if (!_restored) // do not enter twice
            return;

        _restored = false;
        _enableOneWayPlatforms = _motor.enableOneWayPlatforms;
        _oneWayPlatformsAreWalls = _motor.oneWayPlatformsAreWalls;
    }
    // after leave freedom state for ladders
    void FreedomStateRestore(PlatformerMotor2D motor)
    {
        if (_restored) // do not enter twice
            return;

        _restored = true;
        _motor.enableOneWayPlatforms = _enableOneWayPlatforms;
        _motor.oneWayPlatformsAreWalls = _oneWayPlatformsAreWalls;
    }

    // Update is called once per frame
    void Update()
    {
		// Press F12 to reload the first scene.
		if (Input.GetKeyDown(KeyCode.F12))
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene(0);
		}
		UpdateMovement();
	}

	private void UpdateMovement()
	{
		// use last state to restore some ladder specific values
		if (_motor.motorState != PlatformerMotor2D.MotorState.FreedomState)
		{
			// try to restore, sometimes states are a bit messy because change too much in one frame
			FreedomStateRestore(_motor);
		}

		// Jump?
		// If you want to jump in ladders, leave it here, otherwise move it down
		if (Input.GetButtonDown(PC2D.Input.JUMP))
		{
			_motor.Jump();
			_motor.DisableRestrictedArea();
		}

		_motor.jumpingHeld = Input.GetButton(PC2D.Input.JUMP);

		// XY freedom movement
		if (_motor.motorState == PlatformerMotor2D.MotorState.FreedomState)
		{
			_motor.normalizedXMovement = Input.GetAxis(PC2D.Input.HORIZONTAL);
			_motor.normalizedYMovement = Input.GetAxis(PC2D.Input.VERTICAL);

			return; // do nothing more
		}

		// X axis movement
		if (Mathf.Abs(Input.GetAxis(PC2D.Input.HORIZONTAL)) > PC2D.Globals.INPUT_THRESHOLD)
		{
			_motor.normalizedXMovement = Input.GetAxis(PC2D.Input.HORIZONTAL);
		}
		else
		{
			_motor.normalizedXMovement = 0;
		}

		if (Input.GetAxis(PC2D.Input.VERTICAL) != 0)
		{
			bool up_pressed = Input.GetAxis(PC2D.Input.VERTICAL) > 0;
			if (_motor.IsOnLadder())
			{
				if (
					(up_pressed && _motor.ladderZone == PlatformerMotor2D.LadderZone.Top)
					||
					(!up_pressed && _motor.ladderZone == PlatformerMotor2D.LadderZone.Bottom)
				 )
				{
					// do nothing!
				}
				// if player hit up, while on the top do not enter in freeMode or a nasty short jump occurs
				else
				{
					// example ladder behaviour

					_motor.FreedomStateEnter(); // enter freedomState to disable gravity
					_motor.EnableRestrictedArea();  // movements is retricted to a specific sprite bounds

					// now disable OWP completely in a "trasactional way"
					FreedomStateSave(_motor);
					_motor.enableOneWayPlatforms = false;
					_motor.oneWayPlatformsAreWalls = false;

					// start XY movement
					_motor.normalizedXMovement = Input.GetAxis(PC2D.Input.HORIZONTAL);
					_motor.normalizedYMovement = Input.GetAxis(PC2D.Input.VERTICAL);
				}
			}
		}
		else if (Input.GetAxis(PC2D.Input.VERTICAL) < -PC2D.Globals.FAST_FALL_THRESHOLD)
		{
			_motor.fallFast = false;
		}

		if (Input.GetButtonDown(PC2D.Input.DASH))
		{
			_motor.Dash();
		}

		CheckSpawnBullet();

	}

	private void CheckSpawnBullet()
	{
		// Fire
		if (Input.GetButtonDown("Fire1"))
		{
			GameObject createdObj = Instantiate(_bulletPrefab, _bulletSpawn.position, Quaternion.identity);
			if (_motor.facingLeft)
			{
				createdObj.transform.localScale *= (-1);
			}
		}

	}

	/// <summary>
	/// Play footstep sound.
	/// </summary>
	public void FootstepSound()
	{
		var tilepos = _colidableTilemap.WorldToCell(transform.position + new Vector3(0, -0.1f, 0));

		if (_colidableTilemap.GetTile(tilepos))
		{
			FootstepsDatabase.Instance.PlayFootstepOfTile(_colidableTilemap.GetTile(tilepos).name.ToLower());
		}

	}

	public void PlayJumpSound()
	{
		if (_jumpAudioSource != null && _jumpAudioSource.clip != null)
		{
			_jumpAudioSource.Play();
		}
	}

}
