using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace MohawkGame2D
{
    internal class Player : Entity
    {
        public Vector2 velocity;
        public float moveSpeed;
        public float radius;
        public float shotCooldown;
        public float boostCooldown;
        public float boostFactor;
        public float boostTimer;
        public float health;

        public Player(Scene setScene) : base(setScene)
        {
            this.position = Scene.Game.windowCentre; // Start 
            this.velocity = new Vector2(0, 0); // Start 
            this.moveSpeed = 5f;
            this.radius = 20f/2;
            this.shotCooldown = 0.2f; // Seconds shot takes to cooldown
            this.boostCooldown = 1f; // Seconds boost takes to recharge
            this.boostFactor = 5f; // Amount boost scales velocity
            this.boostTimer = 0.3f; // Seconds boost takes place over UNUSED
            this.health = 15f;
        }
        // Intended to be called every frame
        public override void Update()
        {
            Move();
            DrawPlayer();
        }

        public void Move()
        {
            this.position += this.velocity * Time.DeltaTime;
            // Check if hitting border
            Vector2[] bounceResults = Scene.BounceOffBorder(this.position, this.velocity);
            this.position = bounceResults[0];
            this.velocity = bounceResults[1];
        }
        public void AddVelocity(Vector2 addVelocity)
        {
            this.velocity += addVelocity;
        }
        public void Shoot()
        {
            Scene.AddEntity(new Bullet(Scene, this));
        }
        public void GetHit(float damage)
        {
            this.health -= damage;
            if (this.health <= 0)
            {
                Scene.GameEnd();
            }
        }
        public void DrawPlayer()
        {
            Draw.FillColor = Color.Blue;
            Draw.Circle(this.position, radius);
        }
        
    }
}
