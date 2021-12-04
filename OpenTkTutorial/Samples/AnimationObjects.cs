using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTkProject;

namespace OpenTkProject.Samples
{
    public class AnimationObjects
    {
        static public Object Table()
        {
            Object Table = new Object(new float[] { 0, -0.4f, 0 });

            Face Top = new Parallelepiped(0.1f, 2.5f, 1f, new float[] { 0, 0.4f, 0 }, new float[] { 0.0f, 0.0f, 0.0f }, Colors.Wood);
            Face Leg1 = new Parallelepiped(0.7f, 0.2f, 0.2f, new float[] { -1.15f, 0f,  0.4f }, new float[] { 0.0f, 0.0f, 0.0f }, Colors.AshBlack);
            Face Leg2 = new Parallelepiped(0.7f, 0.2f, 0.2f, new float[] { -1.15f, 0f, -0.4f }, new float[] { 0.0f, 0.0f, 0.0f }, Colors.AshBlack);
            Face Leg3 = new Parallelepiped(0.7f, 0.2f, 0.2f, new float[] {  1.15f, 0f,  0.4f }, new float[] { 0.0f, 0.0f, 0.0f }, Colors.AshBlack);
            Face Leg4 = new Parallelepiped(0.7f, 0.2f, 0.2f, new float[] {  1.15f, 0f, -0.4f }, new float[] { 0.0f, 0.0f, 0.0f }, Colors.AshBlack);
            Table.AddFace("Top", Top);
            Table.AddFace("Leg1", Leg1);
            Table.AddFace("Leg2", Leg2);
            Table.AddFace("Leg3", Leg3);
            Table.AddFace("Leg4", Leg4);

            return Table;
        }


        static public Object Ant()
        {
            Object Ant = new Object(new float[] { -1f, 0.095f, 0 });

            Ant.AddFace("Tail", new Parallelepiped(0.8f, 1f, 0.8f, new float[] { -1.75f, 0, 0 }, new float[] { 0.0f, 0.0f, 0.0f }, Colors.Black));
            Ant.AddFace("TailBody", new Parallelepiped(0.5f, 1f, 0.5f, new float[] { -1f, 0, 0 }, new float[] { 0.0f, 0.0f, 0.0f }, Colors.White));
            Ant.AddFace("Body", new Parallelepiped(1f, 2.0f, 1f, new float[] { 0, 0, 0 }, new float[] { 0.0f, 0.0f, 0.0f }, Colors.Black));
            Ant.AddFace("BodyHead", new Parallelepiped(0.5f, 1f, 0.5f, new float[] { +1f, 0, 0 }, new float[] { 0.0f, 0.0f, 0.0f }, Colors.White));
            Ant.AddFace("Head", new Cube(0.9f, new float[] { 1.75f, 0f, 0 }, new float[] { 0.0f, 0.0f, 0.0f }, Colors.Black));
            Ant.AddFace("RigthLeg1", new Parallelepiped(0.8f, 0.2f, 0.2f, new float[] {  1f, -0.5f,  0.5f }, new float[] { 0.0f, 0.0f, 0.0f }, Colors.Blue));
            Ant.AddFace("RigthLeg2", new Parallelepiped(0.8f, 0.2f, 0.2f, new float[] { -1f, -0.5f, 0.5f }, new float[] { 0.0f, 0.0f, 0.0f }, Colors.Blue));
            Ant.AddFace("RigthLeg3", new Parallelepiped(0.8f, 0.2f, 0.2f, new float[] {  0f, -0.5f,  0.5f }, new float[] { 0.0f, 0.0f, 0.0f }, Colors.Blue));
            Ant.AddFace("LeftLeg1", new Parallelepiped(0.8f, 0.2f, 0.2f, new float[] {  1f, -0.5f, -0.5f }, new float[] { 0.0f, 0.0f, 0.0f }, Colors.Blue));
            Ant.AddFace("LeftLeg2", new Parallelepiped(0.8f, 0.2f, 0.2f, new float[] {  0f, -0.5f, -0.5f }, new float[] { 0.0f, 0.0f, 0.0f }, Colors.Blue));
            Ant.AddFace("LeftLeg3", new Parallelepiped(0.8f, 0.2f, 0.2f, new float[] { -1f, -0.5f, -0.5f }, new float[] { 0.0f, 0.0f, 0.0f }, Colors.Blue));
            Ant.AddFace("RigthAntennae", new Parallelepiped(0.4f, 0.05f, 0.05f, new float[] { 2f, 0.5f, 0.3f }, new float[] { 0.0f, 0.0f, 0.0f }, Colors.Blue));
            Ant.AddFace("LeftAntennae", new Parallelepiped(0.4f, 0.05f, 0.05f, new float[] { 2f, 0.5f, -0.3f }, new float[] { 0.0f, 0.0f, 0.0f }, Colors.Blue));

            Ant.Scale(0.05f);

            return Ant;
        }

        static public Object Book()
        {
            int sheets = 4;
            float coverThick = 0.02f;
            float backThick = 0.02f;
            float sheetThick = 0.01f;

            Object Book = new Object(new float[] { 1f, (coverThick + (sheetThick * sheets) + backThick), 0 });

            Parallelepiped cover = new Parallelepiped(coverThick, 0.4f, 0.6f, new float[] { 0, 0, 0 }, new float[] { -0.2f, 0.0f, 0.0f }, Colors.Blue);
            Book.AddFace("Cover", cover);
            for (int i = 0; i < sheets; i++)
            {
                Book.AddFace("Sheet" + i.ToString(), new Parallelepiped(sheetThick, 0.4f, 0.6f, new float[] { 0, (coverThick/2)+(sheetThick/2)+(sheetThick*i), 0 }, new float[] { -0.2f, 0.0f, 0.0f }, Colors.White));
            }
            Book.AddFace("Back", new Parallelepiped(backThick, 0.4f, 0.6f, new float[] { 0, (coverThick / 2) + (sheets * sheetThick) + (backThick/2), 0 }, new float[] { -0.2f, 0.0f, 0.0f }, Colors.Blue));

            return Book;
        }

        static public void SaveObjects()
        {
            Object table = Table();
            table.Serialize("table.json");
            Object ant = Ant();
            ant.Serialize("ant.json");
            Object book = Book();
            book.Serialize("book.json");
        }
    }
}
