using Projet_IHM.Movable;
using Projet_IHM.Movable.Shape;
using Projet_IHM.Movable.Shape.Simple;
using Projet_IHM.Movable.Shape.Complex;
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
        Stars,
        Line
    }

    internal class Modele
    {
        #region 1. Événements et Callbacks
        public Action OnModelChanged = null!;
        public Action<bool, ControleCalque> UpdateCalque = null!;
        public Action<int, int> SwapCalque = null!;
        #endregion

        
        #region 2. Attributs (État du modèle)
        // Outils et couleurs
        private FreeHand currentFHDraw = null!;
        private Simple currentSDraw = null!;
        private Rect zoomBorder = new Rect(PointF.Empty, SizeF.Empty, false, Color.DimGray);
        private int nbPointsStar = 5;
        private Color currentColor = Color.Red;

        // Gestion des calques
        private Dictionary<int, ControleCalque> calquesControler = new Dictionary<int, ControleCalque>();
        private Dictionary<int, List<Movable.Movable>> calques = new Dictionary<int, List<Movable.Movable>>();
        private List<int> calquesOrder = new List<int>();
        private Dictionary<int, bool> isVisible = new Dictionary<int, bool>();
        private Dictionary<int, bool> isLocked = new Dictionary<int, bool>();
        private int currentCalque = 0;
        private int nbCalque = 0;

        // Sélections
        private List<Movable.Movable> formes => calques[currentCalque];
        private HashSet<Movable.Movable> selectedFormes = new HashSet<Movable.Movable>();
        #endregion

        
        #region 3. Gestion des Calques
        public bool isCalqueVisible(int idx) => isVisible[idx];
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
                        isVisible[id] = !isVisible[id]; OnModelChanged?.Invoke(); break;
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

        public void NouveauProjet()
        {
            foreach (var id in calquesOrder)
            {
                UpdateCalque?.Invoke(false, calquesControler[id]);
            }

            calques.Clear();
            calquesControler.Clear();
            calquesOrder.Clear();
            isVisible.Clear();
            isLocked.Clear();
            selectedFormes.Clear();

            nbCalque = 0;
            currentCalque = 0;

            CreateNewCalque();
            OnModelChanged?.Invoke();
        }

        #endregion


        #region 4. Gestion de la Sélection
        public HashSet<Movable.Movable> GetSelected() => selectedFormes;
        public void addSelected(int idx) { selectedFormes.Add(formes[idx]); OnModelChanged?.Invoke(); }
        public void removeSelected() { selectedFormes.Clear(); OnModelChanged?.Invoke(); }

        public void deleteSelected()
        {
            if (selectedFormes.Count == 0) return;

            foreach (var kvp in calques)
            {
                int calqueId = kvp.Key;
                List<Movable.Movable> listeFormes = kvp.Value;

                if (!isLocked[calqueId])
                {
                    listeFormes.RemoveAll(forme => selectedFormes.Contains(forme));
                }
            }
            OnModelChanged?.Invoke();
        }

        public void MoveSelected(PointF newPos)
        {
            foreach (var forme in selectedFormes)
            {
                forme.updatePosition(newPos);
            }
            OnModelChanged?.Invoke();
        }

        public void ResizeSelected(PointF mousePos, HandleType handle)
        {
            foreach (var forme in selectedFormes)
            {
                forme.ResizeFromHandle(mousePos, handle);
            }
            OnModelChanged?.Invoke();
        }
        #endregion

        
        #region 5. Gestion et Ajout des Formes
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
        public Movable.Movable GetForme(int index) => formes[index];

        public IReadOnlyList<Movable.Movable> GetMovables()
        {
            List<Movable.Movable> resultat = calques
                .Where(kvp => isVisible.TryGetValue(kvp.Key, out bool estActif) && estActif)
                .SelectMany(kvp => kvp.Value)
                .ToList();
            return resultat;
        }

        public void AddForme(Movable.Movable forme) { formes.Add(forme); OnModelChanged?.Invoke(); }

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

        public void addStar(PointF p)
        {
            if (isLocked[currentCalque]) return;
            if (currentFHDraw == null)
                currentFHDraw = new Stars(p, nbPointsStar, 0, currentColor);
        }

        public void resizeStar(PointF p)
        {
            if (isLocked[currentCalque]) return;
            if (currentFHDraw is Stars star)
            {
                star.Resize(p);
                OnModelChanged?.Invoke();
            }
        }

        public void updateNbPicStar(int dir)
        {
            if (isLocked[currentCalque]) return;
            if (currentFHDraw is Stars star)
            {
                star.modNbPic(dir);
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
                    case (Types.Rect): currentSDraw = new Rect(p, currentColor); break;
                    case (Types.Square): currentSDraw = new Square(p, currentColor); break;
                    case (Types.Ellipse): currentSDraw = new Ellipse(p, currentColor); break;
                    case (Types.Circle): currentSDraw = new Circle(p, currentColor); break;
                    case (Types.Line): currentSDraw = new Line(p, currentColor); break;
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

        public void AddLabel(PointF p, string text)
        {
            if (isLocked[currentCalque]) return;
            if (!string.IsNullOrEmpty(text))
            {
                formes.Add(new TextLabel(p, text, currentColor));
                OnModelChanged?.Invoke();
            }
        }

        public void MoveUpShape()
        {
            if (selectedFormes.Count == 0) return;
            bool hasChanged = false;
            List<(Movable.Movable forme, int source, int dest)> deplacements = new List<(Movable.Movable, int, int)>();

            foreach (var forme in selectedFormes)
            {
                int currentLayerId = GetLayerIdOfShape(forme);
                if (currentLayerId == -1) continue;

                int orderIndex = calquesOrder.IndexOf(currentLayerId);
                if (orderIndex >= calquesOrder.Count - 1) continue;

                int nextLayerId = calquesOrder[orderIndex + 1];

                if (!isLocked[currentLayerId] && !isLocked[nextLayerId])
                {
                    deplacements.Add((forme, currentLayerId, nextLayerId));
                }
            }

            foreach (var dep in deplacements)
            {
                calques[dep.source].Remove(dep.forme);
                calques[dep.dest].Add(dep.forme);
                hasChanged = true;
            }

            if (hasChanged) OnModelChanged?.Invoke();
        }

        public void MoveDownShape()
        {
            if (selectedFormes.Count == 0) return;
            bool hasChanged = false;
            List<(Movable.Movable forme, int source, int dest)> deplacements = new List<(Movable.Movable, int, int)>();

            foreach (var forme in selectedFormes)
            {
                int currentLayerId = GetLayerIdOfShape(forme);
                if (currentLayerId == -1) continue;

                int orderIndex = calquesOrder.IndexOf(currentLayerId);
                if (orderIndex <= 0) continue;

                int prevLayerId = calquesOrder[orderIndex - 1];

                if (!isLocked[currentLayerId] && !isLocked[prevLayerId])
                {
                    deplacements.Add((forme, currentLayerId, prevLayerId));
                }
            }

            foreach (var dep in deplacements)
            {
                calques[dep.source].Remove(dep.forme);
                calques[dep.dest].Add(dep.forme);
                hasChanged = true;
            }

            if (hasChanged) OnModelChanged?.Invoke();
        }

        private int GetLayerIdOfShape(Movable.Movable forme)
        {
            foreach (var kvp in calques)
            {
                if (kvp.Value.Contains(forme)) return kvp.Key;
            }
            return -1;
        }
        #endregion

        
        #region 6. Collisions et Interactions Souris
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
        #endregion

        
        #region 7. Utilitaires de Rendu et Zoom
        public FreeHand getFH() => currentFHDraw;
        public Simple getSimpleDraw() => currentSDraw;
        public Rect getZoomBorder() => zoomBorder;

        public void setColor(Color c)
        {
            currentColor = c;
            foreach (var forme in selectedFormes)
            {
                if (forme is Shape s)
                    s.SetColor(currentColor);
            }
            OnModelChanged?.Invoke();
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
        #endregion
    }

    
    #region Extensions
    public static class PointFExtensions
    {
        public static SizeF Subtract(this PointF p1, PointF p2)
        {
            return new SizeF(p1.X - p2.X, p1.Y - p2.Y);
        }
    }
    #endregion
}