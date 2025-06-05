using System.Linq;
using UnityEngine;

public abstract class ComboTracker
{
    private const int K_defaultComboCount = 1;

    public abstract string Name { get; }

    public int ComboCount => m_comboCount;
    protected int m_comboCount = K_defaultComboCount;

    public abstract bool TryIncreaseCombo(CardPile pile);

    public void Reset()
    {
        m_comboCount = K_defaultComboCount;
    }
}

public class SuitCombo : ComboTracker
{
    public override string Name => $"Same suit combo";

    public override bool TryIncreaseCombo(CardPile pile)
    {
        var pileCards = pile.PeekAllCards();
        if (pileCards[0].suit == pileCards[1].suit)
        {
            m_comboCount++;
            return true;
        }
        return false;
    }
}

public class NumberIncreasingCombo : ComboTracker
{
    public override string Name => "Cards increasing";

    public override bool TryIncreaseCombo(CardPile pile)
    {
        var pileCards = pile.PeekAllCards();
        var secondToLastId = pileCards.Length - 2;
        if (pileCards.Last().numberValue == pileCards[secondToLastId].numberValue + 1)
        {
            m_comboCount++;
            return true;
        }
        return false;
    }
}

public class NumberDecreasingCombo : ComboTracker
{
    public override string Name => "Cards decreasing";

    public override bool TryIncreaseCombo(CardPile pile)
    {
        var pileCards = pile.PeekAllCards();
        var secondToLastId = pileCards.Length - 2;
        if (pileCards.Last().numberValue == pileCards[secondToLastId].numberValue - 1)
        {
            m_comboCount++;
            return true;
        }
        return false;
    }
}

public class SameNumberCombo : ComboTracker
{
    public override string Name => "Same number";

    public override bool TryIncreaseCombo(CardPile pile)
    {
        var pileCards = pile.PeekAllCards();
        var secondToLastId = pileCards.Length - 2;
        if (pileCards.Last().numberValue == pileCards[secondToLastId].numberValue)
        {
            m_comboCount++;
            return true;
        }
        return false;
    }
}