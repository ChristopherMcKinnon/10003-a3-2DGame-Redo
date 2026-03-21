using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace MohawkGame2D
{
    internal class Bullet : Entity
    {
        public Entity Owner;
        Vector2 direction;
        Vector2 velocity;
        float radius;
        float moveSpeed;
        float damage;
        

        public Bullet(Scene setScene, Entity setOwner) : base(setScene)
        {
            this.Owner = setOwner;
            this.Scene = setScene;
            this.position = Owner.position;
            if (Owner is Player) { this.direction = Vector2.Normalize(Input.GetMousePosition() - this.position); }
            if (Owner is Enemy) { this.direction = Vector2.Normalize(Scene.Player.position - this.position); }
            
            this.moveSpeed = 500f;
            this.velocity = this.direction * moveSpeed;
            this.radius = 10f / 2;
            this.damage = 3f;

            
        }
        // Intended to run once every frame
        public override void Update() // Overrides the base method of Entity
        {
            Move();

            DrawBullet();
            if (Owner is Player)
            {
                EnemyCollision();
            }

        }
        public void Move()
        {
            this.position += this.velocity * Time.DeltaTime;
            // Check within borders
            if (Scene.CheckOutsideBorders(this.position))
            {
                Scene.RemoveEntity(this);
            }
        }
        public void DrawBullet()
        {
            if (Owner is Enemy)
            {
                Draw.FillColor = Color.Red;
            }
            else { Draw.FillColor = Color.Blue; }
            Draw.Circle(this.position, radius);
        }

        public void EnemyCollision()
        {
            foreach (Entity entity in Scene.entities)
            {
                if (entity is Enemy enemy)
                {

                    // Collision Check
                    if (Vector2.DistanceSquared(enemy.position, this.position) <= enemy.boundingRadius * enemy.boundingRadius + this.radius * this.radius)
                    {
                        enemy.GetHit(this.damage);
                        Scene.RemoveEntity(this);
                    }
                }
            }
        }
    }
}
