using UnityEngine;

namespace PC2D
{
    /// <summary>
    /// This is a very very very simple example of how an animation system could query information from the motor to set state.
    /// This can be done to explicitly play states, as is below, or send triggers, float, or bools to the animator. Most likely this
    /// will need to be written to suit your game's needs.
    /// </summary>

    public class PlatformerAnimation2D : MonoBehaviour
    {
        public float jumpRotationSpeed;
        public GameObject visualChild;
		public Animator _animator;

		private PlatformerMotor2D _motor;
        private bool _isJumping;
        private bool _currentFacingLeft;

        // Use this for initialization
        void Start()
        {
            _motor = GetComponent<PlatformerMotor2D>();
            _animator.SetBool("idle", true);

            _motor.onJump += SetCurrentFacingLeft;
        }

        // Update is called once per frame
        void Update()
        {
            if (_motor.motorState == PlatformerMotor2D.MotorState.Jumping ||
                _isJumping &&
                    (_motor.motorState == PlatformerMotor2D.MotorState.Falling ||
                                 _motor.motorState == PlatformerMotor2D.MotorState.FallingFast))
            {
                _isJumping = true;
                _animator.SetBool("inAir", true);

                if (_motor.velocity.x <= -0.1f)
                {
                    _currentFacingLeft = true;
                }
                else if (_motor.velocity.x >= 0.1f)
                {
                    _currentFacingLeft = false;
                }

                Vector3 rotateDir = _currentFacingLeft ? Vector3.forward : Vector3.back;
                visualChild.transform.Rotate(rotateDir, jumpRotationSpeed * Time.deltaTime);
            }
            else
            {
                _isJumping = false;
                visualChild.transform.rotation = Quaternion.identity;
				_animator.SetBool("dash", false);
				_animator.SetBool("inAir", false);
				_animator.SetBool("walk", false);

				if (_motor.motorState == PlatformerMotor2D.MotorState.Falling ||
                                 _motor.motorState == PlatformerMotor2D.MotorState.FallingFast)
                {
					_animator.SetBool("inAir", true);
				}
				//else if (_motor.motorState == PlatformerMotor2D.MotorState.WallSliding ||
    //                     _motor.motorState == PlatformerMotor2D.MotorState.WallSticking)
    //            {
    //                _animator.Play("Cling");
    //            }
    //            else if (_motor.motorState == PlatformerMotor2D.MotorState.OnCorner)
    //            {
    //                _animator.Play("OnCorner");
    //            }
    //            else if (_motor.motorState == PlatformerMotor2D.MotorState.Slipping)
    //            {
    //                _animator.Play("Slip");
    //            }
                else if (_motor.motorState == PlatformerMotor2D.MotorState.Dashing)
                {
					_animator.SetBool("dash", true);
				}
				else
                {
                    if (_motor.velocity.sqrMagnitude >= 0.1f * 0.1f)
                    {
                        _animator.SetBool("walk",true);
                    }
                    else
                    {
                        _animator.SetBool("idle",true);
                    }
                }
            }

            // Facing
            float valueCheck = _motor.normalizedXMovement;

            if (_motor.motorState == PlatformerMotor2D.MotorState.Slipping ||
                _motor.motorState == PlatformerMotor2D.MotorState.Dashing ||
                _motor.motorState == PlatformerMotor2D.MotorState.Jumping)
            {
                valueCheck = _motor.velocity.x;
            }
            
            if (Mathf.Abs(valueCheck) >= 0.1f)
            {
                Vector3 newScale = transform.localScale;
                newScale.x = Mathf.Abs(newScale.x) * ((valueCheck > 0) ? 1.0f : -1.0f);
                transform.localScale = newScale;
            }
        }

        private void SetCurrentFacingLeft()
        {
            _currentFacingLeft = _motor.facingLeft;
        }
    }
}
