using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class SpriteAtlasManager : MonoBehaviour
{
    [SerializeField] private SpriteAtlas atlas;
    [SerializeField] private string sprite_name;

    private void Awake()
    {
        GetComponent<Image>().sprite = atlas.GetSprite(sprite_name);
    }


    public void ChangeExpression(int index, string expression)
    {
        GetComponent<Image>().sprite=atlas.GetSprite(index+"_"+expression);
        
    }

}



