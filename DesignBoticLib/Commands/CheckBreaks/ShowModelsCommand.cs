using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Windows.Controls;
using System.Linq;
using DesignBoticUI.Models;
using DesignBoticUI.Views;

namespace DesignBoticLib.Commands.CheckBreaks;

[Transaction(TransactionMode.Manual)]
public class ShowModelsCommand : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        try
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;

            FilteredElementCollector collector = new(doc);

            List<Element> allElements = new List<Element>();

            var walls = collector.OfCategory(BuiltInCategory.OST_Walls).WhereElementIsNotElementType().ToList();
            var floors = collector.OfCategory(BuiltInCategory.OST_Floors).WhereElementIsNotElementType().ToList();
            var windows = collector.OfCategory(BuiltInCategory.OST_Windows).WhereElementIsNotElementType().ToList();
            var doors = collector.OfCategory(BuiltInCategory.OST_Doors).WhereElementIsNotElementType().ToList();

            allElements.AddRange(walls);
            allElements.AddRange(floors);
            allElements.AddRange(windows);
            allElements.AddRange(doors);


            var listOfElement = allElements.Select(x => Map(x)).ToList();
            ShowModelWindow window = new ShowModelWindow(listOfElement);
            window.ShowDialog();

            return Result.Succeeded;
        }
        catch (Exception ex)
        {
            message = ex.Message;
            return Result.Failed;
        }
    }

    private ElementInformationDto Map(Element element)
    {
        return new ElementInformationDto
        {
            Category = element.Category.Name,
            Name = element.Name,
            Id = element.Id.Value,
            Parameters = element.Parameters.Cast<Parameter>()
                                .Select(p => new KeyValuePair<string, string>(
                                    p.Definition.Name,
                                    p.AsValueString() ?? p.AsString() ?? "N/A"))
                                .ToList()
        };
    }
}
