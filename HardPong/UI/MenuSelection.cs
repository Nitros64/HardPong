using System;

namespace HardPong;

// Estado y navegacion circular de la seleccion, independientes de la presentacion.
internal sealed class MenuSelection
{
    private readonly int _optionCount;

    public MenuSelection(int optionCount)
    {
        if (optionCount <= 0)
            throw new ArgumentOutOfRangeException(nameof(optionCount), "El menu requiere al menos una opcion.");

        _optionCount = optionCount;
    }

    public int SelectedIndex { get; private set; }

    public void MoveNext() => SelectedIndex = (SelectedIndex + 1) % _optionCount;

    public void MovePrevious() => SelectedIndex = SelectedIndex == 0 ? _optionCount - 1 : SelectedIndex - 1;

    public void Reset() => SelectedIndex = 0;
}
