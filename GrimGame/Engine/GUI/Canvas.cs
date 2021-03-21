using System.Collections.Generic;

namespace GrimGame.Engine.GUI
{
    /// <summary>
    ///     A UI Canvas holds panels and renders them onto the screen.
    ///     <see cref="Panel" />
    /// </summary>
    public class Canvas
    {
        /// <summary>
        ///     A list of any panels that are attached to this canvas.
        /// </summary>
        private readonly List<Panel> _panels;

        /// <summary>
        ///     Create a new canvas and initialise <see cref="_panels" />
        /// </summary>
        public Canvas()
        {
            _panels = new List<Panel>();
        }

        /// <summary>
        ///     Add a new panel to this canvas.
        /// </summary>
        /// <param name="panel">A panel to add</param>
        public void AddPanel(Panel panel)
        {
            lock (_panels)
            {
                _panels.Add(panel);
            }
        }

        /// <summary>
        ///     Remove a panel from this canvas.
        /// </summary>
        /// <param name="panel">Panel to remove</param>
        public void RemovePanel(Panel panel)
        {
            lock (_panels)
            {
                _panels.Remove(panel);
            }
        }

        /// <summary>
        ///     Draws all of the panels in the <see cref="_panels" /> list.
        /// </summary>
        public void Draw()
        {
            lock (_panels)
            {
                foreach (var panel in _panels)
                    // draw every panel in this canvas
                    panel.Draw();
            }
        }

        /// <summary>
        ///     Update all of the <see cref="_panels" />.
        /// </summary>
        public void Update()
        {
            lock (_panels)
            {
                foreach (var panel in _panels)
                    // draw every panel in this canvas
                    panel.Update();
            }
        }
    }
}