using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using DesignBoticUI.Views;

namespace DesignBoticLib.Commands;

[Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
public class ShowSelectedPropertiesCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        UIDocument uiDoc = commandData.Application.ActiveUIDocument;
        Document doc = uiDoc.Document;

        var selectedElementIds = uiDoc.Selection.GetElementIds();
        if (selectedElementIds.Count == 0)
        {
            TaskDialog.Show("Information", "Didnt select any element");
            return Result.Succeeded;
        }

        var properties = selectedElementIds
            .Select(id => doc.GetElement(id))
            .Select(element => GetElementProperties(element))
            .ToList();

        PropertiesWindow window = new(properties);
        window.ShowDialog();
        return Result.Succeeded;
    }

    private static string GetElementProperties(Element element)
    {
        string name = element.Name;
        var parameters = element.Parameters.Cast<Parameter>()
            .Select(p => $"{p.Definition.Name}: {p.AsValueString() ?? p.AsString()}")
            .ToList();

        return $"{name}\n{string.Join("\n", parameters)}";
    }
}
