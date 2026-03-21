using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace MohawkGame2D
{
    internal class Bullet
    {
        Player player;
        Vector2 position;
        Vector2 direction;
        Vector2 velocity;
        float size;
        float moveSpeed;
        

        public Bullet(Player setPlayer)
        {
            this.player = setPlayer;
            this.position = player.position;
            this.direction = Vector2.Normalize(Input.GetMousePosition() - this.position);
            this.moveSpeed = 500f;
            this.velocity = this.direction * moveSpeed;
            this.size = 10f / 2;
            
        }
        // Intended to run once every frame
        public void Update()
        {
            Move();

            DrawBullet();

        }
        public void Move()
        {
            this.position += this.velocity * Time.DeltaTime;
            // Check within borders
            if (player.scene.CheckOutsideBorders(this.position))
            {
                player.scene.liveBullets.Remove(this);
            }
        }
        public void DrawBullet()
        {
            Draw.FillColor = Color.Green;
            Draw.Circle(this.position, size);
        }
    }
}
