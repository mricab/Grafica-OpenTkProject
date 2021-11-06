using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using OpenTK.Mathematics;

namespace OpenTkProject
{
    public static class Design
    {
        static public Object BuildHouse()
        {
            // House
            Object House = new Object(new float[] { 0.0f, 0.0f, 0.0f });

                // House Foundation
                Parallelepiped Foundation = new Parallelepiped(0.1f, 1.1f, 2.1f, new float[] { 0.0f, 0.0f, 0.0f }, Colors.Concrete);
                House.AddObject("Foundation", Foundation);

                    // House Body
                    Parallelepiped HouseBody = new Parallelepiped(1.0f, 1.0f, 2.0f, new float[] { 0.0f, 0.5f, 0.0f }, Colors.DarkBeige);
                    Foundation.AddObject("House Body", HouseBody);

                        // Door
                        Parallelepiped Door = new Parallelepiped(0.5f, 0.3f, 0.025f, new float[] { 0.0f, -0.25f, 1.0f }, Colors.Wood);
                        HouseBody.AddObject("Door", Door);

                        // House Roof
                        Object Roof = new Object(new float[] { 0.0f, 1.0f, 0.0f });
                        HouseBody.AddObject("Roof", Roof);
                        Roof.AddFace(new Triangle(1f, Colors.DarkBeige, 0f, 0f, -1f, 0f, 0f, 0f));
                        Roof.AddFace(new Triangle(1f, Colors.DarkBeige, 0f, 0f, 1f, 0f, 0f, 0f));
                        var aux = 0.5 - MathHelper.Sin(MathHelper.DegreesToRadians(60)) * 0.5;
                        Roof.AddFace(new Rectangle(1f, 2f, Colors.PastelOrange, -0.25f, -(float)aux, 0f, 0f, -90f, -30f));
                        Roof.AddFace(new Rectangle(1f, 2f, Colors.PastelOrange, 0.25f, -(float)aux, 0f, 0f, 90f, 30f));

            // Return
            return House;
        }
    }
}
