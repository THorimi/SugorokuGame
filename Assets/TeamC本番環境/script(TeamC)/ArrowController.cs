using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamC
{
        public class ArrowController : MonoBehaviour
    {
        [SerializeField] public float positionPath;

        public enum SelectPath
        {
            PathA,
            PathB,
            PathC,
        }
        public SelectPath selectPath;
    }
}
