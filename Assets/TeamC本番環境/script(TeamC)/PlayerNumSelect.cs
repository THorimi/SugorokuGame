using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamC {
    public class PlayerNumSelect : MonoBehaviour
    {
        public int playerNum;
        public void OnValueChanged(int value)//�l�X�V��̏���
        {
            Debug.Log($"{value}�Ԗڂ̗v�f���I�΂ꂽ");
            playerNum = value;
        }
        private void Start()
        {
            playerNum = 0;
        }
    }
}


