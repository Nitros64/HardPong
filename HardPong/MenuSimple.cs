using System;
using HardPong.SpriteClass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HardPong;

internal class MenuSimple
{
    private const Keys Up = Keys.Up;
    private const Keys Down = Keys.Down;
    private const Keys Start = Keys.Enter;
    private const Keys Select = Keys.Space;
    private const Keys Escape = Keys.Escape;

    private SelectArrow _selectArrow;
    private readonly string[] _options;        
    private int _menuOption;
    private readonly Vector2 _startlocation;// la localizacion del menu, incluyendo la flecha seleccionadora
    private Vector2 _optionsLocation;//Location of the tittles

    //KeyBoard Input Managers
    private SpriteFont _nesFont2;//Fuentes

    internal KeyInputManager InputManager { get; set; }

    private MenuSimple(Vector2 newLocation) {
        InputManager = new KeyInputManager();
        _startlocation = newLocation;
        _optionsLocation = _startlocation;
        _optionsLocation.X += 20;
    }

    public MenuSimple(Vector2 newLocation,params string[] ops)
        : this(newLocation)
    {
        if (ops is null || ops.Length == 0)
            throw new ArgumentException("MenuSimple requiere al menos una opcion.", nameof(ops));

        _options = ops;
    }

    public void Initialize()
    {
        _selectArrow.SetPosition(_startlocation.X, _startlocation.Y);
        _menuOption = 0;
    }
    public void LoadContent(SpriteFont sf, Texture2D arrowSprite, SoundEffect arrowSound) 
    {
        _nesFont2 = sf;
        _selectArrow = new SelectArrow(arrowSprite, _startlocation, arrowSound);
    }

    private int InputHandler() {
        InputManager.Begin();
        //Chequea si la posicion de la flecha ha cambiado
        if (InputManager.CheckPressedKey(Up)) // Flecha Para arriba
        {
            _menuOption = (--_menuOption < 0) ? _options.Length - 1 : _menuOption;
            _selectArrow.SetPosition(_startlocation.X, _startlocation.Y + 30 * _menuOption);
            _selectArrow.PlaySelectionSound();
        }
        else if (InputManager.CheckPressedKey(Down) ||
                 InputManager.CheckPressedKey(Select)) // Flecha Para Abajo
        {
            _menuOption = (_menuOption + 1) % _options.Length;
            _selectArrow.SetPosition(_startlocation.X, _startlocation.Y + 30 * _menuOption);
            _selectArrow.PlaySelectionSound();
        }
        //Chequea si se ha presionado la tecla Escape o Enter
        if (InputManager.CheckPressedKey(Start))
        {
            InputManager.End();
            return _menuOption + 1;
        }

        if (InputManager.CheckPressedKey(Escape))
        {
            InputManager.End();
            return 0;
        }

        InputManager.End();
        return -1;
    }

    public bool OPEN_KEY(Keys open)
    {
        InputManager.Begin();
        bool op = InputManager.CheckPressedKey(open);
        InputManager.End();
        return op;
    }

    public void Exit()
    {
        InputManager.Exit();
    }

    public int Update() {            
        return InputHandler();
    }
    public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
        _selectArrow.Draw(spriteBatch);// flecha de seleccion
        _optionsLocation.Y = _startlocation.Y;
        foreach (string vin in _options) {
            spriteBatch.DrawString(_nesFont2, vin, _optionsLocation, Color.White);
            _optionsLocation.Y += 30;
        }
    }
}