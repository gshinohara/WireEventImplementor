using Grasshopper.GUI.Canvas.Interaction;
using Grasshopper.Kernel;
using System;
using System.Reflection;

namespace WireEventImplementor
{
    public enum LinkMode
    {
        Replace,
        Add,
        Remove
    }

    public static class WireUtil
    {
        internal static void GetWireParameters(this GH_WireInteraction self, out WireStatus wireStatus)
        {
            Func<string, object> getPrivateParam = (name) => typeof(GH_WireInteraction).GetField(name, BindingFlags.NonPublic | BindingFlags.Instance).GetValue(self);

            IGH_Param source = getPrivateParam("m_source") as IGH_Param;
            IGH_Param target = getPrivateParam("m_target") as IGH_Param;
            LinkMode mode = (LinkMode)Enum.Parse(typeof(LinkMode), getPrivateParam("m_mode").ToString());
            bool dragFromInput = (bool)getPrivateParam("m_dragfrominput");

            wireStatus = new WireStatus(source, target, mode, dragFromInput);
        }
    }
}
