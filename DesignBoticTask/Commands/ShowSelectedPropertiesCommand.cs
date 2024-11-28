﻿using System.Reflection.Metadata;
using System.Xml.Linq;

namespace DesignBoticTask.Commands;

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
            TaskDialog.Show("Informacja", "Nie zaznaczono żadnych elementów.");
            return Result.Succeeded;
        }

        var properties = selectedElementIds
            .Select(id => doc.GetElement(id))
            .Select(element => GetElementProperties(element))
            .ToList();

        PropertiesWindow window = new PropertiesWindow(properties);
        window.ShowDialog();
        return Result.Succeeded;
    }

    private string GetElementProperties(Element element)
    {
        string name = element.Name;
        var parameters = element.Parameters.Cast<Parameter>()
            .Select(p => $"{p.Definition.Name}: {p.AsValueString() ?? p.AsString()}")
            .ToList();

        return $"{name}\n{string.Join("\n", parameters)}";
    }
}
