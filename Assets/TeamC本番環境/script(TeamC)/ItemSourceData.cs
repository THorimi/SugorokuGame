using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//アイテムの種類(列挙体)
public enum ItemType
{
    DICE = 0,   //ダイスの目を操作
    COIN = 1,       //コインの数を操作
    WARP = 2,     //ワープする
}

//アイテムのソースデータ
[CreateAssetMenu(menuName = "Items/ItemSourceData")]
public class ItemSourceData : ScriptableObject
{
    //アイテム識別用id
    [SerializeField] private string _id;
    //idを取得
    public string id
    {
        get { return _id; }
    }

    //アイテムの名前
    [SerializeField] private string _itemName;
    //アイテム名を取得
    public string itemName
    {
        get { return _itemName; }
    }

    //アイテムの説明文
    [SerializeField] private string _itemText;
    //アイテム名を取得
    public string itemText
    {
        get { return _itemText; }
    }

    //アイテムの種類
    [SerializeField] private ItemType _itemType;
    //アイテムの種類を取得
    public ItemType itemType
    {
        get { return _itemType; }
    }

    //買値
    [SerializeField] private int _buyingPrice;
    //買値を取得
    public int buyingPrice
    {
        get { return _buyingPrice; }
    }
}