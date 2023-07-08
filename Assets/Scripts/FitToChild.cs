using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[ExecuteInEditMode]
[RequireComponent(typeof(LayoutElement), typeof(ContentSizeFitter))]
public class FitToChild : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI target;
    [SerializeField] private ContentSizeFitter.FitMode matchWidth;
    [SerializeField] private ContentSizeFitter.FitMode matchHeight;

    private LayoutElement layoutElement;
    private ContentSizeFitter contentSizeFitter;
    private RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        layoutElement = GetComponent<LayoutElement>();
        contentSizeFitter = GetComponent<ContentSizeFitter>();
        Refresh();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (target)
        {
            Refresh();
        }
    }

    public void Refresh()
    {
        layoutElement.preferredWidth = target.preferredWidth;
        contentSizeFitter.horizontalFit = matchWidth;
        layoutElement.preferredHeight = target.preferredHeight;
        contentSizeFitter.verticalFit = matchHeight;
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
    }
}
