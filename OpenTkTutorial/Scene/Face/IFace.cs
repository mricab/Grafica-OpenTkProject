using System;
namespace OpenTkProject
{
    public interface IFace
    {
        void Initialize();
        void Draw(bool Polygon = false);
        void Delete();
    }
}
