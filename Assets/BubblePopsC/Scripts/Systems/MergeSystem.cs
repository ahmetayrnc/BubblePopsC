using System;
using System.Collections.Generic;
using BubblePopsC.Scripts.Components.Position;
using BubblePopsC.Scripts.Services;
using Entitas;
using UnityEngine;

namespace BubblePopsC.Scripts.Systems
{
    public class MergeSystem : ReactiveSystem<GameEntity>, IInitializeSystem
    {
        private Vector2Int _boardSize;
        private readonly Contexts _contexts;
        private GameEntity[,] _hexMap;
        private IGroup<GameEntity> _bubbleGroup;

        public MergeSystem(Contexts contexts) : base(contexts.game)
        {
            _contexts = contexts;
        }

        public void Initialize()
        {
            _boardSize = _contexts.game.boardSize.Value;
            _hexMap = new GameEntity[_boardSize.x, _boardSize.y];
            _bubbleGroup = _contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Bubble)
                .NoneOf(GameMatcher.Destroyed, GameMatcher.Ghost, GameMatcher.WillBeShotNext));
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.MergeDirty.Added());
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.isMergeDirty;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            var dirtyBubble = entities[0];

            UpdateHexMap();

            var canMerge = GetSpotToMerge(dirtyBubble, out var mergeCoord, out var rest);
            if (!canMerge)
            {
                dirtyBubble.isMergeDirty = false;
                _contexts.game.isMerging = false;
                return;
            }

            _contexts.game.isMerging = true;
            StartMerge(mergeCoord, rest);
        }

        private void UpdateHexMap()
        {
            for (var x = 0; x < _boardSize.x; x++)
            {
                for (var y = 0; y < _boardSize.y; y++)
                {
                    _hexMap[x, y] = null;
                }
            }

            //get the bubbles in an array
            foreach (var bubble in _bubbleGroup)
            {
                var bubbleAxialCoord = bubble.axialCoord.Value;
                var arrayIndices = HexHelperService.GetArrayIndices(bubbleAxialCoord);
                _hexMap[arrayIndices.x, arrayIndices.y] = bubble;
            }
        }

        private bool GetSpotToMerge(GameEntity bubble, out AxialCoord mergeCoord, out List<AxialCoord> rest)
        {
            var cluster = GetBubbleCluster(bubble.axialCoord.Value);
            if (cluster.Count <= 1)
            {
                mergeCoord = bubble.axialCoord.Value;
                rest = new List<AxialCoord>();
                return false;
            }

            mergeCoord = FindMergeSpot(cluster);
            cluster.Remove(mergeCoord);
            rest = cluster;
            return true;
        }

        private AxialCoord FindMergeSpot(List<AxialCoord> cluster)
        {
            var rootIndices = HexHelperService.GetArrayIndices(cluster[0]);
            var rootBubble = _hexMap[rootIndices.x, rootIndices.y];
            var bubbleNumber = rootBubble.bubbleNumber.Value;
            var exponent = (int) Math.Log(bubbleNumber, 2);
            bubbleNumber = (int) Mathf.Pow(2, exponent + cluster.Count - 1);

            var maxRIndex = 0;
            for (var i = 0; i < cluster.Count; i++)
            {
                var axialCoord = cluster[i];
                var neighbours = GetNeighboursWithBubbleNumber(axialCoord, bubbleNumber);

                if (neighbours.Count > 0)
                {
                    return axialCoord;
                }

                if (axialCoord.R > cluster[maxRIndex].R)
                {
                    maxRIndex = i;
                }
            }

            return cluster[maxRIndex];
        }

        private List<AxialCoord> GetBubbleCluster(AxialCoord bubbleCoord)
        {
            var visited = new bool[_boardSize.x, _boardSize.y];
            for (var x = 0; x < _boardSize.x; x++)
            {
                for (var y = 0; y < _boardSize.y; y++)
                {
                    visited[x, y] = false;
                }
            }

            var cluster = new List<AxialCoord>();
            var queue = new Queue<AxialCoord>();

            var rootIndices = HexHelperService.GetArrayIndices(bubbleCoord);
            var rootBubble = _hexMap[rootIndices.x, rootIndices.y];
            var bubbleNumber = rootBubble.bubbleNumber.Value;

            cluster.Add(bubbleCoord);
            queue.Enqueue(bubbleCoord);
            visited[rootIndices.x, rootIndices.y] = true;

            while (queue.Count > 0)
            {
                var testCord = queue.Dequeue();
                var neighbours = HexHelperService.GetNeighbours(testCord);

                foreach (var neighbourCoord in neighbours)
                {
                    var arrayIndices = HexHelperService.GetArrayIndices(neighbourCoord);

                    //bounds check
                    if (arrayIndices.x >= _boardSize.x
                        || arrayIndices.y >= _boardSize.y
                        || arrayIndices.x < 0
                        || arrayIndices.y < 0) continue;

                    //null check
                    if (_hexMap[arrayIndices.x, arrayIndices.y] == null) continue;

                    //visited check
                    if (visited[arrayIndices.x, arrayIndices.y]) continue;
                    visited[arrayIndices.x, arrayIndices.y] = true;

                    //number check
                    if (_hexMap[arrayIndices.x, arrayIndices.y].bubbleNumber.Value != bubbleNumber) continue;

                    queue.Enqueue(neighbourCoord);
                    cluster.Add(neighbourCoord);
                }
            }

            return cluster;
        }

        private List<AxialCoord> GetNeighboursWithBubbleNumber(AxialCoord testedBubbleCoord, int bubbleNumber)
        {
            var neighbourAxialCoords = HexHelperService.GetNeighbours(testedBubbleCoord);
            var canBeMergedNeighbourAxialCoords = new List<AxialCoord>();

            foreach (var axialCoord in neighbourAxialCoords)
            {
                var arrayIndices = HexHelperService.GetArrayIndices(axialCoord);

                //bounds check
                if (arrayIndices.x >= _boardSize.x
                    || arrayIndices.y >= _boardSize.y
                    || arrayIndices.x < 0
                    || arrayIndices.y < 0) continue;

                //null check
                if (_hexMap[arrayIndices.x, arrayIndices.y] == null) continue;

                //number check
                if (_hexMap[arrayIndices.x, arrayIndices.y].bubbleNumber.Value != bubbleNumber) continue;

                canBeMergedNeighbourAxialCoords.Add(axialCoord);
            }

            return canBeMergedNeighbourAxialCoords;
        }

        private void StartMerge(AxialCoord mergeSpot, List<AxialCoord> rest)
        {
            foreach (var axialCoord in rest)
            {
                var indices = HexHelperService.GetArrayIndices(axialCoord);
                var bubble = _hexMap[indices.x, indices.y];
                var bubbleId = bubble.id.Value;

                bubble.AddMergeTo(mergeSpot,
                    () =>
                    {
                        var b = _contexts.game.GetEntityWithId(bubbleId);
                        if (b == null) return;
                        b.isDestroyed = true;
                    });
            }

            var masterIndices = HexHelperService.GetArrayIndices(mergeSpot);
            var master = _hexMap[masterIndices.x, masterIndices.y];
            var masterId = master.id.Value;
            master.AddMergeTo(mergeSpot,
                () =>
                {
                    var b = _contexts.game.GetEntityWithId(masterId);
                    if (b == null) return;
                    b.isDestroyed = true;

                    var bubbleNumber = b.bubbleNumber.Value;
                    var exponent = (int) Math.Log(bubbleNumber, 2);
                    bubbleNumber = (int) Mathf.Pow(2, exponent + rest.Count);
                    bubbleNumber = Mathf.Min(2048, bubbleNumber);

                    var nb = BubbleCreatorService.CreateBoardBubble(mergeSpot, bubbleNumber);
                    nb.isMergeDirty = true;
                });
        }
    }
}