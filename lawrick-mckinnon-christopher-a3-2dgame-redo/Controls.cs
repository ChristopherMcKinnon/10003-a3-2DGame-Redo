using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace MohawkGame2D
{
    internal class Controls
    {
        Scene scene;
        float moveSpeed;
        float boostFactor;
        float shotCooldown;
        float boostCooldown;
        float boostTimer;
        float shotInterval; // Measures how long it's been since the last shot
        float boostInterval; // Measures how long it's been since the last boost

        public Controls(Scene setScene)
        {
            this.scene = setScene;
            this.moveSpeed = scene.player.moveSpeed;
            this.boostFactor = scene.player.boostFactor;
            this.shotCooldown = scene.player.shotCooldown;
            this.boostCooldown = scene.player.boostCooldown;
            this.boostCooldown = scene.player.boostTimer;
            this.shotInterval = 0;
            this.boostInterval = 0;
        }
        public void Update()
        {
            CheckMove();
            CheckShoot();
            //CheckBurst();
            

        }
        public void CheckMove()
        {
            // Normal movement
            if (Input.IsKeyboardKeyDown(KeyboardInput.W)) // Up
            {
                scene.player.AddVelocity(new Vector2(0, -moveSpeed));
            }
            if (Input.IsKeyboardKeyDown(KeyboardInput.S)) // Down
            {
                scene.player.AddVelocity(new Vector2(0, moveSpeed));
            }
            if (Input.IsKeyboardKeyDown(KeyboardInput.A)) // Left
            {
                scene.player.AddVelocity(new Vector2(-moveSpeed, 0));
            }
            if (Input.IsKeyboardKeyDown(KeyboardInput.D)) // Right
            {
                scene.player.AddVelocity(new Vector2(moveSpeed, 0));
            }
        }
        public void CheckBurst()
        {
            // Count down timer
            boostInterval -= Time.DeltaTime;
            // Burst
            if (Input.IsKeyboardKeyDown(KeyboardInput.Space) && boostInterval <= 0)
            {
                scene.player.AddVelocity(scene.player.velocity * boostFactor);
                boostInterval = boostCooldown; // Reset
            }
        }
        public void CheckShoot()
        {
            // Count down timer
            shotInterval -= Time.DeltaTime;
            // Shoot
            if (Input.IsMouseButtonDown(MouseInput.Left) && shotInterval <= 0)
            {
                scene.player.Shoot();
                shotInterval = shotCooldown; // Reset
            }
        }
    }
}
