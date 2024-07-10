using Raylib_cs;
class game
{
    static void Main()
    {
        Player player = new Player();
        Stone stone = new Stone();
        Post post = new Post();

        Platforms platforms = new Platforms();
        // Initialzes The Window
        Raylib.InitWindow(1920, 1000, "Window");
        Raylib.SetTargetFPS(120);
        stone.FillArr();
        while (!Raylib.WindowShouldClose())
        {
            #region Update
            player.Update();
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
            Raylib.EndDrawing();
            #endregion
            #region stone
            if (timer > 10)
            {
                stone.Update();
            }
            #endregion
        }
    }
}