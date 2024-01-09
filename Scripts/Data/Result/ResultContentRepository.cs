using System.Collections.Generic;
using System.Linq;

namespace Unity1week202312.Result
{
    public class ResultContentRepository
    {
        private readonly IReadOnlyList<ResultContent> _resultContents;

        public ResultContentRepository(IReadOnlyList<ResultContent> resultContents)
        {
            _resultContents = resultContents;
        }

        public ResultContent Get(ResultType type)
        {
            return _resultContents.First(content => content.Type == type);
        }
    }
}