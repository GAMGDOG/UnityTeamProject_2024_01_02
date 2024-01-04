using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [field : SerializeField] public string GunName { get; private set; }  // 총의 이름
    [field: SerializeField] public float Range { get; private set; }      // 총의 사정 거리
    [field: SerializeField] public float FireRate { get; private set; }   // 연사 속도 (한발과 한발간의 시간 텀)
    [field: SerializeField] public float ReloadTime { get; private set; } // 재장전 속도
    [field: SerializeField] public int Damage { get; private set; }       // 총의 공격력
    [field: SerializeField] public float RetroActionForce { get; private set; }   // 반동 세기
    [field: SerializeField] public float RetroActionFineSightForce { get; private set; }    // 정조준 반동 세기
    [field: SerializeField] public float Accuracy { get; private set; } // 정확도
    [field: SerializeField] public float AccuracyFineSight { get; private set; } // 정조준 정확도

    [field: SerializeField] public int ReloadBulletCount { get; private set; }    // 총의 재장전 개수
    public int CurrentBulletCount;  // 현재 탄창에 남아있는 총알의 개수
    [field: SerializeField] public int MaxBulletCount { get; private set; }       // 총알 최대 소유 개수
    public int CarryBulletCount;    // 현재 소유하고 있는 총알의 총 개수

    [field: SerializeField] public Animator Anim { get; private set; }
    [field: SerializeField] public ParticleSystem MuzzleFlash { get; private set; }    // 화염구 이펙트
    [field: SerializeField] public GameObject HitEffectPrefab { get; private set; }    // 총알 피격 이펙트
    [field: SerializeField] public AudioClip Fire_Sound { get; private set; }      // 총 발사 소리 오디오 클립

}
