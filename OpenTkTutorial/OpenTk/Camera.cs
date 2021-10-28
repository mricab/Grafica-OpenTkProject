using System;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenTkProject
{
    public class Camera
    {
        /* Properties */

        // Camera Position
        public Vector3 Position { get; set; }

        // Camera Movement
        const float cameraSpeed = 1.5f;
        const float sensitivity = 0.2f;
        private bool MouseFirstMove;
        private Vector2 MouseLastPosition;
        private Keys[] keys = new Keys[6] {
            Keys.W, Keys.S,     // forward, backward
            Keys.A, Keys.D,     // left, right
            Keys.Y, Keys.H      // up, down
        };

        // Camera Coordinate System
        private Vector3 Front = -Vector3.UnitZ;
        private Vector3 Up = Vector3.UnitY;
        private Vector3 Right = Vector3.UnitX;

        // Camera Rotations        
        private float _pitch;                           // Rotation around the X axis (radians)
        private float _yaw = -MathHelper.PiOver2;       // Rotation around the Y axis (radians)

        // Camera Field of View (radians)
        private float _fov = MathHelper.PiOver2;

        // Camera Aspect-Ratio
        public float AspectRatio;



        /* Methods */

        public Camera(Vector3 position, float aspectRatio)
        {
            Position = position;
            AspectRatio = aspectRatio;
        }

        public float Pitch
        {
            get => MathHelper.RadiansToDegrees(_pitch);
            set
            {
                var angle = MathHelper.Clamp(value, -89f, 89f);
                _pitch = MathHelper.DegreesToRadians(angle);
                UpdateVectors();
            }
        }

        public float Yaw
        {
            get => MathHelper.RadiansToDegrees(_yaw);
            set
            {
                _yaw = MathHelper.DegreesToRadians(value);
                UpdateVectors();
            }
        }

        private void UpdateVectors()
        {
            Front.X = MathF.Cos(_pitch) * MathF.Cos(_yaw);
            Front.Y = MathF.Sin(_pitch);
            Front.Z = MathF.Cos(_pitch) * MathF.Sin(_yaw);

            Front = Vector3.Normalize(Front);

            Right = Vector3.Normalize(Vector3.Cross(Front, Vector3.UnitY));
            Up = Vector3.Normalize(Vector3.Cross(Right, Front));
        }

        public float Fov
        {
            get => MathHelper.RadiansToDegrees(_fov);
            set
            {
                var angle = MathHelper.Clamp(value, 1f, 45f);
                _fov = MathHelper.DegreesToRadians(angle);
            }
        }

        public Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(Position, Position + Front, Up);
        }

        public Matrix4 GetProjectionMatrix()
        {
            return Matrix4.CreatePerspectiveFieldOfView(_fov, AspectRatio, 0.01f, 100f);
        }



        /* Camera Aspect-Ratio */

        public void SetAspectRatio(float AspectRatio)
        {
            this.AspectRatio = AspectRatio;
        }


        /* Camera Movement */

        public void SetKeys(Keys[] keys)
        {
            this.keys = keys;
        }

        public void CheckInput(KeyboardState input, MouseState mouse, float time)
        {
            // Keyboard
            if (input.IsKeyDown(keys[0])) { Position += Front * cameraSpeed * time; };  // Move Camera Forward.
            if (input.IsKeyDown(keys[1])) { Position -= Front * cameraSpeed * time; };  // Move Camera Backward.
            if (input.IsKeyDown(keys[2])) { Position -= Right * cameraSpeed * time; };  // Move Camera Left.
            if (input.IsKeyDown(keys[3])) { Position += Right * cameraSpeed * time; };  // Move Camera Right.
            if (input.IsKeyDown(keys[4])) { Position += Up * cameraSpeed * time; };     // Move Camera Up.
            if (input.IsKeyDown(keys[5])) { Position -= Up * cameraSpeed * time; };     // Move Camera Down.

            Console.WriteLine("(Camera Input)\tKeyboard Checked.");
        }

        public void SetPerspective(float X, float Y)
        {
            // Mouse            
            if (MouseFirstMove)
            {
                MouseLastPosition = new Vector2(X, Y);
                MouseFirstMove = false;
            }
            else
            {
                // Mouse's offset
                var deltaX = X - MouseLastPosition.X;
                var deltaY = Y - MouseLastPosition.Y;
                MouseLastPosition = new Vector2(X, Y);
                // Pitch and Yaw
                Yaw += deltaX * sensitivity;
                Pitch -= deltaY * sensitivity;
            }
        }

        public void SetZoom(float offset)
        {
            Fov -= offset;
        }


    }
}
