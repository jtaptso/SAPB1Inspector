#if SAP_UI_SDK
using SAPbouiCOM;
#endif
using System.Collections.Generic;
using SapB1.Addon.FormInspector.Snapshot.SnapshotModels;

namespace SapB1.Addon.FormInspector.Inspection;

/// <summary>
/// Inspects data sources bound to SAP Business One forms.
/// Extracts DBDataSource information: table name, aliases, and bound fields.
/// Read-only — does not modify the SAP UI.
/// </summary>
public class DataSourceInspector
{
    /// <summary>
    /// Inspects all DB data sources on a given form.
    /// </summary>
    public List<DataSourceInfo> InspectDataSources(int formId)
    {
        // TODO: Use SAPbouiCOM.Form.DataSources.DBDataSources
        // to extract table names and column bindings
        return new List<DataSourceInfo>();
    }

    /// <summary>
    /// Gets data binding info for a specific item.
    /// </summary>
    public DataBindingDto? GetDataBinding(int formId, string itemUid)
    {
        // TODO: Use SAPbouiCOM.EditText.DataBind or equivalent
        // to extract TableName and ColumnName
        return null;
    }
}

/// <summary>
/// Information about a DB data source on an SAP form.
/// </summary>
public class DataSourceInfo
{
    public string TableName { get; set; } = string.Empty;
    public List<string> Aliases { get; set; } = new List<string>();
}
