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
        Font font = Text.LoadFont("..\\..\\..\\..\\..\\10003-a3-2DGame-Redo\\lawrick-mckinnon-christopher-a3-2dgame-redo\\Assets\\m42.TTF");

        public Vector2[] spawnArea;
        float spawnWidth;

        public bool gameOver;
        float gameTimer;
        float gameInterval;
        float peaceTimer;

        float enemySpawnTimer;
        float enemySpawnInterval;

        public float score;

        float starChangeTimer;
        float starChangeInterval;


        public List<Entity> entities = new List<Entity>();
        public List<Entity> addEntityQueue = new List<Entity>();
        public List<Entity> removeEntityQueue = new List<Entity>();
        public Star[] stars = new Star[100];

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
            this.gameOver = false;
            this.gameTimer = 60f;
            this.gameInterval = gameTimer;
            this.peaceTimer = 3f;
            this.spawnWidth = 50;
            this.spawnArea = [new Vector2(0-spawnWidth, 0-spawnWidth), new Vector2(Game.windowSize[0] + spawnWidth, Game.windowSize[1] + spawnWidth)];

            this.enemySpawnTimer = 1f;
            this.enemySpawnInterval = 0f;

            this.score = 0;

            this.MakeStars();
            
        }
        // Runs every frame
        public void Update()
        {
            
            this.Controls.Update();
            // Update each entity (do not remove or add any new ones yet)
            foreach (Entity entity in entities)
            {
                entity.Update();
            }
            // Only add when game is on
            if (!gameOver)
            {
                
                // Add new entities from entity queue
                foreach (Entity entity in addEntityQueue.ToList())
                {
                    entities.Add(entity);
                }
                addEntityQueue.Clear();

                SpawnEnemies();
                DrawHealthUI();
                GameTimeUpdate();
            }
            
            // Remove entities from entity queue
            foreach (Entity entity in removeEntityQueue.ToList())
            {
                entities.Remove(entity);
            }
            removeEntityQueue.Clear();

            // Process Game End
            if (gameOver)
            {
                GameEnd();
            }
            
        }
        public void AddEntity(Entity setEntity)
        {
            addEntityQueue.Add(setEntity);
        }
        public void RemoveEntity(Entity setEntity)
        {
            removeEntityQueue.Add(setEntity);
        }

        public void GameEnd() // Should always be called within the update loop
        {
            if (!gameOver) // Only runs once at the end of the game
            {
                gameOver = true; // Permanently set to true past this point
                CalculateScore();
            }
            
            // Add all entities to the remove queue (this happens every frame after gameOver set to true in this case (considering Update();)
            foreach(Entity entity in entities)
            {
                if (entity is not Star)
                {
                    RemoveEntity(entity);
                }
            }
            Text.Color = Color.Blue;
            string gameOverText = "GAME OVER!";
            Text.Size = 70;
            DrawCentredText(gameOverText, 15, 0);
            Text.Size = 25;
            DrawCentredText($"Your score: {this.score}", 0, 50);
        }
        public void SpawnEnemies()
        {
            peaceTimer -= Time.DeltaTime;
            if (peaceTimer <= 0)
            {
                enemySpawnInterval -= Time.DeltaTime;
                if (enemySpawnInterval <= 0)
                {
                    AddEntity(new Enemy(this));
                    ChangeDifficulty();
                }
                
            }
            

            

            
        }
        public void DrawCentredText(string setText, float xOffset, float yOffset)
        {
            float fontOffset = 20f;
            Text.Draw(setText, new Vector2(Game.windowSize[0]/2 - setText.Length * (Text.Size/2) + xOffset + fontOffset, Game.windowSize[1]/2 - (Text.Size/2) + yOffset - fontOffset), font); // Repurpose this to centre text on screen
        }
        public void GameTimeUpdate()
        {
            // Set timer
            gameInterval -= Time.DeltaTime;
            if (gameInterval <= 0)
            {
                // End Timer (timer disappears)

                gameOver = true;
            }
            else
            {
                // Continue Timer
                DrawTimerUI();
            }
        }
        public float CalculateScore()
        {
            score += Player.health; // Enemies already add to score when they die
            return score;
        }
        public void DrawTimerUI()
        {
            // Draw Timer
            Text.Color = Color.Blue;
            Text.Size = 25;
            
            string timerString = $"Time: {Math.Round(gameInterval)}";
            Text.Draw(timerString, new Vector2(0, 0), font);
            
        }
        public void DrawHealthUI()
        {
            Text.Color = Color.Green;
            DrawCentredText($"Health: {Player.health}", 0, 313);
        }
        public void ChangeDifficulty()
        {
            enemySpawnInterval = enemySpawnTimer + (gameInterval/gameTimer)*2;
        }
        public Vector2 RandomBorderSpawn()
        {
            Vector2 spawnLocation = new Vector2(-spawnWidth, -spawnWidth); // Just in case of bugs ( this helped me once already )
            int spawnSide = Random.Integer(0, 4);
            if (spawnSide == 0) // Left
            {
                // Each version of this for left, top, right, and left helps the object spawn randomly just enough outside the screen so that it may not spawn on the screen
                spawnLocation = new Vector2(Random.Float(-Enemy.radius*2, -spawnWidth), Random.Float(0, Game.windowSize[1])); 
            }
            if (spawnSide == 1) // Top
            {
                spawnLocation = new Vector2(Random.Float(0, Game.windowSize[0]), Random.Float(-Enemy.radius*2, -spawnWidth));
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
            // This is just for the rubric criteria, as just looping 100 times and doing AddEntity() does much better here;
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i] = new Star(this);
                AddEntity(stars[i]);
            }
        }
        
            
        

    }
}
