using System.Text.Json;

using MovieDbAssistant.Dmn.Components.Builders.Html;
using MovieDbAssistant.Dmn.Models.Build;
using MovieDbAssistant.Dmn.Models.Scrap.Json;

using static MovieDbAssistant.Dmn.Globals;

namespace MovieDbAssistant.Dmn.Components.Builders.Templates;

/// <summary>
/// The template builder.
/// </summary>
public partial class TemplateBuilder
{
    string IntegratesData(
        string tpl,
        MovieModel data)
    {
        var src = JsonSerializer.Serialize(
            data,
            JsonSerializerProperties.Value)!;

        tpl = SetVar(tpl, Var_Data, src);
        return tpl;
    }

    string IntegratesProps(
        BuildModel build,
        string tpl,
        HtmlDocumentBuilderContext htmlContext,
        MovieModel? data = null)
    {
        var src = JsonSerializer.Serialize(
            GetTemplateProps(
                build,
                data,
                htmlContext),
            JsonSerializerProperties.Value)!;

        tpl = SetVar(tpl, Var_Props, src);
        return tpl;
    }
}
