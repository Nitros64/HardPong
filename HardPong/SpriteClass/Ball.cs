using Microsoft.Xna.Framework;

namespace HardPong.SpriteClass;

// Entidad fisica de la pelota: posicion, velocidad y colision.
// No conoce texturas, sonido ni reglas de puntuacion.
internal class Ball
{
    public const float BallSpeedX = 5;
    public const float BallSpeedY = 4;
    public const float MinVerticalSpeed = 3f;
    public const int BallWidth = 12;
    public const int BallHeight = 12;

    private readonly Vector2 _spawn;
    private Vector2 _position;
    private Vector2 _speed;

    public Ball(Rectangle court)
    {
        _spawn = new Vector2(court.Width / 2 - BallWidth / 2, court.Height / 2 - BallHeight / 2);
        ResetPosition();
    }

    public Vector2 Position { get => _position; set => _position = value; }
    public Vector2 Direction => _speed;
    public Rectangle CollisionRect => new((int)_position.X, (int)_position.Y, BallWidth, BallHeight);

    public void Update() => _position += _speed;

    public void SetPosition(float x, float y)
    {
        _position.X = x;
        _position.Y = y;
    }

    // Vuelve al saque. Las velocidades conservan el signo con el que se viajaba
    // (asino lo hacia el antiguo SetSpeed del reset).
    public void ResetPosition()
    {
        SetPosition(_spawn.X, _spawn.Y);
        SetSpeed(BallSpeedX, BallSpeedY);
    }

    public void ChangeDirection()
    {
        InvertDirectionHorizontal();
        InvertDirectionVertical();
    }

    public void InvertDirectionVertical() => _speed.Y *= -1;

    public void InvertDirectionHorizontal() => _speed.X *= -1;

    // Normaliza la direccion entrante: magnitud horizontal minima, techo
    // vertical y respeto del signo con el que se viaja.
    public void SetDirection(Vector2 value)
    {
        if (_speed.X < 0)
        {
            if (value.X < BallSpeedX)
                _speed.X = -BallSpeedX;
            else
                _speed.X = -value.X;
        }
        else if (value.X < BallSpeedX)
            _speed.X = BallSpeedX;
        else
            _speed.X = value.X;

        if (value.Y > 7)
            value.Y = 7;

        if (value.Y == 0 || value.Y < 1)
            value.Y = MinVerticalSpeed;

        if (_speed.Y < 0)
        {
            switch (value.Y)
            {
                case < 0:
                    _speed.Y = value.Y;
                    break;
                case > 0:
                    value.Y *= -1;
                    _speed.Y = value.Y;
                    break;
            }
        }
        else
            _speed.Y = value.Y;
    }

    public void SetSpeed(float x, float y)
    {
        if (_speed.X < 0)
        {
            _speed.X = x < BallSpeedX ? BallSpeedX : x;
            _speed.X *= -1;
        }
        else
        {
            _speed.X = x < BallSpeedX ? BallSpeedX : x;
        }

        if (y > 7)
            y = 7;

        if (y is 0 or < 1)
            y = MinVerticalSpeed;

        if (_speed.Y < 0)
        {
            switch (y)
            {
                case < 0:
                    _speed.Y = y;
                    break;
                case > 0:
                    y *= -1;
                    _speed.Y = y;
                    break;
            }
        }
        else
            _speed.Y = y;
    }
}
