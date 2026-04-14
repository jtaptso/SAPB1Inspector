#if SAP_UI_SDK
using SAPbouiCOM;
#endif
using System;
using System.Collections.Generic;
using SapB1.Addon.FormInspector.Snapshot.SnapshotModels;
using SapB1.Addon.FormInspector.Utilities;

namespace SapB1.Addon.FormInspector.Inspection;

/// <summary>
/// Inspects data sources bound to SAP Business One forms.
/// Extracts DBDataSource information: table name, aliases, and bound fields.
/// Read-only — does not modify the SAP UI.
/// </summary>
public class DataSourceInspector
{
    private readonly ISapContext _sapContext;

    public DataSourceInspector(ISapContext sapContext)
    {
        _sapContext = sapContext;
    }

    /// <summary>
    /// Inspects all DB data sources on a given form.
    /// </summary>
    public List<DataSourceInfo> InspectDataSources(string formUid)
    {
#if SAP_UI_SDK
        var form = _sapContext.TryGetForm(formUid);
        if (form != null)
        {
            var dataSources = new List<DataSourceInfo>();
            try
            {
                var dbDataSources = form.DataSources.DBDataSources;
                for (int i = 0; i < dbDataSources.Count; i++)
                {
                    try
                    {
                        var ds = dbDataSources.Item(i);
                        var info = new DataSourceInfo
                        {
                            TableName = ds.TableName ?? string.Empty,
                            Aliases = ExtractFieldAliases(ds)
                        };
                        dataSources.Add(info);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(ds);
                    }
                    catch (Exception)
                    {
                        // Individual data source failure — continue
                    }
                }
            }
            catch (Exception)
            {
                // DBDataSources enumeration failed — return what we have
            }
            finally
            {
                try { System.Runtime.InteropServices.Marshal.ReleaseComObject(form); } catch { }
            }
            return dataSources;
        }
#endif
        return new List<DataSourceInfo>();
    }

    /// <summary>
    /// Gets data binding info for a specific item.
    /// </summary>
    public DataBindingDto? GetDataBinding(string formUid, string itemUid)
    {
#if SAP_UI_SDK
        var form = _sapContext.TryGetForm(formUid);
        if (form != null)
        {
            try
            {
                var item = form.Items.Item(itemUid);
                var specific = item.Specific;

                string? tableName = null;
                string? alias = null;

                // Try EditText data binding
                if (specific is SAPbouiCOM.EditText editText && editText.DataBind != null)
                {
                    tableName = editText.DataBind.TableName;
                    alias = editText.DataBind.Alias;
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(editText);
                }
                // Try ComboBox data binding
                else if (specific is SAPbouiCOM.ComboBox comboBox && comboBox.DataBind != null)
                {
                    tableName = comboBox.DataBind.TableName;
                    alias = comboBox.DataBind.Alias;
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(comboBox);
                }
                // Try CheckBox data binding
                else if (specific is SAPbouiCOM.CheckBox checkBox && checkBox.DataBind != null)
                {
                    tableName = checkBox.DataBind.TableName;
                    alias = checkBox.DataBind.Alias;
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(checkBox);
                }

                System.Runtime.InteropServices.Marshal.ReleaseComObject(item);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(form);

                if (!string.IsNullOrEmpty(tableName) && !string.IsNullOrEmpty(alias))
                {
                    return new DataBindingDto
                    {
                        TableName = tableName,
                        ColumnName = alias
                    };
                }

                return null;
            }
            catch (Exception)
            {
                // Item access or data binding extraction failed
                try { System.Runtime.InteropServices.Marshal.ReleaseComObject(form); } catch { }
            }
        }
#endif
        return null;
    }

#if SAP_UI_SDK
    /// <summary>
    /// Extracts field aliases from a DBDataSource's Fields collection.
    /// </summary>
    private static List<string> ExtractFieldAliases(SAPbouiCOM.DBDataSource ds)
    {
        var aliases = new List<string>();
        try
        {
            for (int i = 0; i < ds.Fields.Count; i++)
            {
                try
                {
                    var field = ds.Fields.Item(i);
                    var alias = field.Name ?? field.Alias;
                    if (!string.IsNullOrEmpty(alias))
                        aliases.Add(alias);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(field);
                }
                catch (Exception)
                {
                    // Individual field failure — continue
                }
            }
        }
        catch (Exception)
        {
            // Fields enumeration failed — return what we have
        }
        return aliases;
    }
#endif
}

/// <summary>
/// Information about a DB data source on an SAP form.
/// </summary>
public class DataSourceInfo
{
    public string TableName { get; set; } = string.Empty;
    public List<string> Aliases { get; set; } = new List<string>();
}
