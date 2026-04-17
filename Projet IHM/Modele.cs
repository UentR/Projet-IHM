using System;
using System.Collections.Generic;
using System.Text;
using Projet_IHM.Movable.Shape;
using Projet_IHM.Movable.Shape.Simple;

namespace Projet_IHM
{
    enum Types
    {
        Rect,
        Square,
        Ellipse,
        Circle,
    }




    internal class Modele
    {
        public event Action OnModelChanged;

        private FreeHand currentFHDraw = null;
        private Simple currentSDraw = null;


        public FreeHand getFH() => currentFHDraw;
        public Simple getSimpleDraw() => currentSDraw;

        private List<Movable.Movable> formes = new List<Movable.Movable>
        {
            new Rect(new PointF(0, 0), new Size(410, 210)),
            new Ellipse(new PointF(500, 500), new Size(350, 505)),
            new Square(new PointF(100, 600), 300, false)
        };

        private HashSet<Movable.Movable> selectedFormes = new HashSet<Movable.Movable>();

        public int FormesCount() => formes.Count;

        public void addFH()
        {
            if (currentFHDraw != null)
            {
                currentFHDraw.ClearLastPointF();
                formes.Add(currentFHDraw);
                currentFHDraw = null;
                OnModelChanged?.Invoke();
            }
        }

        public void addSimple()
        {
            if (currentSDraw != null)
            {
                formes.Add(currentSDraw);
                currentSDraw = null;
                OnModelChanged?.Invoke();
            }
        }

        public void addMouseFH(PointF p)
        {
            if (currentFHDraw != null)
            {
                currentFHDraw.SetLastPointF(p);
                OnModelChanged?.Invoke();
            }
        }




        public Movable.Movable GetForme(int index) => formes[index]; 

        public void addSelected(int idx) { selectedFormes.Add(formes[idx]); OnModelChanged?.Invoke(); }
        public void removeSelected() { selectedFormes.Clear(); OnModelChanged?.Invoke(); }

        public void AddForme(Movable.Movable forme) { formes.Add(forme); OnModelChanged?.Invoke(); }

        public IReadOnlyList<Movable.Movable> GetMovables() => formes.AsReadOnly();
        public HashSet<Movable.Movable> GetSelected() => selectedFormes;

        public void MoveSelected(PointF newPos)
        {
            foreach (var forme in selectedFormes)
            {
                forme.updatePosition(newPos);
            }
            OnModelChanged?.Invoke();
        }

        public void setDeltaMouse(PointF mousePos)
        {
            foreach (var forme in selectedFormes)
            {
                forme.setDeltaMouse(mousePos);
            }
            OnModelChanged?.Invoke();
        }

        public bool collide(PointF p)
        {
            for (int i = FormesCount() - 1; i >= 0; i--)
            {
                if (formes[i].isInside(p))
                {
                    selectedFormes.Add(formes[i]);
                    OnModelChanged?.Invoke();
                    return true;
                }
            }
            return false;
        }

        public void removeCollide(PointF p)
        {
            foreach (var kvp in selectedFormes)
            {
                if (kvp.isInside(p))
                {
                    selectedFormes.Remove(kvp);
                    OnModelChanged?.Invoke();
                    return;
                }
            }
        }


        public void addFHPointF(PointF p)
        {
            if (currentFHDraw != null)
            {
                currentFHDraw.Add(p);
                OnModelChanged?.Invoke();
            }
            else
            {
                currentFHDraw = new FreeHand(p);
                OnModelChanged?.Invoke();
            }
        }

        public void setPosNewShape(PointF p, Types t)
        {
            if (currentSDraw == null)
            {
                switch (t)
                {
                    case (Types.Rect):
                        currentSDraw = new Rect(p);
                        break;
                    case (Types.Square):
                        currentSDraw = new Square(p);
                        break;
                    case (Types.Ellipse):
                        currentSDraw = new Ellipse(p);
                        break;
                    case (Types.Circle):
                        currentSDraw = new Circle(p);
                        break;
                }
            }
        }

        public void setSizeSimple(PointF p)
        {
            if (currentSDraw != null) 
            { 
                SizeF s = p.Subtract(currentSDraw.getPosition());

                currentSDraw.resize(s);
                OnModelChanged?.Invoke();
            }
        }
    }

    public static class PointFExtensions
    {
        public static SizeF Subtract(this PointF p1, PointF p2)
        {
            return new SizeF(p1.X - p2.X, p1.Y - p2.Y);
        }
    }

    public static class RectangleFExtensions
    {
        public static RectangleF multiply(this RectangleF rect, SizeF s)
        {
            RectangleF newRect = rect;
            newRect.Width *= s.Width;
            newRect.Height *= s.Height;
            newRect.Location = new PointF(rect.X*s.Width, rect.Y*s.Height);
            return newRect;
        }
    }
}
