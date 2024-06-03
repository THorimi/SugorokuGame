using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//�A�C�e���̎��(�񋓑�)
public enum ItemType
{
    DICE = 0,   //�_�C�X�̖ڂ𑀍�
    COIN = 1,       //�R�C���̐��𑀍�
    WARP = 2,     //���[�v����
}

//�A�C�e���̃\�[�X�f�[�^
[CreateAssetMenu(menuName = "Items/ItemSourceData")]
public class ItemSourceData : ScriptableObject
{
    //�A�C�e�����ʗpid
    [SerializeField] private string _id;
    //id���擾
    public string id
    {
        get { return _id; }
    }

    //�A�C�e���̖��O
    [SerializeField] private string _itemName;
    //�A�C�e�������擾
    public string itemName
    {
        get { return _itemName; }
    }

    //�A�C�e���̐�����
    [SerializeField] private string _itemText;
    //�A�C�e�������擾
    public string itemText
    {
        get { return _itemText; }
    }

    //�A�C�e���̎��
    [SerializeField] private ItemType _itemType;
    //�A�C�e���̎�ނ��擾
    public ItemType itemType
    {
        get { return _itemType; }
    }

    //���l
    [SerializeField] private int _buyingPrice;
    //���l���擾
    public int buyingPrice
    {
        get { return _buyingPrice; }
    }
}