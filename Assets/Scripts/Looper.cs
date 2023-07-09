using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Looper : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private RectTransform rectMe;
    [SerializeField] private RectTransform rectParent;

    void Awake()
    {
        rectMe = GetComponent<RectTransform>();
        rectParent = transform.parent.GetComponent<RectTransform>();
    }
    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        float width = rectParent.rect.width;

        if (transform.localPosition.x < -width/2 - rectMe.rect.width)
        {
            transform.localPosition = new Vector2(width/2, 0);
        }
    }
}
