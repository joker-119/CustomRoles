namespace CustomRoles.Configs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using YamlDotNet.Core;
    using YamlDotNet.Core.Events;
    using YamlDotNet.Serialization;

    public class AbstractNodeNodeTypeResolverWithValidation : INodeDeserializer
    {
        private readonly INodeDeserializer _original;
        private readonly ITypeDiscriminator[] _typeDiscriminators;

        public AbstractNodeNodeTypeResolverWithValidation(INodeDeserializer original,
            params ITypeDiscriminator[] discriminators)
        {
            _original = original;
            _typeDiscriminators = discriminators;
        }

        public bool Deserialize(IParser reader, Type expectedType, Func<IParser, Type, object> nestedObjectDeserializer,
            out object value)
        {
            if (!reader.Accept<MappingStart>(out MappingStart mapping))
            {
                value = null;
                return false;
            }

            List<ITypeDiscriminator> supportedTypes = _typeDiscriminators.Where(t => t.BaseType == expectedType).ToList();
            if (!supportedTypes.Any())
                return _original.Deserialize(reader, expectedType, nestedObjectDeserializer, out value);

            Mark start = reader.Current.Start;
            Type actualType;
            ParsingEventBuffer buffer;
            try
            {
                buffer = new ParsingEventBuffer(ReadNestedMapping(reader));
                actualType = CheckWithDiscriminators(expectedType, supportedTypes, buffer);
            }
            catch (Exception exception)
            {
                throw new YamlException(start, reader.Current.End, "Failed when resolving abstract type", exception);
            }

            buffer.Reset();
            return _original.Deserialize(buffer, actualType, nestedObjectDeserializer, out value);
        }

        private static Type CheckWithDiscriminators(Type expectedType, IEnumerable<ITypeDiscriminator> supportedTypes,
            ParsingEventBuffer buffer)
        {
            foreach (ITypeDiscriminator discriminator in supportedTypes)
            {
                buffer.Reset();
                if (!discriminator.TryResolve(buffer, out Type actualType)) continue;
                return actualType;
            }

            throw new Exception(
                $"None of the registered type discriminators could supply a child class for {expectedType}");
        }

        private static LinkedList<ParsingEvent> ReadNestedMapping(IParser reader)
        {
            LinkedList<ParsingEvent> result = new LinkedList<ParsingEvent>();
            result.AddLast(reader.Consume<MappingStart>());
            int depth = 0;
            do
            {
                ParsingEvent next = reader.Consume<ParsingEvent>();
                depth += next.NestingIncrease;
                result.AddLast(next);
            } while (depth >= 0);

            return result;
        }
    }
}