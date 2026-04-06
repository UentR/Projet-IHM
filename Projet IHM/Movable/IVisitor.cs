using Projet_IHM.Movable.Shape;
using Projet_IHM.Movable.Shape.Simple;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Projet_IHM.Movable
{
    internal interface IVisitor
    {
        public void Visit(Rect rect);
        public void Visit(Ellipse ellipse);
        public void Visit(FreeHand fh);
    }
}
