using UnityEngine;
using UnityEngine.SceneManagement;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

[DisallowMultipleComponent]
public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public string m_strPlayer = "Caillou";
    public Sprite m_spritePlayer;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
}