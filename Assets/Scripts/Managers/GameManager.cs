using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public UnityAction OnStageStart;
    public UnityAction OnStageOver;
    public UnityAction OnKeyGet;

    private PlayerHealth _health;
    private UI_HUDPanel _uiHUD;

    public bool IsClear { get; set; }
    public bool IsStage1Clear { get; set; }

    public void CallStageStart()
    {
        OnStageStart?.Invoke();
    }

    public void CallStageOver()
    {
        OnStageOver?.Invoke();
    }

    public void CallkeyGet()
    {
        OnKeyGet?.Invoke();
    }
    private void Awake()
    {
        _health = GameObject.Find("Player").GetComponent<PlayerHealth>();
        _uiHUD = FindObjectOfType<UI_HUDPanel>();
    }
    private void Start()
    {
        _health.OnDie += GameOver;
    }

    private void GameOver()
    {
        _uiHUD.ShowOverPanel();
    }

    private void Update()
    {

    }
}
