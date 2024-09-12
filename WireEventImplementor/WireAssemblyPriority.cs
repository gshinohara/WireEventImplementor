using Grasshopper;
using Grasshopper.GUI.Canvas;
using Grasshopper.GUI.Canvas.Interaction;
using Grasshopper.Kernel;
using System.Windows.Forms;

namespace WireEventImplementor
{
    public delegate void PreWiredEventHandler();

    public abstract class WireAssemblyPriority : GH_AssemblyPriority
    {
        public event PreWiredEventHandler PreWired;

        public IGH_Param WireSource { get; private set; }

        public IGH_Param WireTarget { get; private set; }

        public LinkMode? LinkMode { get; private set; }

        public bool? IsDragFromInput { get; private set; }

        public override GH_LoadingInstruction PriorityLoad()
        {
            Instances.CanvasCreated += canvas =>
            {
                canvas.MouseMove += CheckWireInteraction;
                canvas.MouseUp += ResetWireParams;
            };

            return GH_LoadingInstruction.Proceed;
        }

        private void CheckWireInteraction(object sender, MouseEventArgs e)
        {
            if ((sender as GH_Canvas).ActiveInteraction is GH_WireInteraction wireInteraction)
            {
                wireInteraction.GetWireParameters(out IGH_Param source, out IGH_Param target, out LinkMode mode, out bool dragFromInput);

                if (target != null)
                {
                    WireSource = source;
                    WireTarget = target;
                    LinkMode = mode;
                    IsDragFromInput = dragFromInput;

                    if (e.Button == MouseButtons.Left)
                        PreWired?.Invoke();
                }
                else
                {
                    WireSource = null;
                    WireTarget = null;
                    LinkMode = LinkMode = null;
                    IsDragFromInput = null;
                }
            }
        }

        private void ResetWireParams(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                WireSource = null;
                WireTarget = null;
                LinkMode = LinkMode = null;
                IsDragFromInput = null;

                (sender as GH_Canvas).Refresh();
            }
        }
    }
}
