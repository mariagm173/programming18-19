﻿class Sprite
{
    protected int x, y;
    protected int startX, startY;
    protected int width, height;
    protected int xSpeed, ySpeed;
    protected bool visible;
    protected Image image;
    protected Image[][] sequence;
    protected bool containsSequence;
    protected int currentFrame;

    protected byte numDirections = 6;
    protected byte currentDirection;
    public const byte RIGHT = 0;
    public const byte LEFT = 1;
    public const byte DOWN = 2;
    public const byte UP = 3;
    public const byte APPEARING = 4;
    public const byte DISAPPEARING = 5;

    public Sprite()
    {
        startX = -1;
        startY = -1;
        width = 120;
        height = 95;
        visible = true;
        sequence = new Image[numDirections][];
        currentDirection = RIGHT;
    }

    public Sprite(string imageName)
        : this()
    {
        LoadImage(imageName);
    }

    public Sprite(string[] imageNames)
        : this()
    {
        LoadSequence(imageNames);
    }

    public void LoadImage(string name)
    {
        image = new Image(name);
        containsSequence = false;
    }

    public void LoadSequence(byte direction, string[] names)
    {
        int amountOfFrames = names.Length;
        sequence[direction] = new Image[amountOfFrames];
        for (int i = 0; i < amountOfFrames; i++)
            sequence[direction][i] = new Image(names[i]);
        containsSequence = true;
        currentFrame = 0;
    }

    public void LoadSequence(string[] names)
    {
        LoadSequence(RIGHT, names);
    }

    public int GetX()
    {
        return x;
    }

    public int GetY()
    {
        return y;
    }

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }

    public int GetSpeedX()
    {
        return xSpeed;
    }

    public int GetSpeedY()
    {
        return ySpeed;
    }

    public bool IsVisible()
    {
        return visible;
    }

    public void MoveTo(int newX, int newY)
    {
        x = newX;
        y = newY;
        if (startX == -1)
        {
            startX = x;
            startY = y;
        }
    }

    public void SetSpeed(int newXSpeed, int newYSpeed)
    {
        xSpeed = newXSpeed;
        ySpeed = newYSpeed;
    }

    public void Show()
    {
        visible = true;
    }

    public void Hide()
    {
        visible = false;
    }

    public virtual void Move()
    {
        // To be redefined in subclasses
    }

    public void DrawOnHiddenScreen()
    {
        if (!visible)
            return;

        if (containsSequence)
            SdlHardware.DrawHiddenImage(
                sequence[currentDirection][currentFrame], x, y);
        else
            SdlHardware.DrawHiddenImage(image, x, y);
    }

    public void NextFrame()
    {
        currentFrame++;
        if (currentFrame >= sequence[currentDirection].Length)
            currentFrame = 0;
    }

    public void ChangeDirection(byte newDirection)
    {
        if (!containsSequence) return;
        if (currentDirection != newDirection)
        {
            currentDirection = newDirection;
            currentFrame = 0;
        }
    }

    public bool CollisionsWith(Sprite otherSprite)
    {
        return (visible && otherSprite.IsVisible() &&
            CollisionsWith(otherSprite.GetX(),
                otherSprite.GetY(),
                otherSprite.GetX() + otherSprite.GetWidth(),
                otherSprite.GetY() + otherSprite.GetHeight()));
    }

    public bool CollisionsWith(int xStart, int yStart, int xEnd, int yEnd)
    {
        if (visible &&
                (x < xEnd) &&
                (x + width > xStart) &&
                (y < yEnd) &&
                (y + height > yStart)
                )
            return true;
        return false;
    }

    public void Restart()
    {
        x = startX;
        y = startY;
    }
}
