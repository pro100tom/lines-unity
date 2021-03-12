using System.Collections.Generic;
using System.Linq;
using Lines.Scripts.BoardStuff.Models;
using Lines.Scripts.General.Models;
using UnityEngine;

namespace Lines.Scripts.BallStuff.Managers
{
    public class BallPopManager
    {
        // Constructor arguments
        public Board Board { get; protected set; }
        
        public BallPopManager(Board board)
        {
            Board = board;
        }
        
        public bool TryPopLines()
        {
            var ballsToPop = new HashSet<GameObject>(ScanHorizontally());
            ballsToPop.UnionWith(ScanVertically());
            ballsToPop.UnionWith(ScanDiagonallyFromTopLeft());
            ballsToPop.UnionWith(ScanDiagonallyFromTopRight());

            if (!ballsToPop.Any())
                return false;

            foreach (var ball in ballsToPop) {
                Board.TryPopBall(ball);
            }
            
            return true;
        }

        protected IEnumerable<GameObject> ScanHorizontally()
        {
            var ballsToPop = new HashSet<GameObject>();
            var dimension = Board.BoardSettings.GetDimension();

            for (dynamic rowIndex = 0, potentialBallsToPop = new HashSet<GameObject>(); 
                rowIndex < dimension.GetRowCount(); 
                rowIndex++, potentialBallsToPop.Clear()
            ) {
                for (dynamic columnIndex = 0, previousBallColor = null, currentBallColor = null;
                    columnIndex < dimension.GetColumnCount();
                    columnIndex++, previousBallColor = currentBallColor, currentBallColor = null
                ) {
                    if (Board.TryGetBall(new Index2D(columnIndex, rowIndex), out var ball)) {
                        currentBallColor = GetBallColor(ball);
                        if (!currentBallColor.Equals(previousBallColor))
                            potentialBallsToPop.Clear();

                        potentialBallsToPop.Add(ball);
                    }

                    if (potentialBallsToPop.Count > 4)
                        ballsToPop.UnionWith(potentialBallsToPop);
                }
            }

            return ballsToPop;
        }

        protected IEnumerable<GameObject> ScanVertically()
        {
            var ballsToPop = new HashSet<GameObject>();
            var dimension = Board.BoardSettings.GetDimension();

            for (dynamic columnIndex = 0, potentialBallsToPop = new HashSet<GameObject>(); 
                columnIndex < dimension.GetColumnCount(); 
                columnIndex++, potentialBallsToPop.Clear()
            ) {
                for (dynamic rowIndex = 0, previousBallColor = null, currentBallColor = null;
                    rowIndex < dimension.GetRowCount();
                    rowIndex++, previousBallColor = currentBallColor, currentBallColor = null
                ) {
                    if (Board.TryGetBall(new Index2D(columnIndex, rowIndex), out var ball)) {
                        currentBallColor = GetBallColor(ball);
                        if (!currentBallColor.Equals(previousBallColor))
                            potentialBallsToPop.Clear();

                        potentialBallsToPop.Add(ball);
                    }

                    if (potentialBallsToPop.Count > 4)
                        ballsToPop.UnionWith(potentialBallsToPop);
                }
            }

            return ballsToPop;
        }

        protected IEnumerable<GameObject> ScanDiagonallyFromTopLeft()
        {
            var ballsToPop = new HashSet<GameObject>();
            var dimension = Board.BoardSettings.GetDimension();
            var rowCount = dimension.GetRowCount();
            var columnCount = dimension.GetColumnCount();

            for (dynamic columnIndex = -rowCount, potentialBallsToPop = new HashSet<GameObject>(); 
                columnIndex < columnCount; 
                columnIndex++, potentialBallsToPop.Clear()
            ) {
                for (dynamic offset = 0, previousBallColor = null, currentBallColor = null;
                    offset < columnCount; 
                    offset++, previousBallColor = currentBallColor, currentBallColor = null
                ) {
                    int slotColumnIndex = columnIndex + offset;
                    int slotRowIndex = offset;
                    
                    if (Board.TryGetBall(new Index2D(slotColumnIndex, slotRowIndex), out var ball)) {
                        currentBallColor = GetBallColor(ball);
                        if (!currentBallColor.Equals(previousBallColor))
                            potentialBallsToPop.Clear();

                        potentialBallsToPop.Add(ball);
                    }
                    
                    if (potentialBallsToPop.Count > 4)
                        ballsToPop.UnionWith(potentialBallsToPop);
                }
            }

            return ballsToPop;
        }

        protected IEnumerable<GameObject> ScanDiagonallyFromTopRight()
        {
            var ballsToPop = new HashSet<GameObject>();
            var dimension = Board.BoardSettings.GetDimension();
            var rowCount = dimension.GetRowCount();
            var columnCount = dimension.GetColumnCount();

            for (dynamic columnIndex = columnCount + rowCount, potentialBallsToPop = new HashSet<GameObject>(); 
                columnIndex >= 0;
                columnIndex--, potentialBallsToPop.Clear()
            ) {
                for (dynamic offset = 0, previousBallColor = null, currentBallColor = null;
                    offset < columnCount; 
                    offset++, previousBallColor = currentBallColor, currentBallColor = null
                ) {
                    int slotColumnIndex = columnIndex - offset;
                    int slotRowIndex = offset;
                    
                    if (Board.TryGetBall(new Index2D(slotColumnIndex, slotRowIndex), out var ball)) {
                        currentBallColor = GetBallColor(ball);
                        if (!currentBallColor.Equals(previousBallColor))
                            potentialBallsToPop.Clear();

                        potentialBallsToPop.Add(ball);
                    }
                    
                    if (potentialBallsToPop.Count > 4)
                        ballsToPop.UnionWith(potentialBallsToPop);
                }
            }

            return ballsToPop;
        }

        protected Color32 GetBallColor(GameObject ball)
        {
            return ball.GetComponent<Renderer>().material.color;
        }
    }
}
