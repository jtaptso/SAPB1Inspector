#if SAP_UI_SDK
using SAPbouiCOM;
#endif
#if B1UP_SDK
using SwissAddonFramework.UI;
#endif
using System;
using System.Collections.Generic;
using SapB1.Addon.FormInspector.Snapshot.SnapshotModels;
using SapB1.Addon.FormInspector.Utilities;

namespace SapB1.Addon.FormInspector.Inspection;

/// <summary>
/// Inspects individual UI items on an SAP Business One form.
/// Extracts item metadata: UID, type, position, visibility, data bindings.
/// Read-only — does not modify the SAP UI.
/// </summary>
public class ItemInspector
{
    private readonly MatrixInspector _matrixInspector;
    private readonly ISapContext _sapContext;

    public ItemInspector(MatrixInspector matrixInspector, ISapContext sapContext)
    {
        _matrixInspector = matrixInspector;
        _sapContext = sapContext;
    }

    /// <summary>
    /// Inspects a single item by its UID on a given form.
    /// </summary>
    public ItemDto InspectItem(string formUid, string itemUid)
    {
#if SAP_UI_SDK
        var form = _sapContext.TryGetForm(formUid);
        if (form != null)
        {
            try
            {
                var item = form.Items.Item(itemUid);
                var dto = MapItem(item, formUid);

                // If the item is a matrix or grid, attach matrix metadata
                if (dto.ItemType == "it_MATRIX" || dto.ItemType == "it_GRID")
                {
                    dto.MatrixMetadata = _matrixInspector.InspectMatrix(formUid, itemUid);
                }

                System.Runtime.InteropServices.Marshal.ReleaseComObject(item);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(form);
                return dto;
            }
            catch (Exception)
            {
                // Item may not exist or form is busy — fall through to default
                try { System.Runtime.InteropServices.Marshal.ReleaseComObject(form); } catch { }
            }
        }
#endif
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
    public List<ItemDto> InspectAllItems(string formUid)
    {
#if SAP_UI_SDK
        var form = _sapContext.TryGetForm(formUid);
        if (form != null)
        {
            var items = new List<ItemDto>();
            try
            {
                for (int i = 0; i < form.Items.Count; i++)
                {
                    try
                    {
                        var item = form.Items.Item(i);
                        var dto = MapItem(item, formUid);

                        // Attach matrix metadata for matrix/grid items
                        if (dto.ItemType == "it_MATRIX" || dto.ItemType == "it_GRID")
                        {
                            dto.MatrixMetadata = _matrixInspector.InspectMatrix(formUid, dto.ItemUid);
                        }

                        items.Add(dto);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(item);
                    }
                    catch (Exception)
                    {
                        // Individual item failure should not abort the entire enumeration
                    }
                }
            }
            catch (Exception)
            {
                // Form.Items enumeration failed — return what we have
            }
            finally
            {
                try { System.Runtime.InteropServices.Marshal.ReleaseComObject(form); } catch { }
            }
            return items;
        }
#endif
        return new List<ItemDto>();
    }

#if SAP_UI_SDK
    /// <summary>
    /// Maps an SAPbouiCOM.Item to an ItemDto.
    /// </summary>
    private static ItemDto MapItem(SAPbouiCOM.Item item, string formUid)
    {
        var dto = new ItemDto
        {
            ItemUid = item.ItemUID ?? string.Empty,
            ItemType = item.Type.ToString(),
            Layout = new LayoutDto
            {
                Top = item.Top,
                Left = item.Left,
                Width = item.Width,
                Height = item.Height
            },
            Visible = item.Visible,
            Enabled = item.Enabled,
            FromPane = item.FromPane,
            ToPane = item.ToPane,
            Description = item.Description
        };

        // Extract data binding from specific control types
        dto.DataBinding = ExtractDataBinding(item);

        return dto;
    }

    /// <summary>
    /// Extracts data binding information from an item's Specific object.
    /// Only certain control types (EditText, ComboBox, CheckBox) support data binding.
    /// </summary>
    private static DataBindingDto? ExtractDataBinding(SAPbouiCOM.Item item)
    {
        try
        {
            var specific = item.Specific;
            if (specific == null)
                return null;

            string? tableName = null;
            string? alias = null;

            switch (item.Type)
            {
                case BoFormItemTypes.it_EDIT:
                    var editText = specific as SAPbouiCOM.EditText;
                    if (editText?.DataBind != null)
                    {
                        tableName = editText.DataBind.TableName;
                        alias = editText.DataBind.Alias;
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(editText);
                    }
                    break;

                case BoFormItemTypes.it_COMBO:
                    var comboBox = specific as SAPbouiCOM.ComboBox;
                    if (comboBox?.DataBind != null)
                    {
                        tableName = comboBox.DataBind.TableName;
                        alias = comboBox.DataBind.Alias;
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(comboBox);
                    }
                    break;

                case BoFormItemTypes.it_CHECK_BOX:
                    var checkBox = specific as SAPbouiCOM.CheckBox;
                    if (checkBox?.DataBind != null)
                    {
                        tableName = checkBox.DataBind.TableName;
                        alias = checkBox.DataBind.Alias;
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(checkBox);
                    }
                    break;
            }

            if (!string.IsNullOrEmpty(tableName) && !string.IsNullOrEmpty(alias))
            {
                return new DataBindingDto
                {
                    TableName = tableName,
                    ColumnName = alias
                };
            }
        }
        catch (Exception)
        {
            // DataBind access may fail for some item types — swallow and return null
        }

        return null;
    }
#endif
}
