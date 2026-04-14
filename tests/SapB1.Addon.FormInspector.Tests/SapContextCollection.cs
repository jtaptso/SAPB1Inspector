using Xunit;

namespace SapB1.Addon.FormInspector.Tests;

/// <summary>
/// xUnit collection definition that serializes all tests touching the static SapContext.
/// Prevents parallel races when one test calls SapContext.Initialize/Reset
/// while another relies on SapContext.IsInitialized.
/// </summary>
[CollectionDefinition("SapContext", DisableParallelization = true)]
public class SapContextCollection
{
    // This class is never instantiated — it's just a marker for xUnit.
}
