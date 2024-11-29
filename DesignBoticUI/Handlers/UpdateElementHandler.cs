using Autodesk.Revit.UI;

namespace DesignBoticUI.Handlers;

public class UpdateElementHandler : IExternalEventHandler
{
    private Action<UIApplication> _action;

    public void SetAction(Action<UIApplication> action)
    {
        _action = action;
    }

    public void Execute(UIApplication app)
    {
        _action?.Invoke(app);
    }

    public string GetName() => "Update Element Handler";
}
