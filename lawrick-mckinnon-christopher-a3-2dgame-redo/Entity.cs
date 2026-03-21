using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace MohawkGame2D
{
    internal class Entity
    {
        public Scene Scene;
        public Vector2 position;

        public Entity(Scene setScene)
        {
            this.Scene = setScene;
        }
        public virtual void Update() // This method will allow all subclasses (Player, Bullet, etc..) to overwrite it, but still have Scene's entity list be able to reference it individually
        {

        }
    }
}
