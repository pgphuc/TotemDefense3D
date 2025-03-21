using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UICanvas : MonoBehaviour
{
    [SerializeField] bool isDestroyOnClose = false;

    // private void Awake()//Công thức để xử lý kích thước màn hình phù hợp với điện thoại iphone (có tai thỏ)
    // {
    //     RectTransform rect = GetComponent<RectTransform>();
    //     float ratio = (float)Screen.width / (float)Screen.height;
    //     if (ratio > 1f)
    //     {
    //         Vector2 leftBottom = rect.offsetMin;
    //         Vector2 rightTop = rect.offsetMax;
    //         leftBottom.y = 0f;
    //         rightTop.y = -100f;
    //         rect.offsetMin = leftBottom;
    //         rect.offsetMax = rightTop;
    //     }
    // }
    
    public virtual void Setup()//Before active canvas
    {
        
    }

    public virtual void Open()//Active canvas
    {
        gameObject.SetActive(true);
    }

    public virtual void Close(float time)//De-active canvas after an amount of time (second)
    {
        Invoke(nameof(CloseDirectly), time);
    }

    public virtual void CloseDirectly()//De-active canvas immediately
    {
        if (isDestroyOnClose)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
