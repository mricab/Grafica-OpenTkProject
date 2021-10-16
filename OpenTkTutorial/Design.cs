using System;
using OpenTK.Mathematics;

namespace OpenTkProject
{
    public static class Design
    {
        static public Scene Get()
        {
            // Root Object
            RootObject Root = new RootObject();

            // House Body
            Parallelepiped HouseBody = new Parallelepiped(1.0f, 1.0f, 2.0f, new Vector3(0.0f, 0.0f, 0.0f), Colors.DarkBeige);
            Root.AddObject("House Body", HouseBody);

            // House Roof
            Object Roof = new Object(new Vector3(0.0f, 1.0f, 0.0f));
            HouseBody.AddObject("Roof", Roof);
            Roof.AddFace(new Triangle(1f, Colors.DarkBeige, new Vector3(0f, 0f, -1f), new Vector3(0f, 0f, 0f)));
            Roof.AddFace(new Triangle(1f, Colors.DarkBeige, new Vector3(0f, 0f, 1f), new Vector3(0f, 0f, 0f)));
            var aux = 0.5 - MathHelper.Sin(MathHelper.DegreesToRadians(60)) * 0.5;
            Roof.AddFace(new Rectangle(1f, 2f, Colors.Green, new Vector3(-0.25f, -(float)aux, 0f), new Vector3(0f, -90f, -30f)));
            Roof.AddFace(new Rectangle(1f, 2f, Colors.Blue, new Vector3(0.25f, -(float)aux, 0f), new Vector3(0f, 90f, 30f)));

            // House Rotation
            HouseBody.SetModel(Matrix4.CreateRotationY(MathHelper.DegreesToRadians(-45)));

            // Scene
            Scene Scene = new Scene(Root);

            // Return
            return Scene;
        }
    }
}
