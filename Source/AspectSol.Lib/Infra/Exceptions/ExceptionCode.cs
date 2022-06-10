namespace AspectSol.Lib.Infra.Exceptions;

public enum ExceptionCode
{
    TokenGenerationFailed = 10001,
    InvalidToken = 20001,
    InvalidPlacementToken = 30001,
    InvalidLocationToken = 30002,
    InvalidVariableAccessToken = 30003,
    InvalidVariableVisibilityToken = 30004,
    InvalidModificationTypeToken = 30005
}