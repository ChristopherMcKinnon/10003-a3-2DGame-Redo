using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace MohawkGame2D
{
    internal class Scene
    {
        public Game game;
        public Player player;
        public Controls controls;
        public Vector2[] spawnArea;
        float spawnWidth;

        Vector2[][] randomStars;
        float starChangeTimer;
        float starChangeInterval;


        public List<Bullet> liveBullets = new List<Bullet>();
        //public List<Enemy> liveEnemies = new List<Enemy>();
            

        // Intended to run once at the start
        public void Init(Game setGame)
        {
            this.game = setGame;
            this.player = new Player(this);
            this.controls = new Controls(this);

            this.spawnArea = [new Vector2(0-spawnWidth, 0-spawnWidth), new Vector2(game.windowSize[0] + spawnWidth, game.windowSize[1] + spawnWidth)];

            this.starChangeTimer = 1f; // Seconds it takes for stars to change;
            this.MakeStars();
            
        }
        // Runs every frame
        public void Update()
        {
            this.DrawStars();
            this.controls.Update();
            this.player.Update();

            // Update each bullet
            for (int i = liveBullets.Count - 1; i >= 0; i--) // Iterate backwards for possibility of removal
            {
                liveBullets[i].Update();
            }
        }
        public Vector2 RandomSpawn()
        {
            Vector2 spawnLocation = new Vector2(-spawnWidth, -spawnWidth); // Just in case
            int spawnSide = Random.Integer(0, 4);
            if (spawnSide == 0) // Left
            {
                spawnLocation = new Vector2(Random.Float(0, -spawnWidth), Random.Float(0, game.windowSize[1]));
            }
            if (spawnSide == 0) // Top
            {
                spawnLocation = new Vector2(Random.Float(0, game.windowSize[0]), Random.Float(0, -spawnWidth));
            }
            if (spawnSide == 0) // Right
            {
                spawnLocation = new Vector2(game.windowSize[0] + Random.Float(game.windowSize[0], spawnWidth), Random.Float(0, game.windowSize[1]));
            }
            if (spawnSide == 0) // Down
            {
                spawnLocation = new Vector2(Random.Float(0, game.windowSize[0]), game.windowSize[1] + Random.Float(0, spawnWidth));
            }
            return spawnLocation;
        }
        public bool CheckOutsideBorders(Vector2 position)
        {
            if (position.X < spawnArea[0].X || position.X > spawnArea[1].X || position.Y < spawnArea[0].Y || position.Y > spawnArea[1].Y)
            {
                return true; // Return true if conditions are met (object OUTSIDE borders)
            }
            return false; // Return false if conditions are not met (object INSIDE borders)
        }
        public Vector2[] BounceOffBorder(Vector2 position, Vector2 velocity)
        {
            float velocityRetain = -0.4f;
            if (position.X < 0) // Left side
            {
                position.X = 0;
                velocity.X *= velocityRetain;
            }
            if (position.X > game.windowSize[0]) // Right side
            {
                position.X = game.windowSize[0];
                velocity.X *= velocityRetain;
            }
            if (position.Y < 0) // Top side
            {
                position.Y = 0;
                velocity.Y *= velocityRetain;
            }
            if (position.Y > game.windowSize[1]) // Bottom side
            {
                position.Y = game.windowSize[1];
                velocity.Y *= velocityRetain;
            }

            return [position, velocity];
        }
        public void MakeStars()
        {
            randomStars = new Vector2[100][];
            // Make stars
            for (int i = 0; i < randomStars.Length; i++)
            {
                randomStars[i] = new Vector2[2];
                randomStars[i][0] = Random.Vector2(new Vector2(0, 0), new Vector2(this.game.windowSize[0], this.game.windowSize[1]));
                randomStars[i][1] = new Vector2(Random.Float(0, this.starChangeTimer*2), 0); // Star scale interval as Vector2.X
            }
        }
        public void DrawStars()
        {
            for (int i = 0; i < randomStars.Length; i++)
            {

                // Decrease star scale interval
                randomStars[i][1].X -= Time.DeltaTime;
                Draw.FillColor = Color.White;
                Draw.LineColor = Color.White;

                if (randomStars[i][1].X <= 0)
                {
                    randomStars[i][1].X = starChangeTimer * 2; // Reset
                }
                if (randomStars[i][1].X <= starChangeTimer)
                {
                    Draw.Circle(randomStars[i][0], 1f);
                }
                if (randomStars[i][1].X <= starChangeTimer*2 && randomStars[i][1].X > starChangeTimer)
                {
                    Draw.Circle(randomStars[i][0], 2f);
                }

                
            }
            
        }

    }
}
