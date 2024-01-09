using System.Collections.Generic;

namespace Unity1week202312
{
    public class ChapterTitleContentRepository
    {
        private readonly IReadOnlyList<ChapterTitleContent> _chapterTitleContents;
        
        public ChapterTitleContentRepository(IReadOnlyList<ChapterTitleContent> chapterTitleContents)
        {
            _chapterTitleContents = chapterTitleContents;
        }
        
        public ChapterTitleContent Get(int id)
        {
            return _chapterTitleContents[id];
        }
    }
}