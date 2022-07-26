using AspectSol.Lib.Domain.Filtering;
using AspectSol.Lib.Infra.Extensions;

namespace AspectSol.Lib.Infra.Helpers;

public static class SelectionResultHelpers
{
    public static SelectionResult Intersect(params SelectionResult[] selectionResults)
    {
        if (selectionResults.Length == 0) return new SelectionResult();
        
        selectionResults = selectionResults.Where(x => x != null).ToArray();

        var interestedContracts = selectionResults[0].InterestedContracts;
        var interestedFunctions = selectionResults[0].InterestedFunctions;
        var interestedDefinitions = selectionResults[0].InterestedDefinitions;
        var interestedStatements = selectionResults[0].InterestedStatements;

        for (var i = 1; i < selectionResults.Length; i++)
        {
            interestedContracts   = interestedContracts.SafetIntersect(selectionResults[i].InterestedContracts);
            interestedFunctions   = interestedFunctions.SafetIntersect(selectionResults[i].InterestedFunctions);
            interestedDefinitions = interestedDefinitions.SafetIntersect(selectionResults[i].InterestedDefinitions);
            interestedStatements  = interestedStatements.SafetIntersect(selectionResults[i].InterestedStatements);
        }

        return new SelectionResult
        {
            InterestedContracts   = interestedContracts,
            InterestedFunctions   = interestedFunctions,
            InterestedDefinitions = interestedDefinitions,
            InterestedStatements  = interestedStatements
        };
    }
}