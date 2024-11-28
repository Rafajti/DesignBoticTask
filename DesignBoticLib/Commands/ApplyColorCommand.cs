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

        try
        {
            var selectedElementIds = uiDoc.Selection.GetElementIds();
            var selectedElement = doc.GetElement(selectedElementIds.First());
            ColorPickWindow colorPicker = new ColorPickWindow();
            if (colorPicker.ShowDialog() != true)
            {
                TaskDialog.Show("Information", "Cancelled");
                return Result.Cancelled;
            }

            System.Windows.Media.Color wpfColor = colorPicker.SelectedColor;
            Color revitColor = new Color(wpfColor.R, wpfColor.G, wpfColor.B);

            using (Transaction trans = new Transaction(doc, "Change Element Color"))
            {
                trans.Start();

                OverrideGraphicSettings ogs = new OverrideGraphicSettings();
                ogs.SetProjectionLineColor(revitColor);

                //doc.ActiveView.SetElementOverrides(selectedElement.Id, ogs);

                trans.Commit();
            }


            TaskDialog.Show("Success", "Element has been coloured");
            return Result.Succeeded;
        }
        catch (OperationCanceledException e)
        {
            TaskDialog.Show("Error", e.Message);
            return Result.Failed;
        }
        catch (Exception ex)
        {
            TaskDialog.Show("Error", ex.Message);
            return Result.Failed;
        }
    }
}
