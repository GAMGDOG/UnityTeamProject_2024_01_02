using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    #region Singleton

    private static Main _instance;
    private static bool _initialized;

    public static Main Instance
    {
        get
        {
            if (!_initialized)
            {
                _initialized = true;

                GameObject obj = GameObject.Find("@Main");
                if (obj == null)
                {
                    obj = new() { name = "@Main" };
                    obj.AddComponent<Main>();

                    obj.AddComponent<AudioSource>();
                    DontDestroyOnLoad(obj);
                    _instance = obj.GetComponent<Main>();
                    _instance._game = GameObject.FindObjectOfType<GameManager>();
                }
            }
            return _instance;
        }
    }
    #endregion
    //new 쓰지 말 것
    private UIManager _ui = new();
    private GameManager _game;
    private SoundManager _sound = new SoundManager();
    private Player _player;

    public static UIManager UI => Instance?._ui;
    public static GameManager Game => Instance?._game;
    public static SoundManager Sound => Instance?._sound;
    public static Player Player
    {
        get
        {
            return Instance?._player == null ? null : Instance?._player;
        }
        set
        {
            Instance._player = value;
        }
    }
}