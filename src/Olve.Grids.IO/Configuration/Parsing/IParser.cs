using Olve.Grids.IO.Configuration.Models;

namespace Olve.Grids.IO.Configuration.Parsing;

internal interface IParser<TOut> : IParser<ConfigurationModel, TOut>;

internal interface IParser<in TIn, TOut>
{
    Result<TOut> Parse(TIn input);
}