using Projet_IHM.Movable;
using Projet_IHM.Movable.Shape;
using Projet_IHM.Movable.Shape.Simple;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Xml.Linq;

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
        public Action OnModelChanged = null!;
        public Action<bool, ControleCalque> UpdateCalque = null!;
        public Action<int, int> SwapCalque = null!;

        private FreeHand currentFHDraw = null!;
        private Simple currentSDraw = null!;
        private Rect zoomBorder = new Rect(PointF.Empty, SizeF.Empty, false, Color.DimGray);

        private Color currentColor = Color.Red;

        private Dictionary<int, ControleCalque> calquesControler = new Dictionary<int, ControleCalque>();
        private Dictionary<int, List<Movable.Movable>> calques = new Dictionary<int, List<Movable.Movable>>();
        private List<int> calquesOrder = new List<int>();

        private Dictionary<int, bool> isVisible = new Dictionary<int, bool>();
        private Dictionary<int, bool> isLocked = new Dictionary<int, bool>();

        private int currentCalque = 0;
        private int nbCalque = 0;
        private List<Movable.Movable> formes => calques[currentCalque];
        private HashSet<Movable.Movable> selectedFormes = new HashSet<Movable.Movable>();

        public bool isCalqueVisible(int idx) => isVisible[idx];

        public void CreateNewCalque()
        {
            ControleCalque c = new ControleCalque(nbCalque);
            calquesControler.Add(nbCalque, c);
            setupNewCalque(c, nbCalque);
            while (calquesControler.ContainsKey(nbCalque)) { nbCalque++; }
        }

        public void CreateNewCalque(int id)
        {
            ControleCalque c = new ControleCalque(id);
            calquesControler.Add(id, c);
            setupNewCalque(c, id);
        }

        public void deleteCalque()
        {
            if (isLocked[currentCalque]) return;
            if (!isLocked[currentCalque] && calquesControler.ContainsKey(currentCalque))
            {
                UpdateCalque?.Invoke(false, calquesControler[currentCalque]);
                calquesControler.Remove(currentCalque);
                calquesOrder.Remove(currentCalque);
                OnModelChanged?.Invoke();
            }
        }

        private void setupNewCalque(ControleCalque c, int id)
        {
            calques.Add(id, new List<Movable.Movable>());
            isVisible.Add(id, true);
            isLocked.Add(id, false);
            calquesOrder.Add(id);
            if (calquesControler.ContainsKey(currentCalque))
                calquesControler[currentCalque].SetSelected(false);
            currentCalque = id;
            c.SetSelected(true);
            UpdateCalque?.Invoke(true, c);
            c.updateCalque += (id, type) => {
                int pos;
                switch (type)
                {
                    case UpdateCalqueOption.Visibility:
                        isVisible[id] = !isVisible[id]; OnModelChanged?.Invoke();  break;
                    case UpdateCalqueOption.Lock:
                        isLocked[id] = !isLocked[id]; removeSelected(); break;
                    case UpdateCalqueOption.Down:
                        pos = calquesOrder.IndexOf(id);
                        if (pos > 0)
                        {
                            SwapCalque?.Invoke(pos, pos - 1);
                            int temp = calquesOrder[pos - 1];
                            calquesOrder[pos - 1] = calquesOrder[pos];
                            calquesOrder[pos] = temp;
                            OnModelChanged?.Invoke();
                        }
                        break;
                    case UpdateCalqueOption.Up:
                        pos = calquesOrder.IndexOf(id);
                        if (pos < calquesOrder.Count - 1)
                        {
                            SwapCalque?.Invoke(pos, pos + 1);
                            int temp = calquesOrder[pos + 1];
                            calquesOrder[pos + 1] = calquesOrder[pos];
                            calquesOrder[pos] = temp;
                            OnModelChanged?.Invoke();
                        }
                        break;
                    case UpdateCalqueOption.Choose:
                        if (calquesControler.ContainsKey(currentCalque)) 
                            calquesControler[currentCalque].SetSelected(false);
                        currentCalque = id;
                        calquesControler[currentCalque].SetSelected(true);
                        break;
                }
            };
        }

        public FreeHand getFH() => currentFHDraw;
        public Simple getSimpleDraw() => currentSDraw;
        public Rect getZoomBorder() => zoomBorder;

        public void setColor(Color c) { currentColor = c; }

        public Dictionary<int, List<Movable.Movable>> getCalques() => calques;
        public List<Movable.Movable> getCalques(int idx) => calques[idx];
        public List<int> getCalquesOrder() => calquesOrder;
        

        public void setCalques(Dictionary<int, List<Movable.Movable>> data, List<int> dataOrder)
        {
            calques.Clear();
            isVisible.Clear();
            isLocked.Clear();
            OnModelChanged?.Invoke();
            setupCalqueOrder(dataOrder);
            calquesOrder = dataOrder;
            calques = data;
        }


        private void setupCalqueOrder(List<int> order)
        {

            foreach (var id in calquesOrder)
            {
                UpdateCalque?.Invoke(false, calquesControler[id]);
            }
            calquesControler.Clear();

            foreach (var id in order)
            {
                CreateNewCalque(id);
            }

        }

        public int FormesCount()
        {
            int total = 0;
            foreach (var (id, calque) in calques)
            {
                total += calque.Count;
            }
            return total;
        }
        public int FormesCount(int a) => calques[a].Count;

        public void addFH()
        {
            if (isLocked[currentCalque]) return;
            if (!isLocked[currentCalque] && currentFHDraw != null)
            {
                currentFHDraw.ClearLastPointF();
                formes.Add(currentFHDraw);
                currentFHDraw = null;
                OnModelChanged?.Invoke();
            }
        }

        public void addSimple()
        {
            if (isLocked[currentCalque]) return;
            if (!isLocked[currentCalque] && currentSDraw != null)
            {
                formes.Add(currentSDraw);
                currentSDraw = null;
                OnModelChanged?.Invoke();
            }
        }

        public void addMouseFH(PointF p)
        {
            if (isLocked[currentCalque]) return;
            if (!isLocked[currentCalque] && currentFHDraw != null)
            {
                currentFHDraw.SetLastPointF(p);
                OnModelChanged?.Invoke();
            }
        }

        public void setPosZoom(PointF p)
        {
            zoomBorder.updatePosition(p);
            OnModelChanged?.Invoke();
        }

        public void setSizeZoom(PointF p)
        {
            zoomBorder.setEndBoundingBox(p);
            OnModelChanged?.Invoke();
        }

        public void clearZoom() 
        { 
            zoomBorder.updatePosition(PointF.Empty);
            zoomBorder.setEndBoundingBox(PointF.Empty);
            OnModelChanged?.Invoke();
        }

        public Movable.Movable GetForme(int index) => formes[index]; 

        public void addSelected(int idx) { selectedFormes.Add(formes[idx]); OnModelChanged?.Invoke(); }
        public void removeSelected() { selectedFormes.Clear(); OnModelChanged?.Invoke(); }

        public void AddForme(Movable.Movable forme) { formes.Add(forme); OnModelChanged?.Invoke(); }

        public IReadOnlyList<Movable.Movable> GetMovables()
        {
            List<Movable.Movable> resultat = calques
                .Where(kvp => isVisible.TryGetValue(kvp.Key, out bool estActif) && estActif)
                .SelectMany(kvp => kvp.Value)
                .ToList();
            return resultat;
        } 
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
            if (isLocked[currentCalque]) return false;
            for (int i = FormesCount(currentCalque) - 1; i >= 0; i--)
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
            if (isLocked[currentCalque]) return;
            if (currentFHDraw != null)
            {
                currentFHDraw.Add(p);
                OnModelChanged?.Invoke();
            }
            else
            {
                currentFHDraw = new FreeHand(p, currentColor);
                OnModelChanged?.Invoke();
            }
        }

        public void setPosNewShape(PointF p, Types t)
        {
            if (isLocked[currentCalque]) return;
            if (currentSDraw == null)
            {
                switch (t)
                {
                    case (Types.Rect):
                        currentSDraw = new Rect(p, currentColor);
                        break;
                    case (Types.Square):
                        currentSDraw = new Square(p, currentColor);
                        break;
                    case (Types.Ellipse):
                        currentSDraw = new Ellipse(p, currentColor);
                        break;
                    case (Types.Circle):
                        currentSDraw = new Circle(p, currentColor);
                        break;
                }
            }
        }

        public void setSizeSimple(PointF p)
        {
            if (isLocked[currentCalque]) return;
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
