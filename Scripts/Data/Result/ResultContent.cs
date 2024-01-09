using System;
using UnityEngine;

namespace Unity1week202312.Result
{
    [Serializable]
    public class ResultContent
    {
        public ResultType Type => _type;
        public GameObject Content => _content;
        
        [SerializeField]
        private ResultType _type;

        [SerializeField]
        private GameObject _content;
    }
}