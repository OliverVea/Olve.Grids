using Olve.Grids.IO.Configuration.Models;

namespace Olve.Grids.IO.Configuration.Parsing;

internal interface IParser<TOut>
{
    OneOf<TOut, FileParsingError> Parse(ConfigurationModel configurationModel);
}

internal interface IParser<in TIn, TOut>
{
    OneOf<TOut, FileParsingError> Parse(TIn input);
}