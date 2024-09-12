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
        internal static void GetWireParameters(this GH_WireInteraction interaction, out IGH_Param source, out IGH_Param target, out LinkMode mode, out bool dragFromInput)
        {
            Func<string, object> getPrivateParam = (name) => typeof(GH_WireInteraction).GetField(name, BindingFlags.NonPublic | BindingFlags.Instance).GetValue(interaction);

            source = getPrivateParam("m_source") as IGH_Param;
            target = getPrivateParam("m_target") as IGH_Param;
            mode = (LinkMode)Enum.Parse(typeof(LinkMode), getPrivateParam("m_mode").ToString());
            dragFromInput = (bool)getPrivateParam("m_dragfrominput");
        }
    }
}
