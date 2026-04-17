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
        public void Visit(Rect rect, SizeF ratio);
        public void Visit(Ellipse ellipse, SizeF ratio);
        public void Visit(FreeHand fh, SizeF ratio);
    }
}
