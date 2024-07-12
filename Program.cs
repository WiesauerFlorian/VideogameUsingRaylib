using Raylib_cs;
using System.Numerics;
class game
{
    static void Main()
    {
        bool isRunning = true;
        Player player = new Player();
        Stone stone = new Stone();
        Post post = new Post();
        Platforms platforms = new Platforms();
        Vector2[] stoneArr = new Vector2[] {new Vector2(320, 30), new Vector2(640, -170),
                                            new Vector2(960, -370), new Vector2(1260, -570),
                                            new Vector2(120, -770), new Vector2(320, -970),
                                            new Vector2(640, -1170), new Vector2(960, -1370),
                                            new Vector2(1260, -1570), new Vector2(120, -1770),
                                            new Vector2(320, -1970), new Vector2(640, -2170), 
                                            new Vector2(960, -2370), new Vector2(1260, -2570),
                                            new Vector2(120, -2770), new Vector2(320, -2970),
                                            new Vector2(640, -3170), new Vector2(960, -3370)};
        double timer = Raylib.GetTime();
        // Initialzes The Window
        Raylib.InitWindow(1920, 1000, "EPIC GAME OF DOOM AND DEATH!!!💀☠️💀");
        Raylib.SetTargetFPS(120);
        while (!Raylib.WindowShouldClose())
        {
            if (isRunning && timer < 90)
            {
                #region Update
                timer = Raylib.GetTime();
                isRunning = player.UpdatePlayer(stoneArr, isRunning);
                post.Update();
                #endregion
                #region Render
                // Draws Background and Game Objects
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.SkyBlue);
                Raylib.DrawText($"Timesurvived: {timer:0} sec", 320, 10, 20, Color.White);
                //Background
                Raylib.DrawCircle(30, 20, 100, Color.Yellow); // Sun
                Raylib.DrawRectangle(00, 880, 1920, 150, Color.Green); // Floor
                                                                       // Draws Gameobjects
                player.Render();
                post.Render();
                platforms.Render();
                // Stop drawing
                #region stone
                if (timer > 10) stone.UpdateRocks(stoneArr);
                stone.SetBackRocks(stoneArr);
                stone.RenderRocks(ref stoneArr);
                #endregion
                Raylib.EndDrawing();
                #endregion
            }
            else if (!isRunning)
            {
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.Black);
                Raylib.DrawText("YOU DIED", 500, 400, 200, Color.Red);
                Raylib.DrawText($"You survived for {timer:0} second(s)", 650, 700, 30, Color.White);
                Raylib.DrawText($"Press 'ESC' to exit ", 100, 900, 30, Color.White);
                Raylib.EndDrawing();
            }
            else
            {
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.Black);
                Raylib.DrawText("YOU WIN", 500, 400, 200, Color.Green);
                Raylib.DrawText($"You survived the entire {timer:0} second(s)!", 625, 700, 30, Color.White);
                Raylib.DrawText($"Press 'ESC' to exit ", 100, 900, 30, Color.White);
                Raylib.EndDrawing();
            }
        }
    }
}