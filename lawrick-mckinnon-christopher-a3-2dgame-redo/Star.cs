using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace MohawkGame2D
{
    internal class Star : Entity
    {
        float starChangeTimer;
        float size;
        float starScaleInterval;
        public Star (Scene setScene) : base(setScene)
        {
            this.position = Random.Vector2(new Vector2(0, 0), new Vector2(Scene.Game.windowSize[0], Scene.Game.windowSize[1]));
            this.starChangeTimer = 1f; // Seconds it takes for stars to change;
            this.starScaleInterval = Random.Float(0, this.starChangeTimer * 2); // Set a random interval time (mismatched shining animation)
        }
        public override void Update()
        {
            StarTimer();
            DrawStars();
        }
        public void StarTimer()
        {
            starScaleInterval -= Time.DeltaTime; // Count down timer

            if (starScaleInterval <= 0)
            {
                starScaleInterval = starChangeTimer * 2; // Reset
            }
        }
        public void DrawStars()
        {
            Draw.FillColor = Color.White;
            Draw.LineColor = Color.White;
            if (starScaleInterval <= starChangeTimer)
            {
                Draw.Circle(this.position, 1f);
            }
            if (starScaleInterval <= starChangeTimer * 2 && starScaleInterval > starChangeTimer)
            {
                Draw.Circle(this.position, 2f);
            }
        }
    }
}
