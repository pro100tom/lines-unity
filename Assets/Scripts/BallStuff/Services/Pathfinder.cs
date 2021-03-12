using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Lines.Scripts.BoardStuff.Models;
using Lines.Scripts.General.Models;

namespace Lines.Scripts.Services
{
    public class Pathfinder
    {
        protected int MovementCost { get; } = 10;

        public Pathfinder()
        {
            
        }
        
        public bool TryFindShortestPath(Board board, Index2D slotFrom, Index2D slotTo, out Queue<Index2D> path)
        {
            path = null;
            var allSlots = new List<Index2D>(board.GetSlots());
            var availableSlots = new List<Index2D>(board.GetEmptySlots());
            //if (!availableSlots.Contains(slotTo))
            //    return false;

            //if (availableSlots.Contains(slotFrom))
            //    return false;

            if (!TryGetAvailableNeighbourSlots(board, slotFrom, out var neighbourSlots))
                return false;

            var allNodes = allSlots.Select(s => new Node(s, slotTo)).ToList();
            Node GetNode(Index2D slot) => allNodes.FirstOrDefault(n => n.Slot.Equals(slot));
            IEnumerable<Node> GetNodes(ICollection<Index2D> slots) => allNodes.Where(n => slots.Contains(n.Slot));
            
            var availableNodes = GetNodes(availableSlots).ToList();
            bool IsNodeAvailable(Node node) => availableNodes.Contains(node);

            var startNode = GetNode(slotFrom);
            if (startNode == null)
                return false;
            
            var openNodes = new List<Node> { startNode };
            var closedNodes = new List<Node>();

            while (openNodes.Any()) {
                var minCostF = openNodes.Min(n => n.CostF);
                var closestNodes = openNodes.Where(n => n.CostF == minCostF).ToList();
                if (!closestNodes.Any())
                    continue;
                
                closestNodes.Sort();
                var currentNode = closestNodes.FirstOrDefault();
                if (currentNode == null)
                    continue;

                if (currentNode.Slot.Equals(slotTo)) {
                    var pathReversed = new Queue<Index2D>();
                    while (currentNode != null) {
                        pathReversed.Enqueue(currentNode.Slot);
                        currentNode = currentNode.PointsTo;
                    }
                    
                    path = new Queue<Index2D>(pathReversed.Reverse());
                    
                    return true;
                }

                closedNodes.Add(currentNode);
                openNodes.Remove(currentNode);

                if (!TryGetAvailableNeighbourSlots(board, currentNode.Slot, out neighbourSlots))
                    continue;

                var neighbourNodes = GetNodes(neighbourSlots).Except(closedNodes).Where(IsNodeAvailable).ToList();
                openNodes = openNodes.Union(neighbourNodes).Distinct().ToList();

                foreach (var neighbourNode in neighbourNodes) {
                    var potentialCostG = currentNode.CostG + MovementCost;
                    if (neighbourNode.CostG < potentialCostG && neighbourNode.CostG > 0)
                        continue;

                    neighbourNode.CostG = potentialCostG;
                    neighbourNode.PointsTo = currentNode;
                }
            }

            return false;
        }

        protected bool TryGetAvailableNeighbourSlots(Board board, Index2D slot, out List<Index2D> neighbours)
        {
            neighbours = null;
            if (!board.IsSlotValid(slot))
                return false;
            
            var potentialNeighbours = new List<Index2D> {
                GetSlotOnTheLeft(slot),
                GetSlotOnTheRight(slot),
                GetSlotAbove(slot),
                GetSlotBelow(slot),
            };
            
            neighbours = potentialNeighbours.Where(s => board.IsSlotValid(s) && !board.HasBall(s)).ToList();

            return true;
        }

        protected Index2D GetSlotOnTheLeft(Index2D slot)
        {
            return new Index2D(slot.ColumnIndex - 1, slot.RowIndex);
        }
        
        protected Index2D GetSlotOnTheRight(Index2D slot)
        {
            return new Index2D(slot.ColumnIndex + 1, slot.RowIndex);
        }
        
        protected Index2D GetSlotAbove(Index2D slot)
        {
            return new Index2D(slot.ColumnIndex, slot.RowIndex - 1);
        }
        
        protected Index2D GetSlotBelow(Index2D slot)
        {
            return new Index2D(slot.ColumnIndex, slot.RowIndex + 1);
        }
        
        protected class Node : IComparable
        {
            public Index2D Slot { get; }
            [CanBeNull] public Node PointsTo { get; set; }

            protected int costG;
            protected int costH;

            public Node(Index2D thisSlot, Index2D targetSlot, Node pointsTo = null, int movementCost = 10)
            {
                Slot = thisSlot;
                PointsTo = pointsTo;
                CostH = GetDistance(thisSlot, targetSlot, movementCost);
            }

            protected int GetDistance(Index2D slotStart, Index2D slotEnd, int movementCost)
            {
                return (Math.Abs(slotEnd.ColumnIndex - slotStart.ColumnIndex)
                       + Math.Abs(slotEnd.RowIndex - slotStart.RowIndex)) * movementCost;
            }

            public int CostG
            {
                get => costG;

                set {
                    costG = value;
                    CostF = costG + CostH;
                }
            }

            public int CostH
            {
                get => costH;

                protected set {
                    costH = value;
                    CostF = costG + CostH;
                }
            }
            
            public int CostF { get; protected set; }
            
            public int CompareTo(object obj)
            {
                return ((Node)obj).Slot.CompareTo(Slot);
            }
        }
    }
}
