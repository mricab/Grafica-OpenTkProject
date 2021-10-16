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

            // House Foundation
            Parallelepiped Foundation = new Parallelepiped(0.1f, 1.1f, 2.1f, new Vector3(0.0f, 0.0f, 0.0f), Colors.Concrete);
            Root.AddObject("Foundation", Foundation);

            // House Body
            Parallelepiped HouseBody = new Parallelepiped(1.0f, 1.0f, 2.0f, new Vector3(0.0f, 0.5f, 0.0f), Colors.DarkBeige);
            Foundation.AddObject("House Body", HouseBody);

            // Door
            Parallelepiped Door = new Parallelepiped(0.5f, 0.3f, 0.025f, new Vector3(0.0f, -0.25f, 1f), Colors.Wood);
            HouseBody.AddObject("Door", Door);

            // House Roof
            Object Roof = new Object(new Vector3(0.0f, 1.0f, 0.0f));
            HouseBody.AddObject("Roof", Roof);
            Roof.AddFace(new Triangle(1f, Colors.DarkBeige, new Vector3(0f, 0f, -1f), new Vector3(0f, 0f, 0f)));
            Roof.AddFace(new Triangle(1f, Colors.DarkBeige, new Vector3(0f, 0f, 1f), new Vector3(0f, 0f, 0f)));
            var aux = 0.5 - MathHelper.Sin(MathHelper.DegreesToRadians(60)) * 0.5;
            Roof.AddFace(new Rectangle(1f, 2f, Colors.PastelOrange, new Vector3(-0.25f, -(float)aux, 0f), new Vector3(0f, -90f, -30f)));
            Roof.AddFace(new Rectangle(1f, 2f, Colors.PastelOrange, new Vector3(0.25f, -(float)aux, 0f), new Vector3(0f, 90f, 30f)));

            // House Rotation
            Foundation.SetModel(Matrix4.CreateRotationY(MathHelper.DegreesToRadians(-45)));

            // Scene
            Scene Scene = new Scene(Root);

            // Return
            return Scene;
        }
    }
}
