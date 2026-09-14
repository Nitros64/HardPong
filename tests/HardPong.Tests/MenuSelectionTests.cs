using System;
using Xunit;

namespace HardPong.Tests;

public class MenuSelectionTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_RejectsNonPositiveOptionCount(int optionCount)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new MenuSelection(optionCount));
    }

    [Fact]
    public void NewSelection_StartsAtFirstOption()
    {
        Assert.Equal(0, new MenuSelection(4).SelectedIndex);
    }

    [Fact]
    public void MoveNext_CyclesThroughAllOptionsAndWraps()
    {
        var selection = new MenuSelection(3);

        foreach (int expected in new[] { 1, 2, 0, 1, 2, 0 })
        {
            selection.MoveNext();
            Assert.Equal(expected, selection.SelectedIndex);
        }
    }

    [Fact]
    public void MovePrevious_CyclesThroughAllOptionsAndWraps()
    {
        var selection = new MenuSelection(3);

        foreach (int expected in new[] { 2, 1, 0, 2, 1, 0 })
        {
            selection.MovePrevious();
            Assert.Equal(expected, selection.SelectedIndex);
        }
    }

    [Fact]
    public void SingleOption_RemainsSelectedInBothDirections()
    {
        var selection = new MenuSelection(1);

        selection.MoveNext();
        Assert.Equal(0, selection.SelectedIndex);
        selection.MovePrevious();
        Assert.Equal(0, selection.SelectedIndex);
    }

    [Fact]
    public void Reset_AfterNavigation_RestoresFirstOptionAndAllowsNavigation()
    {
        var selection = new MenuSelection(4);
        selection.MovePrevious();
        selection.MovePrevious();
        Assert.Equal(2, selection.SelectedIndex);

        selection.Reset();

        Assert.Equal(0, selection.SelectedIndex);
        selection.MoveNext();
        Assert.Equal(1, selection.SelectedIndex);
        selection.MovePrevious();
        Assert.Equal(0, selection.SelectedIndex);
    }
}
