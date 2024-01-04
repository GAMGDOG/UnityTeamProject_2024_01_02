using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GunController : MonoBehaviour
{
    [SerializeField] private Gun _currentGun;
    [SerializeField] private Vector3 _originPos;
    [SerializeField] private Vector3 _fineSightOriginPos;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    public bool IsFindSightMode { get; private set; }

    private CinemachinePOV _pov;
    private AudioSource _audioSource;  // 발사 소리 재생기
    private RaycastHit _hitInfo;  // 총알의 충돌 정보
    private float _currentFireRate;
    private bool _isReload = false;
    private float _originFOV;



    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _pov = _virtualCamera.GetCinemachineComponent<CinemachinePOV>();
        
    }

    private void Start()
    {
        IsFindSightMode = false;
    }


    void Update()
    {
        if(Time.timeScale != 0)
        {
            GunFireRateCalc();
            TryFire();
            TryReload();
            TryFineSight();
        }
    }

    private void GunFireRateCalc()
    {
        if (_currentFireRate > 0)
            _currentFireRate -= Time.deltaTime; 
    }

    private void TryFire() 
    {
        if (Input.GetButton("Fire1") && _currentFireRate <= 0)
        {
            Fire();
        }
    }

    private void Fire() 
    {
        if (!_isReload)
        {
            if (_currentGun.CurrentBulletCount > 0)
                Shoot();
            else
            {
                CancelFineSight();
                StartCoroutine(COReload());
            }
        }
    }

    private void Shoot()
    {
        _currentGun.CurrentBulletCount--;
        _currentFireRate = _currentGun.FireRate;
        PlaySE(_currentGun.Fire_Sound);
        if (PlayerPrefs.GetInt("GunEffects") == 1) _currentGun.MuzzleFlash.Play();
        _currentGun.Anim.SetTrigger("Fire");

        // 피격 처리
        Hit();

        // 총기 반동 코루틴 실행
        StopAllCoroutines();
        StartCoroutine(CORetroAction());

        Debug.Log("총알 발사");
    }

    private void TryReload()
    {
        if (Input.GetKeyDown(KeyCode.R) && !_isReload && _currentGun.CurrentBulletCount < _currentGun.ReloadBulletCount)
        {
            CancelFineSight();
            StartCoroutine(COReload());
        }
    }

    private void Hit()
    {
        Vector3 randomRange = new Vector3(Random.Range(-_currentGun.Accuracy, _currentGun.Accuracy), Random.Range(-_currentGun.Accuracy, _currentGun.Accuracy), 0);
        Vector3 randomRangeFineSight = new Vector3(Random.Range(-_currentGun.AccuracyFineSight, _currentGun.AccuracyFineSight), Random.Range(-_currentGun.AccuracyFineSight, _currentGun.AccuracyFineSight), 0);

        if (!IsFindSightMode)  // 정조준이 아닌 상태
        {
            Debug.DrawRay(Camera.main.transform.position, (Camera.main.transform.forward + randomRange) * _currentGun.Range, Color.blue, 0.3f);
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward + randomRange, out _hitInfo, _currentGun.Range)) // 카메라 월드좌표
            {
                GameObject clone = Instantiate(_currentGun.HitEffectPrefab, _hitInfo.point, Quaternion.LookRotation(_hitInfo.normal));

                Debug.Log(_hitInfo.transform.name);

            }
        }
        else // 정조준 상태
        {
            Debug.DrawRay(Camera.main.transform.position, (Camera.main.transform.forward + randomRangeFineSight) * _currentGun.Range, Color.blue, 0.3f);
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward + randomRangeFineSight, out _hitInfo, _currentGun.Range)) // 카메라 월드좌표
            {
                GameObject clone = Instantiate(_currentGun.HitEffectPrefab, _hitInfo.point, Quaternion.LookRotation(_hitInfo.normal));

                Debug.Log(_hitInfo.transform.name);
            }
        }

    }

    private void TryFineSight()
    {
        if (Input.GetButtonDown("Fire2") && !_isReload)
        {
            FineSight();
        }
    }

    private void FineSight()
    {
        IsFindSightMode = !IsFindSightMode;
        _currentGun.Anim.SetBool("FineSightMode", IsFindSightMode);

        if (IsFindSightMode)
        {
            StopAllCoroutines();
            StartCoroutine(COFineSightActivate());
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(COFineSightDeActivate());
        }
    }

    private void CancelFineSight()
    {
        if (IsFindSightMode)
            FineSight();
    }

    IEnumerator COFineSightActivate()
    {
        _originFOV = _virtualCamera.m_Lens.FieldOfView;

        while (_currentGun.transform.localPosition != _fineSightOriginPos)
        {
            _currentGun.transform.localPosition = Vector3.Lerp(_currentGun.transform.localPosition, _fineSightOriginPos, 0.2f);
            _virtualCamera.m_Lens.FieldOfView = 30f;

            yield return null;
        }
    }

    IEnumerator COFineSightDeActivate()
    {
        while (_currentGun.transform.localPosition != _originPos)
        {
            _currentGun.transform.localPosition = Vector3.Lerp(_currentGun.transform.localPosition, _originPos, 0.2f);
            _virtualCamera.m_Lens.FieldOfView = _originFOV;
            yield return null;
        }
    }

    IEnumerator COReload()
    {
        if (_currentGun.CarryBulletCount > 0)
        {
            _isReload = true;
            _currentGun.Anim.SetTrigger("Reload");

            _currentGun.CarryBulletCount += _currentGun.CurrentBulletCount;
            _currentGun.CurrentBulletCount = 0;

            yield return new WaitForSeconds(_currentGun.ReloadTime);

            if (_currentGun.CarryBulletCount >= _currentGun.ReloadBulletCount)
            {
                _currentGun.CurrentBulletCount = _currentGun.ReloadBulletCount;
                _currentGun.CarryBulletCount -= _currentGun.ReloadBulletCount;
            }
            else
            {
                _currentGun.CurrentBulletCount = _currentGun.CarryBulletCount;
                _currentGun.CarryBulletCount = 0;
            }

            _isReload = false;
        }
        else
        {
            Debug.Log("소유한 총알이 없습니다.");
        }
    }

    IEnumerator CORetroAction()
    {
        Vector3 recoilBack = new Vector3(_originPos.x, _originPos.y, _originPos.z - _currentGun.RetroActionForce); // 정조준 x
        Vector3 retroActionRecoilBack = new Vector3(_fineSightOriginPos.x, _fineSightOriginPos.y, _fineSightOriginPos.z - _currentGun.RetroActionFineSightForce);  // 정조준

        if (!IsFindSightMode)  // 정조준이 아닌 상태
        {
            _currentGun.transform.localPosition = _originPos;

            // 반동 시작
            while (_currentGun.transform.localPosition.z >= _originPos.z - _currentGun.RetroActionForce + 0.02f)
            {
                _currentGun.transform.localPosition = Vector3.Lerp(_currentGun.transform.localPosition, recoilBack, 0.4f);
                _pov.m_VerticalAxis.Value += -_currentGun.RetroActionForce;
                yield return null;
            }

            // 원위치
            while (_currentGun.transform.localPosition != _originPos)
            {
                _currentGun.transform.localPosition = Vector3.Lerp(_currentGun.transform.localPosition, _originPos, 0.1f);
                yield return null;
            }
        }
        else  // 정조준 상태
        {
            _currentGun.transform.localPosition = _fineSightOriginPos;

            // 반동 시작
            while (_currentGun.transform.localPosition.z >= _fineSightOriginPos.z - _currentGun.RetroActionFineSightForce + 0.02f)
            {
                _currentGun.transform.localPosition = Vector3.Lerp(_currentGun.transform.localPosition, retroActionRecoilBack, 0.4f);
                _pov.m_VerticalAxis.Value += -_currentGun.RetroActionFineSightForce;
                yield return null;
            }

            // 원위치
            while (_currentGun.transform.localPosition != _fineSightOriginPos)
            {
                _currentGun.transform.localPosition = Vector3.Lerp(_currentGun.transform.localPosition, _fineSightOriginPos, 0.1f);
                yield return null;
            }
        }
    }

    private void PlaySE(AudioClip _clip)
    {
        _audioSource.clip = _clip;
        _audioSource.Play();
    }
}