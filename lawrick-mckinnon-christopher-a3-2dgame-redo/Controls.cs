using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace MohawkGame2D
{
    internal class Controls
    {
        Scene Scene;
        float moveSpeed;
        float boostFactor;
        float shotCooldown;
        float boostCooldown;
        float boostTimer;
        float shotInterval; // Measures how long it's been since the last shot
        float boostInterval; // Measures how long it's been since the last boost

        public Controls(Scene setScene)
        {
            this.Scene = setScene;
            this.moveSpeed = Scene.Player.moveSpeed;
            this.boostFactor = Scene.Player.boostFactor;
            this.shotCooldown = Scene.Player.shotCooldown;
            this.boostCooldown = Scene.Player.boostCooldown;
            this.boostCooldown = Scene.Player.boostTimer;
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
                Scene.Player.AddVelocity(new Vector2(0, -moveSpeed));
            }
            if (Input.IsKeyboardKeyDown(KeyboardInput.S)) // Down
            {
                Scene.Player.AddVelocity(new Vector2(0, moveSpeed));
            }
            if (Input.IsKeyboardKeyDown(KeyboardInput.A)) // Left
            {
                Scene.Player.AddVelocity(new Vector2(-moveSpeed, 0));
            }
            if (Input.IsKeyboardKeyDown(KeyboardInput.D)) // Right
            {
                Scene.Player.AddVelocity(new Vector2(moveSpeed, 0));
            }
        }
        public void CheckBurst()
        {
            // Count down timer
            boostInterval -= Time.DeltaTime;
            // Burst
            if (Input.IsKeyboardKeyDown(KeyboardInput.Space) && boostInterval <= 0)
            {
                Scene.Player.AddVelocity(Scene.Player.velocity * boostFactor);
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
                Scene.Player.Shoot();
                shotInterval = shotCooldown; // Reset
            }
        }
    }
}
