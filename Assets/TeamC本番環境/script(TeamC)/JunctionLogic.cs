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
        // ����\��
        public void ArrowActive(GameObject panel)
        {
            // �q�I�u�W�F�N�g�̐����擾
            int childCount = panel.transform.childCount;
            // �q�I�u�W�F�N�g�����Ɏ擾����
            for (int i = 0; i < childCount; i++)
            {
                Transform childTransform = panel.transform.GetChild(i);
                GameObject childObject = childTransform.gameObject;

                // �擾�����q�I�u�W�F�N�g����������
                childObject.SetActive(true);
            }
        }
        // �����\��
        public void ArrowHide(GameObject panel)
        {
            // �q�I�u�W�F�N�g�̐����擾
            int childCount = panel.transform.childCount;
            // �q�I�u�W�F�N�g�����Ɏ擾����
            for (int i = 0; i < childCount; i++)
            {
                Transform childTransform = panel.transform.GetChild(i);
                GameObject childObject = childTransform.gameObject;

                // �擾�����q�I�u�W�F�N�g����������
                childObject.SetActive(false);
            }
        }
    }
}
