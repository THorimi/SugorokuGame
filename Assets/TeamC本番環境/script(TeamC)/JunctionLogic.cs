using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamC
{ 
    public class JunctionLogic : MonoBehaviour
    {
        GameObject[] arrows;
        // Start is called before the first frame update
        void Start()
        {
            arrows = GameObject.FindGameObjectsWithTag("Arrow"); 
            foreach (GameObject arrow in arrows)
            {
                arrow.SetActive(false);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
        // 矢印を表示
        public void ArrowActive(GameObject panel)
        {
            // 子オブジェクトの数を取得
            int childCount = panel.transform.childCount;
            // 子オブジェクトを順に取得する
            for (int i = 0; i < childCount; i++)
            {
                Transform childTransform = panel.transform.GetChild(i);
                GameObject childObject = childTransform.gameObject;

                // 取得した子オブジェクトを処理する
                childObject.SetActive(true);
            }
        }
        // 矢印を非表示
        public void ArrowHide(GameObject panel)
        {
            // 子オブジェクトの数を取得
            int childCount = panel.transform.childCount;
            // 子オブジェクトを順に取得する
            for (int i = 0; i < childCount; i++)
            {
                Transform childTransform = panel.transform.GetChild(i);
                GameObject childObject = childTransform.gameObject;

                // 取得した子オブジェクトを処理する
                childObject.SetActive(false);
            }
        }
    }
}
