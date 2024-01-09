using System;
using Kameffee.Scenes;
using UnityEngine;

namespace Unity1week202312.Result
{
    [Serializable]
    public class ResultSceneData : ISceneData
    {
        [SerializeField]
        private ResultType _resultType;

        public ResultType ResultType => _resultType;
        
        public ResultSceneData(ResultType resultType)
        {
            _resultType = resultType;
        }
    }
}