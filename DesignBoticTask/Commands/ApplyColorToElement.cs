using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using DesignBoticUI.Views;
using System;
using System.Windows;

namespace DesignBoticTask.Commands;

[Transaction(TransactionMode.Manual)]
public class ApplyColorWithWpfCommand : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        UIDocument uiDoc = commandData.Application.ActiveUIDocument;
        Autodesk.Revit.DB.Document doc = uiDoc.Document;
        View activeView = doc.ActiveView;

        try
        {
            Reference pickedRef = uiDoc.Selection.PickObject(ObjectType.Element, "Wybierz element do pokolorowania");
            if (pickedRef == null)
            {
                TaskDialog.Show("Informacja", "Nie wybrano żadnego elementu.");
                return Result.Cancelled;
            }

            Element selectedElement = doc.GetElement(pickedRef);

            ColorPickWindow colorPicker = new ColorPickWindow();
            if (colorPicker.ShowDialog() != true)
            {
                TaskDialog.Show("Informacja", "Anulowano wybór koloru.");
                return Result.Cancelled;
            }

            // Pobierz wybrany kolor
            System.Windows.Media.Color wpfColor = colorPicker.SelectedColor;
            Autodesk.Revit.DB.Color revitColor = new Autodesk.Revit.DB.Color(wpfColor.R, wpfColor.G, wpfColor.B);

            // Ustawienia graficzne
            OverrideGraphicSettings overrideSettings = new OverrideGraphicSettings();
            overrideSettings.SetProjectionLineColor(revitColor);
            overrideSettings.SetSurfaceBackgroundPatternColor(revitColor);
            overrideSettings.SetSurfaceBackgroundPatternId(ElementId.InvalidElementId); // Brak wzoru

            // Transakcja do nadania koloru
            using (Transaction transaction = new Transaction(doc, "Nadanie koloru elementowi"))
            {
                transaction.Start();
                activeView.SetElementOverrides(selectedElement.Id, overrideSettings);
                transaction.Commit();
            }

            TaskDialog.Show("Sukces", "Kolor został nadany elementowi.");
            return Result.Succeeded;
        }
        catch (OperationCanceledException)
        {
            // Obsługa anulowania wyboru przez użytkownika
            return Result.Cancelled;
        }
        catch (Exception ex)
        {
            message = $"Wystąpił błąd: {ex.Message}";
            return Result.Failed;
        }
    }
}
