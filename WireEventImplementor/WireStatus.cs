using Grasshopper.Kernel;

namespace WireEventImplementor
{
    public class WireStatus
    {
        public IGH_Param WireSource { get; }

        public IGH_Param WireTarget { get; }

        public LinkMode? LinkMode { get; }

        public bool? IsDragFromInput { get; }

        public IGH_Param PreviousSideParam => (bool)IsDragFromInput ? WireTarget : WireSource;

        public IGH_Param SubsequentSideParam => (bool)IsDragFromInput ? WireSource : WireTarget;

        private WireStatus()
        {
        }

        internal WireStatus(IGH_Param wireSource, IGH_Param wireTarget, LinkMode? linkMode, bool? isDragFromInput)
        {
            WireSource = wireSource;
            WireTarget = wireTarget;
            LinkMode = linkMode;
            IsDragFromInput = isDragFromInput;
        }
    }
}
