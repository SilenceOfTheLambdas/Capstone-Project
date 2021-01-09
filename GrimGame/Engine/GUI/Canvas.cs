using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace GrimGame.Engine.GUI
{
    public class Canvas
    {
        public Color BackgroundColor;
        public int   LayerIndex;

        /// <summary>
        ///     A list of any panels that are attached to this canvas.
        /// </summary>
        public List<Panel> Panels;

        public Canvas()
        {
            Panels = new List<Panel>();
        }

        public void AddPanel(Panel panel)
        {
            Panels.Add(panel);
        }

        public void RemovePanel(Panel panel)
        {
            Panels.Remove(panel);
        }

        public void Draw()
        {
            foreach (var panel in Panels)
                // draw every panel in this canvas
                panel.Draw();
        }

        public void Update()
        {
            foreach (var panel in Panels)
                // draw every panel in this canvas
                panel.Update();
        }
    }
}