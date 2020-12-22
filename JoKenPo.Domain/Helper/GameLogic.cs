using System.Collections.Generic;
using System.Linq;
using JoKenPo.Domain.Enum;

namespace JoKenPo.Domain.Helper
{
    public static class GameLogic
    {
        private static readonly IReadOnlyDictionary<HandSymbol, HandSymbol> WinMovements = new Dictionary<HandSymbol, HandSymbol>
        {
            {HandSymbol.Stone, HandSymbol.Scissor},
            {HandSymbol.Scissor, HandSymbol.Paper},
            {HandSymbol.Paper, HandSymbol.Stone}
        };

        public static bool CheckWin(HandSymbol player1, HandSymbol player2)
        {
            return WinMovements.Where((pair, i) => pair.Key == player1 && pair.Value == player2).Any();
        }
    }
}