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
        Trigger();
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

    public void Trigger()
    {
        AudioManager.instance.PlayNext();
        animator.SetTrigger("next");
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
