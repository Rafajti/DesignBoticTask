using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using DesignBoticUI.Models;
using DesignBoticUI.Views;

namespace DesignBoticLib.Commands;

[Transaction(TransactionMode.Manual)]
public class ShowSelectedPropertiesCommand : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        try
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;

            var selectedElementIds = uiDoc.Selection.GetElementIds();
            if (selectedElementIds.Count == 0)
            {
                TaskDialog.Show("Information", "Didnt select any element");
                return Result.Succeeded;
            }

            List<ElementProperties> elementProperties = selectedElementIds
               .Select(id => doc.GetElement(id))
               .Select(element => new ElementProperties
               (
                   Name: element.Name,
                   Id: element.Id.Value,
                   Parameters: element.Parameters
                       .Cast<Parameter>()
                       .Select(p => new KeyValuePair<string, string>(
                           p.Definition.Name,
                           p.AsValueString() ?? p.AsString() ?? "N/A"))
                       .ToList()
               )).ToList();


            PropertiesWindow propertiesWindow = new(elementProperties);
            propertiesWindow.ShowDialog();

            return Result.Succeeded;
        }
        catch (Exception ex)
        {
            TaskDialog.Show("Test", ex.Message);
            message = ex.Message;
            return Result.Failed;
        }

    }
}
