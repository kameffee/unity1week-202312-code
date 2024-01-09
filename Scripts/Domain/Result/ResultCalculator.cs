using System;
using System.Linq;
using System.Text;
using Unity1week202312.PlaceBoard.History;
using Unity1week202312.PlaceObjects;
using UnityEngine;

namespace Unity1week202312.Result
{
    public class ResultCalculator
    {
        private readonly PlaceObjectMasterDataRepository _placeObjectMasterDataRepository;

        public ResultCalculator(PlaceObjectMasterDataRepository placeObjectMasterDataRepository)
        {
            _placeObjectMasterDataRepository = placeObjectMasterDataRepository;
        }

        public ResultData Calculate(PlaceBoardHistory history)
        {
            var placeBoardRecord = history.GetLastRecord();
            var group = placeBoardRecord.PackedPlaceObjects
                .Select(packed => _placeObjectMasterDataRepository.Get(packed.PlaceObject.Id))
                .GroupBy(master => master.Genre);

            // TODO: ポイントで計算する
            // 一番多いジャンルを取得
            var orderedSumPoint = group
                .Select(data => (data, totalPoint: data.Sum(masterData => masterData.Point)))
                .OrderByDescending(x => x.totalPoint)
                .ToArray();

            StringBuilder sb = new();
            sb.AppendLine("[ResultCalculator.Calculate]");
            foreach (var genre in orderedSumPoint)
            {
                sb.AppendLine($"Genre: {genre.data.Key}, Count: {genre.data.Count()}, TotalPoint: {genre.totalPoint}");
            }

            Debug.Log(sb.ToString());

            var resultType = ConvertToResultType(orderedSumPoint.First().data.Key);
            return new ResultData(resultType);
        }

        private ResultType ConvertToResultType(Genre genre)
        {
            switch (genre)
            {
                case Genre.None:
                    return ResultType.Normal;
                case Genre.Music:
                    return ResultType.Musician;
                case Genre.Illustration:
                    return ResultType.Illustrator;
                default:
                    throw new ArgumentOutOfRangeException(nameof(genre), genre, "不正な値です");
            }
        }
    }
}