using Raylib_cs;
using System.Numerics;
class game
{
    static void Main()
    {
        Player player = new Player();
        Stone stone = new Stone();
        Post post = new Post();
        Platforms platforms = new Platforms();
        Vector2[] stoneArr = new Vector2[] {new Vector2(320, 30), new Vector2(640, -170),
                                            new Vector2(960, -370), new Vector2(1260, -570),
                                            new Vector2(120, -770), new Vector2(320, -970),
                                            new Vector2(640, -1170), new Vector2(960, -1370),
                                            new Vector2(1260, -570)};
        // Initialzes The Window
        Raylib.InitWindow(1920, 1000, "Window");
        Raylib.SetTargetFPS(120);
        while (!Raylib.WindowShouldClose())
        {
            #region Update
            player.UpdatePlayer(stoneArr);
            post.Update();
            #endregion
            #region Render
            double timer = Raylib.GetTime();
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
            if (timer > 10)
            {
                stone.UpdateRocks(stoneArr);
            }
            #endregion
            Raylib.EndDrawing();
            #endregion
        }
    }
}