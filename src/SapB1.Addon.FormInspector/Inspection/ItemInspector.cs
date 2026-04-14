#if SAP_UI_SDK
using SAPbouiCOM;
#endif
#if B1UP_SDK
using SwissAddonFramework.UI;
#endif
using System.Collections.Generic;
using SapB1.Addon.FormInspector.Snapshot.SnapshotModels;

namespace SapB1.Addon.FormInspector.Inspection;

/// <summary>
/// Inspects individual UI items on an SAP Business One form.
/// Extracts item metadata: UID, type, position, visibility, data bindings.
/// Read-only — does not modify the SAP UI.
/// </summary>
public class ItemInspector
{
    /// <summary>
    /// Inspects a single item by its UID on a given form.
    /// </summary>
    public ItemDto InspectItem(int formId, string itemUid)
    {
        // TODO: Use SAPbouiCOM.Form.Items.Item(itemUid) to extract metadata
        // var item = form.Items.Item(itemUid);
        // var specific = item.Specific;
        // return new ItemDto { ... };

        return new ItemDto
        {
            ItemUid = itemUid,
            ItemType = string.Empty,
            Layout = new LayoutDto { Top = 0, Left = 0, Width = 0, Height = 0 },
            Visible = true,
            Enabled = true,
            DataBinding = null,
            FromPane = 0,
            ToPane = 0,
            Description = null
        };
    }

    /// <summary>
    /// Inspects all items on a given form.
    /// </summary>
    public List<ItemDto> InspectAllItems(int formId)
    {
        // TODO: Iterate SAPbouiCOM.Form.Items collection
        // var form = SwissAddonFramework.UI.Forms.GetForm(formId);
        // for (int i = 0; i < form.Items.Count; i++) { ... }

        return new List<ItemDto>();
    }
}
