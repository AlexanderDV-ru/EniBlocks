using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Mask mask;
    public Image maskImage;
    public Image image;
    public Item item;
    public float width,height;

    public void init()
    {
        mask=gameObject.AddComponent<Mask>();
        maskImage=gameObject.AddComponent<Image>();
        maskImage.sprite=Ids.ins.slotMaskSprite;
        maskImage.rectTransform.sizeDelta=new Vector2(width,height);
        maskImage.rectTransform.anchorMin=maskImage.rectTransform.anchorMax=maskImage.rectTransform.pivot=new Vector2(0,0);
        GameObject go=new GameObject();
        go.transform.parent=transform;
        image=go.AddComponent<Image>();
        image.type=Image.Type.Sliced;
        image.material=Ids.ins.inventoryMat;
        image.rectTransform.sizeDelta=new Vector2(width,height);
        image.rectTransform.localScale=new Vector3(Ids.ins.xTexturesCount,Ids.ins.yTexturesCount,1);
        image.rectTransform.anchorMin=image.rectTransform.anchorMax=image.rectTransform.pivot=new Vector2(0,0);
    }
    public void update(Item item)
    {
        this.item=item;
        image.enabled=item!=null&&item.type.textures.Length!=0;
        //Debug.Log(item);
        //Debug.Log(item!=null?item.type:null);
        //Debug.Log(item!=null?item.type.id:"");
        if(image.enabled)
        {
            image.rectTransform.localPosition=new Vector3((-item.type.textures[0,0].x)*width,(-item.type.textures[0,0].y)*height,0);
            image.rectTransform.sizeDelta=new Vector2((-item.type.textures[0,0].x+item.type.textures[0,2].x)*width,(-item.type.textures[0,0].y+item.type.textures[0,2].y)*height);
        }
    }
}
