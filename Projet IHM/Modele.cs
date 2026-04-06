using System;
using System.Collections.Generic;
using System.Text;
using Projet_IHM.Movable.Shape;
using Projet_IHM.Movable.Shape.Simple;

namespace Projet_IHM
{
    internal class Modele
    {
        public event Action OnModelChanged;

        private FreeHand currentFHDraw = null;

        public FreeHand getFH() => currentFHDraw;

        private List<Movable.Movable> formes = new List<Movable.Movable>
        {
            new Rect(new Point(0, 0), new Size(410, 210)),
            new Ellipse(new Point(500, 500), new Size(350, 505)),
            new Square(new Point(100, 600), 300, false)
        };

        private HashSet<Movable.Movable> selectedFormes = new HashSet<Movable.Movable>();

        public int FormesCount() { return formes.Count; }

        public void addFH()
        {
            if (currentFHDraw != null)
            {
                currentFHDraw.ClearLastPoint();
                formes.Add(currentFHDraw);
                currentFHDraw = null;
                OnModelChanged?.Invoke();
            }
        }

        public void addMouseFH(Point p)
        {
            if (currentFHDraw != null)
            {
                currentFHDraw.SetLastPoint(p);
                OnModelChanged?.Invoke();
            }
        }

        public Movable.Movable GetForme(int index) => formes[index]; 

        public void addSelected(int idx) { selectedFormes.Add(formes[idx]); OnModelChanged?.Invoke(); }
        public void removeSelected() { selectedFormes.Clear(); OnModelChanged?.Invoke(); }

        public void AddForme(Movable.Movable forme) { formes.Add(forme); OnModelChanged?.Invoke(); }

        public List<Movable.Movable> GetMovables() => formes; 
        public HashSet<Movable.Movable> GetSelected() => selectedFormes;

        public void MoveSelected(Point newPos)
        {
            foreach (var forme in selectedFormes)
            {
                forme.updatePosition(newPos);
            }
            OnModelChanged?.Invoke();
        }

        public void setDeltaMouse(Point mousePos)
        {
            foreach (var forme in selectedFormes)
            {
                forme.setDeltaMouse(mousePos);
            }
            OnModelChanged?.Invoke();
        }

        public bool collide(Point p)
        {
            for (int i = 0; i < FormesCount(); i++)
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

        public void removeCollide(Point p)
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


        public void addFHPoint(Point p)
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
    }
}
