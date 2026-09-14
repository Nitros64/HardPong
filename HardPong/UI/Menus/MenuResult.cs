using System;

namespace HardPong;

internal enum MenuResultKind
{
    None,
    Cancelled,
    Confirmed
}

internal readonly record struct MenuResult
{
    private MenuResult(MenuResultKind kind, int? selectedIndex)
    {
        Kind = kind;
        SelectedIndex = selectedIndex;
    }

    public MenuResultKind Kind { get; }
    public int? SelectedIndex { get; }

    public static MenuResult None => default;
    public static MenuResult Cancelled => new(MenuResultKind.Cancelled, null);

    // El indice confirmado empieza en cero, igual que MenuSelection.SelectedIndex.
    public static MenuResult Confirmed(int selectedIndex)
    {
        if (selectedIndex < 0)
            throw new ArgumentOutOfRangeException(nameof(selectedIndex));

        return new MenuResult(MenuResultKind.Confirmed, selectedIndex);
    }
}
