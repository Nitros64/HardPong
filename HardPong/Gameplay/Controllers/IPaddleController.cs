namespace HardPong;

internal interface IPaddleController
{
    // -1 = up, 1 = down, 0 = no key or both keys pressed
    float ReadMovementAxis();
}
