using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace EasyMonoGame 
{
    /// <summary>
    /// An Actor is an objekt in the game world.
    /// 
    /// Inherit Actor to design objekt to your world.
    /// 
    /// An inheriting class must implement the Update method.
    /// </summary>
    public abstract class Actor
    {
        private Texture2D image;
        private string imageName;
        private Vector2 position;
        // rotation in degrees
        // 0 = right, 90 = down, 180 = left, 270 = up
        private float rotation = 0;
        private World world;
        private bool isFlippedHorizontally = false;
        private float scaleRadius = 1.0f;


        /// <summary>
        /// Get the world this actor lives in.
        /// </summary>
        public  World World
        {
            get { return world; }
            internal set { world = value; }
        }

        public float Width
        {
            get
            {
                if (image == null)
                {
                    return 0;
                }
                return image.Width;
            }
        }
        public float Height
        {
            get
            {
                if (image == null)
                {
                    return 0;
                }
                return image.Height;
            }
        }
        /// <summary>
        /// Set or get the image name for this actor.
        /// </summary>
        public string ImageName
        {
            get { return imageName; }
            set { imageName = value; }
        }
        public bool IsFlippedHorizontally
        {
            get { return isFlippedHorizontally; }
            set { isFlippedHorizontally = value; }
        }
        /// <summary>
        /// Set or get the position for this actor in the world.
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        /// <summary>
        /// Get the actors radius.
        /// </summary>
        public float Radius
        {
            get
            {
                if (world == null)
                {
                    throw new Exception("Actor must be in a world.");
                }
                if (image == null)
                {
                    return 1; // image not loaded yet
                }
                return ScaleRadius * (image.Width + image.Height) / 4;
            }
        }
        /// <summary>
        /// Scale the radius of the collision detection circle. 
        /// 
        /// Example:
        /// 0.9f makes it 90% of the original size.
        /// 1.1f makes it 110% of the original size.
        /// </summary>
        public float ScaleRadius
        {
            get { return scaleRadius; }
            set { scaleRadius = value; }
        }
        /// <summary>
        /// Set or get the rotation of this actor.
        /// </summary>
        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }
        internal Texture2D Image
        {
            get { return image; }
            set { image = value; }
        }
        /// <summary>
        /// Set the x-coordinate for this actor.
        /// </summary>
        public float X
        {
            get
            {
                return position.X;
            }
            set
            {
                position.X = value;
            }
            
        }
        /// <summary>
        /// Set the y-coordinate for this actor.
        /// </summary>
        public float Y
        {
            get
            {
                return position.Y;
            }
            set
            {
                position.Y = value;
            }
        }





        /// <summary>
        /// Called once per frame to draw actor to the screen.
        /// </summary>
        /// <param name="spriteBatch"></param>
        internal void Draw(SpriteBatch spriteBatch)
        {
            /*
            if (image == null)
            {
                image = GameArt.Get(imageName);
            }
            */
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (isFlippedHorizontally)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
            // Draw the actor, centered on the position.
            spriteBatch.Draw(image,
                position,
                null,
                Color.White,
                rotation * MathF.PI / 180f, // Convert to radians
                new Vector2(image.Width / 2, image.Height / 2), // Center image on position
                Vector2.One,
                spriteEffects,
                0f);
        }
        /// <summary>
        /// Get one actor of the specified type. 
        /// Returns null if no intersecting actor was found.
        /// 
        /// Example:
        /// 
        /// GetOneIntersectingActor(typeof(ClassName))
        /// </summary>
        /// <param name="actorType"></param>
        /// <returns></returns>
        public Actor GetOneIntersectingActor(Type actorType)
        {
            if (world == null)
            {
                throw new Exception("Actor must be in a world.");
            }
            if (world.actors.ContainsKey(actorType))
            {
                List<Actor> actorsOfType = world.actors[actorType];
                foreach (var actor in actorsOfType)
                {
                    if (this == actor)
                    {
                        continue;
                    }
                    if (Intersects(actor) && actor.World != null)
                    {
                        return actor;
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// Get all actors of the specified type that intersect with this actor.
        /// </summary>
        /// <param name="actorType"></param>
        /// <returns>Returns a list of actors. 
        /// The list is empty if no intersecting actor was found. </returns>
        /// <exception cref="Exception">This actor must be in a world.</exception>
        public List<Actor> GetAllIntersectingActors(Type actorType)
        {
            if (world == null)
            {
                throw new Exception("Actor must be in a world.");
            }
            List<Actor> intersectingActors = new List<Actor>();
            if (world.actors.ContainsKey(actorType))
            {
                List<Actor> actorsOfType = world.actors[actorType];
                foreach (var actor in actorsOfType)
                {
                    if (this == actor)
                    {
                        continue;
                    }
                    if (Intersects(actor) && actor.World != null)
                    {
                        intersectingActors.Add(actor);
                    }
                }
            }
            return intersectingActors;
        }
        /// <summary>
        /// Returns true if this actor is at the edge of the world.
        /// </summary>
        /// <returns></returns>
        public bool IsAtEdge()
        {
            if (Position.X < 0 || Position.X > World.Width || Position.Y < 0 || Position.Y > World.Height)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Returns true if this actor is touching an actor of the specified type.
        /// 
        /// Example:
        /// 
        /// IsTouching(typeof(ClassName))
        /// </summary>
        /// <param name="actorType"></param>
        /// <returns></returns>
        public bool IsTouching(Type actorType)
        {
            return GetOneIntersectingActor(actorType) != null;
        }
        

        /// <summary>
        /// Move the actor in the direction it is facing.
        /// </summary>
        /// <param name="distance">distance in pixels</param>
        public void Move(float distance)
        {
            if (world == null)
            {
                throw new Exception("Actor must be in a world.");
            }
            var direction = new Vector2((float)Math.Cos(MathHelper.ToRadians(rotation)), (float)Math.Sin(MathHelper.ToRadians(rotation)));
            position += direction * distance;
        }
        /// <summary>
        /// Returns true if this actor intersects with the other actor, 
        /// otherwise false.
        /// </summary>
        /// <param name="otherActor"></param>
        /// <returns></returns>
        public bool Intersects(Actor otherActor)
        {
            if (world == null)
            {
                throw new Exception("Actor must be in a world.");
            }
            if (otherActor.World == null)
            {
                return false;
            }
            if (otherActor == this)
            {
                return false;
            }
            return Vector2.Distance(position, otherActor.Position) < Radius + otherActor.Radius;
        }
        /// <summary>
        /// If this objekt is touching an actor of the specified type,
        /// the other actor is removed.
        /// 
        /// Use IsTouching(...) first to ensure that this actor touch an actor of the specified type.
        /// 
        /// Exmaple:
        /// 
        /// RemoveTouching(typeof(ClassName))
        /// </summary>
        /// <param name="actorType"></param>
        public void RemoveTouching(Type actorType)
        {
            var actor = GetOneIntersectingActor(actorType);
            World.RemoveActor(actor);

        }
        /// <summary>
        /// Turn angle.
        /// </summary>
        /// <param name="angle"></param>
        public void Turn(float angle)
        {
            Rotation += angle;
        }
        /// <summary>
        /// Turn this actor towards the specified coordinate.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void TurnTowards(float x, float y)
        {
            Vector2 directionToOther = new Vector2(x, y) - Position;
            rotation = VectorToAngleInDegrees(directionToOther);
        }
        /// <summary>
        /// This method is called once per frame. Implement game logic in this method.
        /// </summary>
        public virtual void Act()
        {

        }

        /// <summary>
        /// override this method in an inheriting class. 
        /// Implement game logic in the overriding method.
        /// Do NOT call base.Update(gameTime) in the overriding method.
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {
            Act();
        }

        private float VectorToAngleInDegrees(Vector2 direction)
        {
            float angleInRadians = (float)Math.Atan2(direction.Y, direction.X);
            return MathHelper.ToDegrees(angleInRadians);
        }

    }
}
