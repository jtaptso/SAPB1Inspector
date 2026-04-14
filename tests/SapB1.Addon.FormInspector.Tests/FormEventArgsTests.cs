using System;
using SapB1.Addon.FormInspector.Events;

namespace SapB1.Addon.FormInspector.Tests;

public class FormEventArgsTests
{
    [Fact]
    public void FormEventArgs_DefaultFormType_IsEmpty()
    {
        // Act
        var args = new FormEventArgs();

        // Assert
        args.FormType.Should().BeEmpty();
    }

    [Fact]
    public void FormEventArgs_DefaultFormUid_IsEmpty()
    {
        // Act
        var args = new FormEventArgs();

        // Assert
        args.FormUid.Should().BeEmpty();
    }

    [Fact]
    public void FormEventArgs_FormType_CanBeSet()
    {
        // Act
        var args = new FormEventArgs { FormType = "139" };

        // Assert
        args.FormType.Should().Be("139");
    }

    [Fact]
    public void FormEventArgs_FormUid_CanBeSet()
    {
        // Act
        var args = new FormEventArgs { FormUid = "form-1" };

        // Assert
        args.FormUid.Should().Be("form-1");
    }

    [Fact]
    public void FormEventArgs_InheritsFromEventArgs()
    {
        // Act
        var args = new FormEventArgs();

        // Assert
        args.Should().BeAssignableTo<EventArgs>();
    }
}
