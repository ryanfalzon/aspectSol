using System.ComponentModel;

namespace AspectSol.Lib.Infra.Enums;

public enum VariableVisibility
{
    [Description("public")]
    Public = 1,
    
    [Description("private")]
    Private = 2,
    
    [Description("internal")]
    Internal = 3
}