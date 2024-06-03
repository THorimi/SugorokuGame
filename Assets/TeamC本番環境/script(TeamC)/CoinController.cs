using ebihara;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CoinController : MonoBehaviour
{
    private Animator coinAnimator;  // �R�C���t�Đ��ׂ̈�
    [SerializeField] private  GameObject sugorokuGameDirector;
    TeamC.SugorokuGameDirector sgd;
    //[SerializeField] private  GameObject coinUpDownTextObject;
    //TextMeshPro coinUpDownText;


    // �R�C���̌��ʉ�
    AudioSource audioSource;
    [SerializeField] private AudioClip coinUp;
    [SerializeField] private AudioClip coinDown;



    private void Awake()
    {
        //coinUpDownTextObject = GameObject.Find("CoinUpDownText");
        sugorokuGameDirector = GameObject.Find("SugorokuGameDirector");
    }




    // Start is called before the first frame update
    void Start()
    {
        coinAnimator = gameObject.transform.GetComponent<Animator>();
                
        sgd = sugorokuGameDirector.GetComponent<TeamC.SugorokuGameDirector>();
        audioSource = GetComponent<AudioSource>();
        
        if (sgd.coinNum > 0 ) 
        {
            coinAnimator.SetFloat("Speed", 1);
            Invoke("coinUpSe", 0.4f);
        }
        else if (sgd.coinNum < 0)
        {
            coinAnimator.SetFloat("Speed", -1);
            Invoke("coinDownSe", 0.4f);
        }
    }

   
    //���ʉ����Đ�����
    public void coinUpSe()
    {
        audioSource.PlayOneShot(coinUp);
    }

    //����̊֐�2
    public void coinDownSe()
    {
        audioSource.PlayOneShot(coinDown);
    }


}
