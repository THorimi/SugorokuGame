using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamC {
    public class PlayerNumSelect : MonoBehaviour
    {
        public int playerNum;
        public void OnValueChanged(int value)//値更新後の処理
        {
            Debug.Log($"{value}番目の要素が選ばれた");
            playerNum = value;
        }
        private void Start()
        {
            playerNum = 0;
        }
    }
}


