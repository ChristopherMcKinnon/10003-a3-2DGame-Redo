using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace MohawkGame2D
{
    internal class Player
    {
        public Scene scene;
        public Vector2 position;
        public Vector2 velocity;
        public float moveSpeed;
        public float size;
        public float shotCooldown;
        public float boostCooldown;
        public float boostFactor;
        public float boostTimer;

        public Player(Scene setScene)
        {
            this.scene = setScene;
            this.position = scene.game.windowCentre; // Start 
            this.velocity = new Vector2(0, 0); // Start 
            this.moveSpeed = 5f;
            this.size = 20f/2;
            this.shotCooldown = 0.2f; // Seconds shot takes to cooldown
            this.boostCooldown = 1f; // Seconds boost takes to recharge
            this.boostFactor = 5f; // Amount boost scales velocity
            this.boostTimer = 0.3f; // Seconds boost takes place over UNUSED
        }
        // Intended to be called every frame
        public void Update()
        {
            Move();
            DrawPlayer();
        }

        public void Move()
        {
            this.position += this.velocity * Time.DeltaTime;
            // Check if hitting border
            Vector2[] bounceResults = scene.BounceOffBorder(this.position, this.velocity);
            this.position = bounceResults[0];
            this.velocity = bounceResults[1];
        }
        public void DrawPlayer()
        {
            Draw.FillColor = Color.Blue;
            Draw.Circle(this.position, size);
        }

        public void AddVelocity(Vector2 addVelocity)
        {
            this.velocity += addVelocity;
        }

        public void Shoot()
        {
            scene.liveBullets.Add(new Bullet(this));
        }
        
    }
}
