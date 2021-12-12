namespace CustomRoles.Configs
{
    using System;
    using YamlDotNet.Core;
    using YamlDotNet.Core.Events;

    public static class ParserExtension
    {
        public static bool TryFindMappingEntry(this ParsingEventBuffer parser, Func<Scalar, bool> selector,
            out Scalar key, out ParsingEvent value)
        {
            parser.Consume<MappingStart>();
            do
            {
                switch (parser.Current)
                {
                    case Scalar scalar:
                        var keyMatched = selector(scalar);
                        parser.MoveNext();
                        if (keyMatched)
                        {
                            value = parser.Current;
                            key = scalar;
                            return true;
                        }

                        parser.SkipThisAndNestedEvents();
                        break;
                    case MappingStart _:
                    case  SequenceStart _:
                        parser.SkipThisAndNestedEvents();
                        break;
                    default:
                        parser.MoveNext();
                        break;
                }
            } while (parser.Current != null);

            key = null;
            value = null;
            return false;
        }
    }
}