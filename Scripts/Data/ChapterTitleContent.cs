using System;
using UnityEngine;

namespace Unity1week202312
{
    [Serializable]
    public class ChapterTitleContent
    {
        public int Id => _id;
        public string Title => _title;
        public GameObject Content => _content;

        [SerializeField]
        private int _id;
        
        [SerializeField]
        private string _title;

        [SerializeField]
        private GameObject _content;
    }
}