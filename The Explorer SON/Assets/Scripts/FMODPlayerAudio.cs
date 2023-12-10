﻿using FMOD.Studio;
using Gamekit2D;
using UnityEngine;

public class FMODPlayerAudio : MonoBehaviour
{
    [Header("Player")]
    private CharacterController2D _playerController;
    private PlayerCharacter _playerCharacter;

    [SerializeField]
    private bool _isRunning;

    [Header("FMOD Settings")]
    [SerializeField] private string _materialParameterName;
    [SerializeField] private string _speedParameterName;
    [SerializeField] private string _jumpOrLandParameterName;
    [SerializeField] private string _climbParameterName;

    [Header("Footstep Settings")]
    [SerializeField] private float _stepDistance = 2.0f;
    [SerializeField] private float _rayDistance = 1.2f;
    [SerializeField] private float _startRunningTime = 0.3f;

    [Header("Footstep Settings")]
    [SerializeField] private float _climbStepDistance = 1.0f;
    [SerializeField] private float _startClimbingTime = 0.3f;
    public string[] m_materialTypes;

    private float _stepRandom;
    private float _climbStepRandom;
    private Vector3 _prevPos;
    private float _distanceTravelled;
    private float _climbDistanceTravelled;

    private RaycastHit _hit;
    private int _fMaterialValue;

    private bool _previosulyTouchingGround;

    private float _timeTakenSinceStep;
    private float _timeTakenSinceClimbStep;
    private int _fPlayerRunning;

    void Start()
    {
        _stepRandom = Random.Range(0f, 0.5f);
        _climbStepRandom = Random.Range(0f, 0.2f);

        _prevPos = transform.position;

        _playerController = GetComponent<CharacterController2D>();
        _playerCharacter = GetComponent<PlayerCharacter>();
    }

    void Update()
    {
        // Running 
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _playerCharacter.maxSpeed = 10;
            _isRunning = true;
        }
        else
        {
            _playerCharacter.maxSpeed = 7;
            _isRunning = false;
        }

        Debug.DrawRay(transform.position, Vector3.down * _rayDistance, Color.blue);
        
        // Jump or land
        if (_playerController.IsGrounded && _playerCharacter.CheckForJumpInput())
        {
            MaterialCheck(); 
            PlayJumpOrLand(true);
        }
        if (!_previosulyTouchingGround && _playerController.IsGrounded)
        {
            MaterialCheck(); 
            PlayJumpOrLand(false);
        }
        _previosulyTouchingGround = _playerController.IsGrounded;

        // Footsteps
        _timeTakenSinceStep += Time.deltaTime;
        _distanceTravelled += (transform.position - _prevPos).magnitude;

        if (_distanceTravelled >= _stepDistance + _stepRandom)
        {
            MaterialCheck(); 
            SpeedCheck();

            if (_playerController.IsGrounded)
            {
                EventInstance footstep = GameManager.Instance.audioManager.CreateInstance(GameManager.Instance.fmodEvents.GetEvent("Footstep"));
                FMODUnity.RuntimeManager.AttachInstanceToGameObject(footstep, transform, GetComponent<Rigidbody2D>());
                footstep.setParameterByName(_materialParameterName, _fMaterialValue);
                footstep.setParameterByName(_speedParameterName, _fPlayerRunning);
                footstep.start();
            }

            _stepRandom = Random.Range(0f, 0.5f); 
            _distanceTravelled = 0f;
        }

        // Climbing
        if (_playerCharacter.EstoyEscalando)
        {
            _timeTakenSinceClimbStep += Time.deltaTime;
            _climbDistanceTravelled += (transform.position - _prevPos).magnitude;

            if (_climbDistanceTravelled >= _climbStepDistance + _climbStepRandom)
            {
                // We could check the material in case player climbs different types of wall

                // Play climb sound with variation in pitch
                EventInstance climbStep = GameManager.Instance.audioManager.CreateInstance(GameManager.Instance.fmodEvents.GetEvent("Climb"));
                FMODUnity.RuntimeManager.AttachInstanceToGameObject(climbStep, transform, GetComponent<Rigidbody2D>());
                climbStep.setParameterByName(_climbParameterName, UnityEngine.Random.Range(0f, 1f));
                climbStep.start();

                _climbStepRandom = Random.Range(0f, 0.2f);
                _climbDistanceTravelled = 0f;
            }
        }

        _prevPos = transform.position;
    }


    void MaterialCheck()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out _hit, _rayDistance))
        {
            if (_hit.collider.gameObject.GetComponent<FMODMaterialSetter>())
            {
                _fMaterialValue = _hit.collider.gameObject.GetComponent<FMODMaterialSetter>().MaterialValue;
            }
            else
            {
                _fMaterialValue = 0;
            }
        }
        else
        {
            _fMaterialValue = 0;
        }
    }

    void SpeedCheck()
    {
        _fPlayerRunning = _isRunning ? 1 : 0;

        //if (_timeTakenSinceStep < _startRunningTime)
        //{
        //    _fPlayerRunning = 1;
        //}
        //else {
        //    _fPlayerRunning = 0;
        //    _timeTakenSinceStep = 0f;
        //}
    }

    void PlayJumpOrLand(bool F_JumpLandCalc)
    {
        EventInstance jumpOrLand = GameManager.Instance.audioManager.CreateInstance(GameManager.Instance.fmodEvents.GetEvent("JumpLand"));
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(jumpOrLand, transform, GetComponent<Rigidbody2D>()); 
        jumpOrLand.setParameterByName(_materialParameterName, _fMaterialValue);
        jumpOrLand.setParameterByName(_jumpOrLandParameterName, F_JumpLandCalc ? 0f : 1f);
        jumpOrLand.start();
        //jumpOrLand.release();
    }
}

