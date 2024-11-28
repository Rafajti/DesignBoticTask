using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using DesignBoticUI.Views;
using System;
using System.Windows;

namespace DesignBoticLib.Commands;

[Transaction(TransactionMode.Manual)]
public class ApplyColorCommand : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        UIDocument uiDoc = commandData.Application.ActiveUIDocument;
        Document doc = uiDoc.Document;
        View activeView = doc.ActiveView;

        try
        {
            Reference pickedRef = uiDoc.Selection.PickObject(ObjectType.Element, "Select element to color");
            if (pickedRef == null)
            {
                TaskDialog.Show("Information", "Didnt selec any element");
                return Result.Cancelled;
            }

            Element selectedElement = doc.GetElement(pickedRef);

            ColorPickWindow colorPicker = new ColorPickWindow();
            if (colorPicker.ShowDialog() != true)
            {
                TaskDialog.Show("Information", "Cancelled");
                return Result.Cancelled;
            }

            System.Windows.Media.Color wpfColor = colorPicker.SelectedColor;
            Color revitColor = new Color(wpfColor.R, wpfColor.G, wpfColor.B);

            OverrideGraphicSettings overrideSettings = new OverrideGraphicSettings();
            overrideSettings.SetProjectionLineColor(revitColor);
            overrideSettings.SetSurfaceBackgroundPatternColor(revitColor);
            overrideSettings.SetSurfaceBackgroundPatternId(ElementId.InvalidElementId); 

            using (Transaction transaction = new Transaction(doc, "Coloring element"))
            {
                transaction.Start();
                activeView.SetElementOverrides(selectedElement.Id, overrideSettings);
                transaction.Commit();
            }

            TaskDialog.Show("Success", "Element has been coloured");
            return Result.Succeeded;
        }
        catch (OperationCanceledException)
        {
            return Result.Cancelled;
        }
        catch (Exception ex)
        {
            message = $"Exception details: {ex.Message}";
            return Result.Failed;
        }
    }
}
