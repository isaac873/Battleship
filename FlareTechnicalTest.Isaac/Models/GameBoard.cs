using System.Collections.Generic;
using System.Linq;

namespace FlareTechnicalTest.Isaac.Models
{
    public class GameBoard
    {
        public List<Square> Squares { get; set; }

        public GameBoard()
        {
            Squares = new List<Square>();

            for (int i = 0; i <= 9; i++)
            {
                for (int j = 0; j <= 9; j++)
                {
                    Squares.Add(new Square
                    {
                        Coordinates = new CoordinateModel
                        {
                            XCoord = i,
                            YCoord = j
                        }
                    });
                }
            }
        }

        public Square GetSquareByCoordinates(int xCoord, int yCoord)
        {
            return Squares.FirstOrDefault(p =>
                p.Coordinates.XCoord == xCoord &&
                p.Coordinates.YCoord == yCoord);
        }
    }
}
