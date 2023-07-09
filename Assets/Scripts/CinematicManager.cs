using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicManager : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.PlayMusic("eshopy", 0.5f, 2);
        animator.SetTrigger("next");
    }

    public void GetUsername(string strInput)
    {
        GameManager.instance.m_strPlayer = strInput;
        animator.SetTrigger("next");
    }

    public void FinishIntro()
    {
        ChannelManager.Instance.StartDay();
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Period))
        {
            // Skip intro
            this.gameObject.SetActive(false);
            FinishIntro();
        }
#endif
    }
}
