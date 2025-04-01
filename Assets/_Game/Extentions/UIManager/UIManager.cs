using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    Dictionary<Type, UICanvas> CanvasPrefabs = new Dictionary<Type, UICanvas>();
    Dictionary<Type, UICanvas> ActiveCanvases = new Dictionary<System.Type, UICanvas>();
    [SerializeField] Transform UIParent;

    protected override void Awake()
    {
        base.Awake();
        //Load UI Prefab từ resouces
        UICanvas[] canvas = Resources.LoadAll<UICanvas>("UI/");
        for (int i = 0; i < canvas.Length; i++)
        {
            CanvasPrefabs.Add(canvas[i].GetType(), canvas[i]);
        }
    }
    
    public T OpenUI<T>() where T : UICanvas//Canvas SetActive = true
    {
        T canvas = GetUI<T>();
        
        canvas.Setup();
        canvas.Open();
        
        return canvas;
    }

    

   
    public T GetUI<T>() where T : UICanvas//Lấy UI canvas
    {
        if (!IsUILoaded<T>())//UI hasn't been created
        {
            T prefab = GetUIPrefab<T>();
            T canvas = Instantiate(prefab, UIParent);
            ActiveCanvases[typeof(T)] = canvas;
        }
        return ActiveCanvases[typeof(T)] as T;
    }
    
    public bool IsUILoaded<T>() where T : UICanvas//Check if Canvas is Instantiated
    {
        return ActiveCanvases.ContainsKey(typeof(T)) && ActiveCanvases[typeof(T)] != null;
    }
     
    private T GetUIPrefab<T>() where T : UICanvas//Get UI prefab
    {
        return CanvasPrefabs[typeof(T)] as T;
    }
    
    
    
    public void CloseUI<T>(float time) where T : UICanvas//Canvas SetActive = false after an amount of time (second)
    {
        Invoke(nameof(CloseUIDirectly), time);
    }
    
    public void CloseUIDirectly<T>() where T : UICanvas//Canvas SetActive = false immediately
    {
        if (IsUIOpened<T>())
        {
            ActiveCanvases[typeof(T)].CloseDirectly();
        }
    }
    

    public bool IsUIOpened<T>() where T : UICanvas//Check if Canvas is Active
    {
        return IsUILoaded<T>() && ActiveCanvases[typeof(T)].gameObject.activeSelf;
    }
    public void CloseAllUI()//SetActive = false for all canvases
    {
        foreach (var canvas in ActiveCanvases)
        {
            if (canvas.Value != null && canvas.Value.gameObject.activeSelf)
            {
                canvas.Value.CloseDirectly();
            }
        }
    }
}
