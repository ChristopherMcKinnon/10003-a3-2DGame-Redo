using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using System.Linq;

namespace MohawkGame2D
{
    internal class Scene
    {
        public Game Game;
        public Player Player;
        public Controls Controls;
        public Vector2[] spawnArea;
        float spawnWidth;
        float gameTimer;
        float gameInterval;

        float enemySpawnTimer;
        float enemySpawnInterval;

        Vector2[][] randomStars;
        float starChangeTimer;
        float starChangeInterval;


        public List<Entity> entities = new List<Entity>();
        public List<Entity> addEntityQueue = new List<Entity>();
        public List<Entity> removeEntityQueue = new List<Entity>();

        //public List<Enemy> liveEnemies = new List<Enemy>();
            

        // Intended to run once at the start
        public void Init(Game setGame)
        {
            this.Game = setGame;
            this.Player = new Player(this);
            Console.WriteLine($"Entities: {entities.GetType}");
            Console.WriteLine($"Add: {addEntityQueue}");
            AddEntity(this.Player);
            Console.WriteLine($"Entities: {entities}");
            this.Controls = new Controls(this);
            this.gameTimer = 10f;
            this.gameInterval = 0f;
            this.spawnWidth = 50;
            this.spawnArea = [new Vector2(0-spawnWidth, 0-spawnWidth), new Vector2(Game.windowSize[0] + spawnWidth, Game.windowSize[1] + spawnWidth)];

            this.enemySpawnTimer = 3f;
            this.enemySpawnInterval = 0f;

            this.starChangeTimer = 1f; // Seconds it takes for stars to change;
            this.MakeStars();
            
        }
        // Runs every frame
        public void Update()
        {
            this.DrawStars();
            this.Controls.Update();

            // Update each entity (do not remove or add any new ones yet)
            foreach (Entity entity in entities)
            {
                entity.Update();
            }
            // Add new entities from entity queue
            foreach (Entity entity in addEntityQueue.ToList())
            {
                entities.Add(entity);
            }
            addEntityQueue.Clear();
            // Remove entities from entity queue
            foreach (Entity entity in removeEntityQueue.ToList())
            {
                entities.Remove(entity);
            }
            removeEntityQueue.Clear();

            SpawnEnemies();

            DisplayUI();
        }
        public void AddEntity(Entity setEntity)
        {
            addEntityQueue.Add(setEntity);
        }
        public void RemoveEntity(Entity setEntity)
        {
            removeEntityQueue.Add(setEntity);
        }


        public void SpawnEnemies()
        {
            enemySpawnInterval -= Time.DeltaTime;

            if (enemySpawnInterval <= 0)
            {
                AddEntity(new Enemy(this));
                enemySpawnInterval = enemySpawnTimer;
            }

            
        }
        public void DisplayUI()
        {
            // Set timer
            gameTimer -= Time.DeltaTime;
            Text.Color = Color.Blue;
            if (gameTimer <= 0)
            {
                Text.Draw($"Timer: OVER!!", new Vector2(0, 0));
            }
            else
            {
                Text.Draw($"Timer: {gameTimer}", new Vector2(0, 0));
            }
        }
        public Vector2 RandomBorderSpawn()
        {
            Vector2 spawnLocation = new Vector2(-spawnWidth, -spawnWidth); // Just in case
            int spawnSide = Random.Integer(0, 4);
            if (spawnSide == 0) // Left
            {
                spawnLocation = new Vector2(Random.Float(Enemy.radius*2, -spawnWidth), Random.Float(0, Game.windowSize[1]));
            }
            if (spawnSide == 1) // Top
            {
                spawnLocation = new Vector2(Random.Float(0, Game.windowSize[0]), Random.Float(Enemy.radius*2, -spawnWidth));
            }
            if (spawnSide == 2) // Right
            {
                spawnLocation = new Vector2(Game.windowSize[0] + Random.Float(Enemy.radius*2, spawnWidth), Random.Float(0, Game.windowSize[1]));
            }
            if (spawnSide == 3) // Down
            {
                spawnLocation = new Vector2(Random.Float(0, Game.windowSize[0]), Game.windowSize[1] + Random.Float(Enemy.radius*2, spawnWidth));
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
            if (position.X > Game.windowSize[0]) // Right side
            {
                position.X = Game.windowSize[0];
                velocity.X *= velocityRetain;
            }
            if (position.Y < 0) // Top side
            {
                position.Y = 0;
                velocity.Y *= velocityRetain;
            }
            if (position.Y > Game.windowSize[1]) // Bottom side
            {
                position.Y = Game.windowSize[1];
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
                randomStars[i][0] = Random.Vector2(new Vector2(0, 0), new Vector2(this.Game.windowSize[0], this.Game.windowSize[1]));
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
