using Raylib_cs;
using System.Numerics;
public abstract class GameObjects
{
    public abstract void Update();
    public abstract void Render();
}
public class Player : GameObjects
{
    public Vector2 position = new Vector2(320, 820);
    bool isGrounded = true;
    const int jumpheight = 800;
    int playerSpeed = 4;
    float fallspeed = Raylib.GetFrameTime() * 10;
    float velocity_Y = 0;
    Enemy enemy = new Enemy();
    Post post = new Post();
    Stone stone = new Stone();
    Platforms platform = new Platforms();
    FlyingEnemy flyingEnemy = new FlyingEnemy();

    public override void Update()
    {
    }
    public bool UpdatePlayer(Vector2[] stoneArr, bool isRunning)
    {
        double timer = Raylib.GetTime();
        Rectangle player = new Rectangle(position.X, position.Y, 60, 60);
        Rectangle pole = new Rectangle(915, 650, 100, 230);
        Rectangle pL = new Rectangle(300, 650, 300, 20);
        Rectangle pR = new Rectangle(1230, 650, 300, 20);
        var delta = Raylib.GetFrameTime() * 100;
        
            enemy.Follow(ref position, pole);
            enemy.Render();
            isRunning = enemy.EnemyCollision(player, isRunning);
        
                
        if (timer > 45)
        {
            flyingEnemy.Render();
            flyingEnemy.Follow(position, pole, pL, pR);
            isRunning = flyingEnemy.Collision(player, isRunning);
        }
        #region CheckCollision
        isRunning = stone.Collision(player, ref stoneArr, isRunning);
        string side = post.CheckCollision(ref position, playerSpeed);
        switch (side)
        {
            case "top":
                isGrounded = true;
                velocity_Y = 0;
                position.Y = pole.Y - player.Height;
                if (Raylib.IsKeyPressed(KeyboardKey.Space)) isGrounded = false;
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
                if (position.Y < 820) isGrounded = false;
                else if (position.Y > 820) isGrounded = true;
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
        if (Raylib.IsKeyDown(KeyboardKey.D)) position.X += delta * playerSpeed; // moves to the right
        if (Raylib.IsKeyDown(KeyboardKey.A)) position.X -= delta * playerSpeed; // moves to the left 
        if (Raylib.IsKeyDown(KeyboardKey.Space) && isGrounded == true) // Jumps 
        {
            velocity_Y = jumpheight * Raylib.GetFrameTime();
            position.Y -= velocity_Y;
            isGrounded = false;
        }
        if (!isGrounded) // pulls player back down
        {
            velocity_Y -= fallspeed;
            position.Y -= velocity_Y;
        }
        if (position.Y >= 820) // Puts player on the ground
        {
            isGrounded = true;
            position.Y = 820;
            velocity_Y = 0;
            fallspeed = Raylib.GetFrameTime() * 10;
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
        return isRunning;
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
    public override void Update() { }
    public void Follow(ref Vector2 p, Rectangle post)
    {
        if (Raylib.GetTime() > 40) enemySpeed = 1;

        if (p.X > position.X) position.X += enemySpeed;
        else if (p.X < position.X) position.X -= enemySpeed;

        if (p.X > position2.X) position2.X += enemySpeed;
        else if (p.X < position2.X) position2.X -= enemySpeed;

        Rectangle e = new Rectangle(position.X, position.Y, 20, 20);
        Rectangle en = new Rectangle(position2.X, position2.Y, 20, 20);

        if (Raylib.CheckCollisionCircleRec(position, 20, post)) position.X = post.X - e.Width;
        if (Raylib.CheckCollisionCircleRec(position2, 20, post)) position2.X = post.X + en.Width + post.Width;  
    }
    public override void Render()
    {
        // Draws Both Enemies
        Raylib.DrawCircle((int)position.X, (int)position.Y, 20, Color.Brown);
        Raylib.DrawCircle((int)position2.X, (int)position2.Y, 20, Color.Brown);
    }
    public bool EnemyCollision(Rectangle p, bool isRunning)
    {
        if (Raylib.CheckCollisionCircleRec(position, 20, p)) isRunning = false;
        if (Raylib.CheckCollisionCircleRec(position2, 20, p)) isRunning = false;

        return isRunning;
    }
} // working
public class Stone : GameObjects
{   
    public override void Render()
    {
    }
    public override void Update()
    {       
    }
    internal void RenderRocks(ref Vector2[] stoneArr)
    {
        double timer = Raylib.GetTime();
        if (timer > 40) return;
        for (int i = 0; i < stoneArr.Length; i++)
        {
            if (timer > i + 10)
            {
                Raylib.DrawCircle((int)stoneArr[i].X, (int)stoneArr[i].Y, 30, Color.Gray);
            }
        }
    }
    internal void UpdateRocks(Vector2[] stoneArr)
    {
        for (int i = 0; i < stoneArr.Length; i++)
        {
            if (stoneArr[i].Y < 1030)
            {
                stoneArr[i].Y += 4;
            }
            else
            {
                int random = Random.Shared.Next(0, 1830);
                stoneArr[i].X = random;
                stoneArr[i].Y = -40;
            }
        }
    }

    internal Vector2[] SetBackRocks(Vector2[] stoneArr)
    {
        double timer = Raylib.GetTime();
        for (int i = 0; i < stoneArr.Length; i++)
        {
            if(timer < i+10) stoneArr[i].Y = -40;
        }
        return stoneArr;
    }
    internal bool Collision(Rectangle player, ref Vector2[] stoneArr, bool isRunning)
    {
        double timer = Raylib.GetTime();
        if (timer > 40) return isRunning;
        for (int i = 0; i < stoneArr.Length; i++)
        {
            if (timer > i + 10)
            {
                if (Raylib.CheckCollisionCircleRec(stoneArr[i], 30, player)) isRunning = false;
            }
        }
        return isRunning;
    }
} // not working (the collision box of the stones is in the top left corner)
public class Post : GameObjects
{
    Rectangle post = new Rectangle(915, 650, 100, 230);
    
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
    int enemySpeed;
    public override void Update() { }
    public override void Render()
    {
        if (Raylib.GetTime() > 60)
        {
            Raylib.DrawCircle((int)position.X, (int)position.Y, 65, Color.Red);
            enemySpeed = 2;
        }
        else
        {
            Raylib.DrawCircle((int)position.X, (int)position.Y, 40, Color.DarkGreen);
            enemySpeed = 2;
        }
    }
    public void Follow(Vector2 player, Rectangle pole, Rectangle pL, Rectangle pR)
    {
        if (position.X > player.X)
        {
            position.X -= enemySpeed;
        }
        else if (position.X < player.X)
        {
            position.X += enemySpeed;
        }

        if (position.Y < player.Y)
        {
            position.Y += enemySpeed;
        }
        else if (position.Y > player.Y)
        {
            position.Y -= enemySpeed;
        }
    }
    public bool Collision(Rectangle player, bool isRunning)
    {
        if (Raylib.GetTime() < 60)
        {
            if (Raylib.CheckCollisionCircleRec(position, 40, player))
            {
                isRunning = false;
            }
        }
        else
        {
            if (Raylib.CheckCollisionCircleRec(position, 65, player))
            {
                isRunning = false;
            }
        }
        return isRunning;
    }
}