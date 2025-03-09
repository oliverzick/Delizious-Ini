namespace Delizious.Ini.Test;

using System.Collections.Generic;
using System.IO;

[TestClass]
public sealed class IniDocumentCaseSensitivitySpec
{
    private static string Ini => """
                                 [Section]
                                 Property=Value
                                 """;

    private static PropertyValue Value => "Value";
    private static PropertyValue FallbackValue => PropertyValue.None;

    [DataTestMethod]
    [DynamicData(nameof(Reads_the_property_with_configured_case_sensitivity_test_cases), DynamicDataSourceType.Method)]
    public void Reads_the_property_with_configured_case_sensitivity(CaseSensitivity caseSensitivity, SectionName sectionName, PropertyKey propertyKey, PropertyValue expected)
    {
        var configuration = IniDocumentConfiguration.Default.WithCaseSensitivity(caseSensitivity);

        var target = MakeTarget(configuration);

        var actual = target.ReadProperty(sectionName, propertyKey);

        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> Reads_the_property_with_configured_case_sensitivity_test_cases()
    {
        yield return [CaseSensitivity.CaseInsensitive, SectionName.Create("Section"), PropertyKey.Create("Property"), Value];
        yield return [CaseSensitivity.CaseInsensitive, SectionName.Create("section"), PropertyKey.Create("property"), Value];
        yield return [CaseSensitivity.CaseInsensitive, SectionName.Create("SECTION"), PropertyKey.Create("PROPERTY"), Value];
        yield return [CaseSensitivity.CaseInsensitive, SectionName.Create("SECTioN"), PropertyKey.Create("ProPERTy"), Value];

        yield return [CaseSensitivity.CaseSensitive, SectionName.Create("Section"), PropertyKey.Create("Property"), Value];
        yield return [CaseSensitivity.CaseSensitive, SectionName.Create("section"), PropertyKey.Create("property"), FallbackValue];
        yield return [CaseSensitivity.CaseSensitive, SectionName.Create("SECTION"), PropertyKey.Create("PROPERTY"), FallbackValue];
        yield return [CaseSensitivity.CaseSensitive, SectionName.Create("SECTioN"), PropertyKey.Create("ProPERTy"), FallbackValue];
    }

    // ToDo: Cover every operation?

    private static IniDocument MakeTarget(IniDocumentConfiguration configuration)
    {
        using var stringReader = new StringReader(Ini);
        return IniDocument.LoadFrom(stringReader, configuration);
    }
}
