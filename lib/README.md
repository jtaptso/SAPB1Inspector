# SAP B1 SDK & SwissAddonFramework Dependencies

This folder must contain the following DLLs before building the SAP add-on project
with full COM interop support. **The project will compile without these DLLs** —
the references are conditional — but the SAP API types will not be available.

## Required DLLs

| DLL | Source | Description |
|-----|--------|-------------|
| `Interop.SAPbouiCOM.dll` | SAP B1 UI API SDK | Primary interop assembly for SAP Business One UI API. Generated when you add a COM reference to SAPbouiCOM in Visual Studio, or via `tlbimp.exe SAPbouiCOM.dll`. |
| `SAPbouiCOM.dll` | SAP B1 UI API SDK | The native COM type library (optional if using the interop assembly above). |
| `SwissAddonFramework.dll` | B1 Usability Package (Boyum IT) | .NET assembly provided by the B1UP add-on framework. |

## Where to Find the DLLs

### SAPbouiCOM

1. Install the **SAP Business One SDK** (part of the SAP B1 client installation).
2. The COM component is registered automatically during SDK install.
3. Generate the interop assembly:
   ```
   tlbimp.exe "C:\Program Files (x86)\SAP\SAP Business One\SAPbouiCOM.dll" /out:Interop.SAPbouiCOM.dll
   ```
4. Copy `Interop.SAPbouiCOM.dll` into this `lib/` folder.

   Alternatively, if you already have a Visual Studio project with a COM reference
   to SAPbouiCOM, the interop DLL is generated automatically in the `obj/` folder.

### SwissAddonFramework

1. Install **B1 Usability Package (B1UP)** from Boyum IT.
2. Locate `SwissAddonFramework.dll` in the B1UP installation folder, e.g.:
   - `C:\Program Files\Boyum IT\B1UP Server Component\`
   - Or the B1UP client add-on installation folder
3. Copy `SwissAddonFramework.dll` into this `lib/` folder.

## Important Notes

- **Embed Interop Types** must be set to **False** for `Interop.SAPbouiCOM.dll`.
  This is already configured in the project file.
- **Platform Target**: The SAP B1 client is typically 32-bit (x86). If building
  on a machine with the SDK installed, ensure the project targets `x86` or
  `AnyCPU` with prefer 32-bit.
- These DLLs are **proprietary** and should **not** be committed to source control.
  The `.gitignore` excludes `lib/*.dll`.

## Conditional Compilation Symbols

When the DLLs are present, the following compilation symbols are defined automatically:

| Symbol | Condition | Enables access to |
|--------|-----------|-------------------|
| `SAP_UI_SDK` | `Interop.SAPbouiCOM.dll` exists in `lib/` | `SAPbouiCOM` namespace (SAP UI API types) |
| `B1UP_SDK` | `SwissAddonFramework.dll` exists in `lib/` | `SwissAddonFramework.*` namespaces (B1UP framework types) |

> **Note:** The `SwissAddonFramework` namespace paths used in the source code
> (e.g., `SwissAddonFramework.Hosting`, `SwissAddonFramework.UI`,
> `SwissAddonFramework.UI.EventHandlers`, `SwissAddonFramework.Application`)
> are **provisional** and based on documentation/common usage patterns.
> They may need adjustment once the actual DLL is available for reference.
> Verify the namespaces with IntelliSense or Object Browser when the DLL is added.
