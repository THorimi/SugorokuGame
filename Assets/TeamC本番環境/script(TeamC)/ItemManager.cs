using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//アイテムデータ
public class ItemData
{
    public string id;   //アイテムid

    public int count;  //所持数

    //コンストラクタ
    public ItemData(string id, int count = 1)
    {
        this.id = id;
        this.count = count;
    }

    //所持数カウントアップ
    public void CountUp(int value = 1)
    {
        count += value;
    }

    //所持数カウントダウン
    public void CountDown(int value = 1)
    {
        count -= value;
    }
}

//アイテム管理クラス
public class ItemManager : MonoBehaviour
{
    [SerializeField] private List<ItemSourceData> _itemSourceDataList;  //アイテムソースリスト

    private List<ItemData> _playerItemDataList = new List<ItemData>();  //プレイヤーの所持アイテム


    private void Awake()
    {
        LoadItemSourceData();
    }

    //アイテムをロードする
    private void LoadItemSourceData()
    {
        _itemSourceDataList = Resources.LoadAll("Items", typeof(ItemSourceData)).Cast<ItemSourceData>().ToList();
    }

    //アイテムソースデータを取得
    public ItemSourceData GetItemSourceData(string id)
    {
        //アイテムを検索
        foreach (var sourceData in _itemSourceDataList)
        {
            //IDが一致していたら
            if (sourceData.id == id)
            {
                return sourceData;
            }
        }
        return null;
    }


    //アイテムを取得
    public void CountItem(string itemId, int count)
    {
        for (int i = 0; i < _playerItemDataList.Count; i++)
        {
            //IDが一致していたらカウント
            if (_playerItemDataList[i].id == itemId)
            {
                _playerItemDataList[i].CountUp(count);
                break;
            }
        }

        //IDが一致しなければアイテムを追加
        ItemData itemData = new ItemData(itemId, count);
        _playerItemDataList.Add(itemData);
    }
}