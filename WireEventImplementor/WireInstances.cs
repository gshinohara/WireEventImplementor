using Grasshopper.GUI.Canvas;
using Grasshopper.GUI.Canvas.Interaction;
using Grasshopper.Kernel;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WireEventImplementor
{
    public static class WireInstances
    {

        public delegate void PreWiredEventHandler(WireStatus wireStatus);

        public delegate void WiringEventHandler(WireStatus wireStatus);

        public delegate void PostWiredEventHandler(WireStatus wireStatus);

        /// <summary>
        /// Invoked when your wiring cursor is floating.
        /// </summary>
        public static event WiringEventHandler Wiring;

        /// <summary>
        /// Invoked when your wiring cursor is on the grip.
        /// </summary>
        public static event PreWiredEventHandler PreWired;

        /// <summary>
        /// Invoked when your wire connects grips.
        /// </summary>
        public static event PostWiredEventHandler PostWired;

        private static TaskCompletionSource<WireStatus> m_source = new TaskCompletionSource<WireStatus>();

        /// <summary>
        /// Call when canvas created.
        /// </summary>
        /// <param name="canvas">Applied canvas.</param>
        public static void SetUp(GH_Canvas canvas)
        {
            canvas.MouseMove += Canvas_MouseMove;
            canvas.MouseUp += Canvas_MouseUp;
        }

        private static void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (sender is GH_Canvas canvas && canvas.ActiveInteraction is GH_WireInteraction wireInteraction)
            {
                wireInteraction.GetWireParameters(out WireStatus status);

                if (e.Button == MouseButtons.Left)
                {
                    m_source = new TaskCompletionSource<WireStatus>();

                    if (status.WireTarget == null)
                        Wiring?.Invoke(status);
                    else
                    {
                        PreWired?.Invoke(status);
                        m_source.SetResult(status);
                    }
                }

                canvas.Refresh();
            }
        }

        private static async void Canvas_MouseUp(object sender, MouseEventArgs e)
        {
            WireStatus status = await m_source.Task;
            m_source = new TaskCompletionSource<WireStatus>();

            PostWired?.Invoke(status);

            (sender as GH_Canvas).Refresh();
        }
    }
}
