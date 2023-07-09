using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DayEnd : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI m_tmpCorrect;
    [SerializeField] private TextMeshProUGUI m_tmpMistakes;
    [SerializeField] private TextMeshProUGUI m_tmpMissed;
    [SerializeField] private TextMeshProUGUI m_tmpTotal;
    [SerializeField] private CinematicManager m_cinematics;


    public void Trigger(ChannelManager.Results results)
    {
        m_tmpCorrect.text = string.Format("Correct: +{0}", results.cCorrect);
        m_tmpMistakes.text = string.Format("Mistakes: -{0}", results.cMistake);
        m_tmpMissed.text = string.Format("Missed: -{0}", results.cMiss);
        m_tmpTotal.text = string.Format("Total: {0}{1}", results.Total() >= 0 ? "+" : "-", Mathf.Abs(results.Total()));

        Animator animator = GetComponent<Animator>();

        animator.SetBool("win", results.Total() >= 0);
        animator.SetTrigger("start");
    }

    public void Restart()
    {
        /*ChannelManager.Instance.Reset();
        m_cinematics.gameObject.SetActive(true);
        m_cinematics.Trigger();*/
        AudioManager.instance.PlayNext();
        ChannelManager.Instance.m_day -= 1;
        ChannelManager.Instance.StartDay();
    }

    public void Continue()
    {
        AudioManager.instance.PlayNext();
        ChannelManager.Instance.StartDay();
    }

    public void PlaySFX(string sfx)
    {
        AudioManager.instance.PlaySFX(sfx);
    }
}
