// Include the namespaces (code libraries) you need below.
using System;
using System.Numerics;

// The namespace your code is in.
namespace MohawkGame2D
{
    /// <summary>
    ///     Your game code goes inside this class!
    /// </summary>
    public class Game
    {
        // Initialize your variables here:
        public int[] windowSize;
        public Vector2 windowCentre;
        string windowTitle;
        Scene scene;

        /// <summary>
        ///     Setup runs once before the game loop begins.
        /// </summary>
        public void Setup()
        {
            // Set variables
            windowSize = [800, 600];
            windowCentre = new Vector2(windowSize[0] / 2, windowSize[1] / 2);
            windowTitle = "2D Game by Christopher Lawrick-McKinnon";

            scene = new Scene();

            // Window setup
            Window.SetSize(windowSize[0], windowSize[1]);
            Window.SetTitle(windowTitle);
            Window.TargetFPS = 60;


            scene.Init(this);
        }

        /// <summary>
        ///     Update runs every frame.
        /// </summary>
        public void Update()
        {
            Window.ClearBackground(Color.Black);
            scene.Update();
        }
    }

}
