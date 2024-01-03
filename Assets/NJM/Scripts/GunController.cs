using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] private Gun _currentGun;
    [SerializeField] private Vector3 _originPos;

    private float _currentFireRate;
    private bool isReload = false;
    private AudioSource _audioSource;  // 발사 소리 재생기
    private RaycastHit hitInfo;  // 총알의 충돌 정보

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }


    void Update()
    {
        GunFireRateCalc();
        TryFire();
        TryReload();
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
        if (!isReload)
        {
            if (_currentGun.CurrentBulletCount > 0)
                Shoot();
            else
                StartCoroutine(COReload());
        }
    }

    private void Shoot()
    {
        _currentGun.CurrentBulletCount--;
        _currentFireRate = _currentGun.FireRate;
        PlaySE(_currentGun.Fire_Sound);
        _currentGun.MuzzleFlash.Play();
        _currentGun.Anim.SetTrigger("Fire");

        // 피격 처리
        Hit();

        // 총기 반동 코루틴 실행
        StopAllCoroutines();
        StartCoroutine(CORetroAction());

        Debug.Log("총알 발사 함");
    }

    private void TryReload()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isReload && _currentGun.CurrentBulletCount < _currentGun.ReloadBulletCount)
        {
            StartCoroutine(COReload());
        }
    }

    private void Hit()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * _currentGun.Range, Color.blue, 0.3f);
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, _currentGun.Range)) // 카메라 월드좌표
        {
            GameObject clone = Instantiate(_currentGun.HitEffectPrefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));

            Debug.Log(hitInfo.transform.name);
        }
    }

    IEnumerator COReload()
    {
        if (_currentGun.CarryBulletCount > 0)
        {
            isReload = true;
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

            isReload = false;
        }
        else
        {
            Debug.Log("소유한 총알이 없습니다.");
        }
    }

    IEnumerator CORetroAction()
    {
        Vector3 recoilBack = new Vector3(_currentGun.RetroActionForce, _originPos.y, _originPos.z);     

        _currentGun.transform.localPosition = _originPos;

        // 반동 시작
        while (_currentGun.transform.localPosition.x <= _currentGun.RetroActionForce - 0.02f)
        {
            _currentGun.transform.localPosition = Vector3.Lerp(_currentGun.transform.localPosition, recoilBack, 0.4f);
            yield return null;
        }

        // 원위치
        while (_currentGun.transform.localPosition != _originPos)
        {
            _currentGun.transform.localPosition = Vector3.Lerp(_currentGun.transform.localPosition, _originPos, 0.1f);
            yield return null;
        }
    }

    private void PlaySE(AudioClip _clip)
    {
        _audioSource.clip = _clip;
        _audioSource.Play();
    }
}