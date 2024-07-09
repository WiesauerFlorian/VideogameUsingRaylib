using Raylib_cs;
using System.Numerics;
public abstract class GameObjects
{
    public abstract void Update();
    public abstract void Render();
}
public class Player : GameObjects
{
    double timer = Raylib.GetTime();
    public Vector2 position = new Vector2(320, 820);
    bool isGrounded = true;
    const int jumpheight = 800;
    int playerSpeed = 4;
    float velocity_Y = 0;
    Enemy enemy = new Enemy();
    Post post = new Post();
    Stone stone = new Stone();
    Platforms platform = new Platforms();
    FlyingEnemy flyingEnemy = new FlyingEnemy();
    public override void Update()
    {
        Rectangle player = new Rectangle(position.X, position.Y, 60, 60);
        Rectangle pole = new Rectangle(915, 650, 100, 230);
        Rectangle pL = new Rectangle(300, 650, 300, 20);
        Rectangle pR = new Rectangle(1230, 650, 300, 20);
        var delta = Raylib.GetFrameTime() * 100;

        enemy.Follow(ref position, pole);
        enemy.Render();
        if (timer > 60)
        {
            flyingEnemy.Render();
            flyingEnemy.Follow(position);
        }
        #region CheckCollision
        enemy.EnemyColision(player);
        stone.Collision(player);
        string side = post.CheckCollision(ref position, playerSpeed);
        switch (side)
        {
            case "top":
                isGrounded = true;
                velocity_Y = 0;
                position.Y = pole.Y - player.Height;
                if (Raylib.IsKeyPressed(KeyboardKey.Space))
                {
                    isGrounded = false;
                }
                break;
            case "right":
                position.X = pole.X + pole.Width;
                break;
            case "left":
                position.X = pole.X - player.Width;
                break;
            case "bottom":
                velocity_Y = 0;
                position.Y = pole.Y + pole.Height;
                break;

            case " ":
                if (position.Y < 820)
                {
                    isGrounded = false;
                }
                else if (position.Y > 820)
                {
                    isGrounded = true;
                }
                break;


        }
        side = platform.CheckCollision(ref position, playerSpeed);
        switch (side)
        {
            case "topL":
                isGrounded = true;
                velocity_Y = 0;
                position.Y = pL.Y - player.Height;
                if (Raylib.IsKeyPressed(KeyboardKey.Space)) isGrounded = false;
                break;
            case "leftL":
                position.X = pL.X - player.Width;
                break;
            case "rightL":
                position.X = pL.X + pL.Width;
                break;
            case "bottomL":
                velocity_Y = 0;
                position.Y = pL.Y + pL.Height;
                break;
            case "topR":
                isGrounded = true;
                velocity_Y = 0;
                position.Y = pR.Y - player.Height;
                if (Raylib.IsKeyPressed(KeyboardKey.Space)) isGrounded = false;
                break;
            case "leftR":
                position.X = pR.X - player.Width;
                break;
            case "rightR":
                position.X = pR.X + pR.Width;
                break;
            case "bottomR":
                velocity_Y = 0;
                position.Y = pR.Y + pR.Height;
                break;
        }
        #endregion
        #region Movement
        if (Raylib.IsKeyDown(KeyboardKey.D)) // moves to the right
        {
            position.X += delta * playerSpeed;
        }
        if (Raylib.IsKeyDown(KeyboardKey.A)) // moves to the left
        {
            position.X -= delta * playerSpeed;
        }
        if (Raylib.IsKeyDown(KeyboardKey.Space) && isGrounded == true) // Jumps 
        {
            velocity_Y = jumpheight * Raylib.GetFrameTime();
            position.Y -= velocity_Y;
            isGrounded = false;
        }
        if (!isGrounded) // pulls player back down
        {
            velocity_Y -= Raylib.GetFrameTime() * 10;
            position.Y -= velocity_Y;
        }
        if (position.Y >= 820) // Puts player on the ground
        {
            isGrounded = true;
            position.Y = 820;
            velocity_Y = 0;
        }
        #endregion
        #region Map (boarder of the window) 
        switch (position.Y)
        {
            case > 820: position.Y = 820; break;
            case < 0: position.Y = 0; break;
        }
        switch (position.X)
        {
            case < 0: position.X = 0; break;
            case > 1830: position.X = 1830; break;
        }
        #endregion
    }
    public override void Render()
    {
        // Draws Player
        Raylib.DrawRectangle((int)position.X, (int)position.Y, 60, 60, Color.Red);
    }
} // working
public class Enemy : GameObjects
{
    int enemySpeed = 2;
    Vector2 position = new Vector2(800, 860);
    Vector2 position2 = new Vector2(1300, 860);
    public override void Update()
    { }
    public void Follow(ref Vector2 p, Rectangle post)
    {
        if (p.X > position.X)
            position.X += enemySpeed;
        else if (p.X < position.X)
            position.X -= enemySpeed;

        if (p.X > position2.X)
            position2.X += enemySpeed;
        else if (p.X < position2.X)
            position2.X -= enemySpeed;

        Rectangle e = new Rectangle(position.X, position.Y, 20, 20);
        Rectangle en = new Rectangle(position2.X, position2.Y, 20, 20);
        if (Raylib.CheckCollisionCircleRec(position, 20, post))
        {
            position.X = post.X - e.Width;
        }
        if (Raylib.CheckCollisionCircleRec(position2, 20, post))
        {
            position2.X = post.X + en.Width + post.Width;
        }
    }
    public override void Render()
    {
        // Draws Both Enemies
        Raylib.DrawCircle((int)position.X, (int)position.Y, 20, Color.Brown);
        Raylib.DrawCircle((int)position2.X, (int)position2.Y, 20, Color.Brown);
    }
    public void EnemyColision(Rectangle p)
    {
        if (Raylib.CheckCollisionCircleRec(position, 20, p)) Raylib.CloseWindow();
        if (Raylib.CheckCollisionCircleRec(position2, 20, p)) Raylib.CloseWindow();
    }
} // working
public class Stone : GameObjects
{
    Vector2[] stoneArr = new Vector2[5]; // Array For The 5 Stones

    public void FillArr()
    {
        stoneArr[0] = new Vector2(320, 20);
        stoneArr[1] = new Vector2(640, 20);
        stoneArr[2] = new Vector2(960, 20);
        stoneArr[3] = new Vector2(1260, 20);
        stoneArr[4] = new Vector2(120, 20);
    }
    public override void Render()
    {
        double timer = Raylib.GetTime();
        if (timer > 10)
        {
            Raylib.DrawCircle((int)stoneArr[0].X, (int)stoneArr[0].Y, 20, Color.Brown);
        }
        if (timer > 20)
        {
            Raylib.DrawCircle((int)stoneArr[1].X, (int)stoneArr[1].Y, 20, Color.Brown);
        }
        if (timer > 30)
        {
            Raylib.DrawCircle((int)stoneArr[2].X, (int)stoneArr[2].Y, 20, Color.Brown);
        }
        if (timer > 40)
        {
            Raylib.DrawCircle((int)stoneArr[3].X, (int)stoneArr[3].Y, 20, Color.Brown);
        }
        if (timer > 50)
        {
            Raylib.DrawCircle((int)stoneArr[4].X, (int)stoneArr[4].Y, 20, Color.Brown);
        }
    }
    public override void Update()
    {
        for (int i = 0; i < stoneArr.Length; i++)
        {
            if (stoneArr[i].Y < 820)
            {
                stoneArr[i].Y += 4;
                Render();
            }
            else
            {
                int random = Random.Shared.Next(0, 1830);
                stoneArr[i].X = random;
                stoneArr[i].Y = 20;
            }

        }
    }
    internal void Collision(Rectangle player)
    {
        if (Raylib.CheckCollisionCircleRec(stoneArr[1], 20, player))
        {
            Console.WriteLine("Collision");
        }
        if (Raylib.CheckCollisionCircleRec(stoneArr[2], 20, player))
        {
            Console.WriteLine("Collision");
        }
        if (Raylib.CheckCollisionCircleRec(stoneArr[3], 20, player))
        {
            Console.WriteLine("Collilsion");
        }
        if (Raylib.CheckCollisionCircleRec(stoneArr[4], 20, player))
        {
            Console.WriteLine("Collision");
        }
        if (Raylib.CheckCollisionCircleRec(stoneArr[0], 20, player))
        {
            Console.WriteLine("Collision");
        }

    }
} // not working (the collision box of the stones is in the top left corner)
public class Post : GameObjects
{
    Rectangle post = new Rectangle(915, 650, 100, 230);
    Enemy enemy = new Enemy();
    public override void Render()
    {
        Raylib.DrawRectangle((int)post.X, (int)post.Y, (int)post.Width, (int)post.Height, Color.Black);
    }
    public override void Update()
    {

    }
    public string CheckCollision(ref Vector2 position, int playerSpeed)
    {
        Rectangle player = new Rectangle((int)position.X, (int)position.Y, 60, 60);
        if (Raylib.CheckCollisionRecs(player, post))
        {
            // right
            if (position.X >= post.X + post.Width - playerSpeed)
            {
                return "right";
            }
            // left
            else if (position.X + player.Width <= post.X + playerSpeed)
            {
                return "left";
            }
            //bottom
            else if (position.Y < post.Y + post.Height && position.Y > post.Y)
            {
                return "bottom";
            }
            // Top
            else if (position.Y + player.Height - playerSpeed <= post.Y + playerSpeed)
            {
                return "top";
            }
        }
        return " ";
    }
} // working
public class Platforms : GameObjects
{
    Rectangle pL = new Rectangle(300, 650, 300, 20);
    Rectangle pR = new Rectangle(1230, 650, 300, 20);
    public override void Render()
    {
        Raylib.DrawRectangle((int)pL.X, (int)pL.Y, (int)pL.Width, (int)pL.Height, Color.Black);
        Raylib.DrawRectangle((int)pR.X, (int)pR.Y, (int)pR.Width, (int)pR.Height, Color.Black);
    }
    public override void Update()
    {
    }
    public string CheckCollision(ref Vector2 position, int playerSpeed)
    {
        #region left platform
        Rectangle p = new Rectangle((int)position.X, (int)position.Y, 60, 60);
        if (Raylib.CheckCollisionRecs(p, pL))
        {
            // right
            if (position.X >= pL.X + pL.Width - playerSpeed)
            {
                return "rightL";
            }
            // left
            else if (position.X + p.Width <= pL.X + playerSpeed)
            {
                return "leftL";
            }
            //bottom
            else if (position.Y < pL.Y + pL.Height && position.Y > pL.Y)
            {
                return "bottomL";
            }
            // Top
            if (position.Y + p.Height - playerSpeed <= pL.Y + playerSpeed)
            {
                return "topL";
            }
        }
        #endregion
        #region right platform
        if (Raylib.CheckCollisionRecs(p, pR))
        {
            // right
            if (position.X >= pR.X + pR.Width - playerSpeed)
            {
                return "rightR";
            }
            // left
            else if (position.X + p.Width <= pR.X + playerSpeed)
            {
                return "leftR";
            }
            //bottom
            else if (position.Y < pR.Y + pR.Height && position.Y > pR.Y)
            {
                return "bottomR";
            }
            // Top
            if (position.Y + p.Height - playerSpeed <= pR.Y + playerSpeed)
            {
                return "topR";
            }
        }
        #endregion
        return "no Collision";
    }
} // working
public class FlyingEnemy : GameObjects
{
    Vector2 position = new Vector2(900, 100);
    public override void Update()
    {

    }
    public override void Render()
    {
        Raylib.DrawCircle((int)position.X, (int)position.Y, 30, Color.DarkGreen);
    }
    public void Follow(Vector2 player)
    {
        if (position.X > player.X)
        {
            position.X -= 2;
        }
        else if (position.X < player.X)
        {
            position.X += 2;
        }

        if (position.Y < player.Y)
        {
            position.Y += 2;
        }
        else if (position.Y > player.Y)
        {
            position.Y -= 2;
        }

    }
}