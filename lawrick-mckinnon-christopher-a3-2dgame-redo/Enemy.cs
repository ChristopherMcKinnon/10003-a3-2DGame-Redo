using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace MohawkGame2D
{
    internal class Enemy : Entity
    {
        public Vector2 velocity;
        public Vector2 direction;
        public float moveSpeed;
        public static float radius = 40f / 2;
        public float boundingRadius;
        public float shotCooldown;
        public float health;
        float shotInterval;

        public Enemy(Scene setScene) : base(setScene)
        {
            this.position = Scene.RandomBorderSpawn(); // Start 
            this.direction = Vector2.Normalize(Scene.Player.position-this.position);
            this.moveSpeed = 15f;
            this.velocity = this.direction * moveSpeed; // Start 
            this.boundingRadius = radius + 5f;
            this.shotCooldown = 1f;
            this.shotInterval = 0;
            this.health = 10f;
        }
        public override void Update()
        {
            Move();
            DrawEnemy();
            Shoot();
        }
        public void Move()
        {
            this.position += velocity * Time.DeltaTime;
            // Check if hitting border
            /*
            Vector2[] bounceResults = Scene.BounceOffBorder(this.position, this.velocity);
            this.position = bounceResults[0];
            this.velocity = bounceResults[1];
            */

            if (Scene.CheckOutsideBorders(this.position))
            {
                Scene.RemoveEntity(this);
            }
        }
        public void AddVelocity(Vector2 addVelocity)
        {
            this.velocity += addVelocity;
        }
        public void Shoot()
        {

            // Count down timer
            shotInterval -= Time.DeltaTime;
            // Shoot
            if (shotInterval <= 0)
            {
                Scene.AddEntity(new Bullet(Scene, this));
                shotInterval = shotCooldown; // Reset
            }
        }
        public void GetHit(float damage)
        {
            this.health -= damage;
            if (this.health < 0)
            {
                Scene.RemoveEntity(this);
            }
        }
        public void DrawEnemy()
        {
            Draw.FillColor = Color.Red;
            Draw.Circle(position, radius);
        }

    }
}
