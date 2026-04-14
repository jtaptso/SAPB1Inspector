using System;
using System.Collections.Generic;
using SapB1.Addon.FormInspector.Inspection;

namespace SapB1.Addon.FormInspector.Tests;

public class DataSourceInfoTests
{
    [Fact]
    public void DataSourceInfo_DefaultTableName_IsEmpty()
    {
        // Act
        var info = new DataSourceInfo();

        // Assert
        info.TableName.Should().BeEmpty();
    }

    [Fact]
    public void DataSourceInfo_DefaultAliases_IsEmptyList()
    {
        // Act
        var info = new DataSourceInfo();

        // Assert
        info.Aliases.Should().NotBeNull();
        info.Aliases.Should().BeEmpty();
    }

    [Fact]
    public void DataSourceInfo_CanSetTableName()
    {
        // Act
        var info = new DataSourceInfo { TableName = "ORDR" };

        // Assert
        info.TableName.Should().Be("ORDR");
    }

    [Fact]
    public void DataSourceInfo_CanSetAliases()
    {
        // Act
        var info = new DataSourceInfo { Aliases = new List<string> { "DocNum", "CardCode" } };

        // Assert
        info.Aliases.Should().ContainInOrder("DocNum", "CardCode");
    }
}
