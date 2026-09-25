using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class LevelObject : MonoBehaviour
{
    public Boolean beaten;
    public Boolean available;
    public String LevelName;

    private Sprite notAvailable;
    private Sprite isAvailable;
    private Sprite isBeaten;

    private SpriteRenderer sp;
    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        LevelName = gameObject.name;

        notAvailable = Resources.Load<Sprite>("Sprites/Stone");
        isAvailable = Resources.Load<Sprite>("Sprites/SoulFragment");
        isBeaten = Resources.Load<Sprite>("Sprites/Diamond");

        if (LevelManager.Instance.isAvailable(LevelName) )
        {
            sp.sprite = isAvailable;
        }
        else
        {
            sp.sprite = notAvailable;
        }

        if (LevelManager.Instance.isBeaten(LevelName))
        {
            sp.sprite = isBeaten;
        }
        
            
    }

    
    void Update()
    {
        
    }
}
