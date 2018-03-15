using System;
using System.Globalization;

namespace CamelSnakeKebab
{
    public delegate int StringTransformation(ReadOnlySpan<char> source, Span<char> destination);

    public static class StringTransformations
    {
        public static StringTransformation CamelCase { get; } = TransformCamelCase;

        public static StringTransformation KebabCase { get; }

        public static StringTransformation SnakeCase { get; }

        public static StringTransformation PascalCase { get; }

        public static StringTransformation TitleCase { get; }

        private static int TransformCamelCase(ReadOnlySpan<char> source, Span<char> destination)
        {
            var written = 0;
            var isAcronym = true;
            var isNewWord = false;

            for (var index = 0; index < source.Length; index++)
            {
                var character = source[index];

                if (IsSeparator(character))
                {
                    isAcronym = true;
                    isNewWord = written > 0;
                    continue;
                }

                if (index > 0 && isNewWord)
                {
                    destination[written++] = char.ToUpper(character, CultureInfo.InvariantCulture);
                    isNewWord = false;
                    continue;
                }

                var nextIndex = index + 1;
                var hasNext = nextIndex < source.Length;

                if (hasNext && char.IsLower(source[nextIndex]))
                {
                    isAcronym = false;
                }

                if (isAcronym || written == 0)
                {
                    destination[written++] = char.ToLower(character, CultureInfo.InvariantCulture);
                    continue;
                }

                isAcronym = char.IsDigit(character);

                destination[written++] = character;
            }

            return written;
        }

        private static bool IsSeparator(char value)
        {
            return char.IsSeparator(value) || value == '_' || value == '-';
        }
    }

    public static class StringExtensions
    {
        public static string ToCamelCase(this string value)
        {
            return value.Transform(StringTransformations.CamelCase);
        }

        public static string ToKebabCase(this string value)
        {
            return value.Transform(StringTransformations.KebabCase);
        }

        public static string ToSnakeCase(this string value)
        {
            return value.Transform(StringTransformations.SnakeCase);
        }

        public static string ToPascalCase(this string value)
        {
            return value.Transform(StringTransformations.PascalCase);
        }

        public static string ToTitleCase(this string value)
        {
            return value.Transform(StringTransformations.TitleCase);
        }

        public static unsafe string Transform(this string value, StringTransformation transformation)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (value.Length == 0)
            {
                return string.Empty;
            }

            var source = value.AsSpan();

            var length = source.Length;

            char* buffer = stackalloc char[length];

            var destination = new Span<char>(buffer, length);

            var count = source.Transform(destination, transformation);

            return count == 0 ? string.Empty : new string(buffer, 0, count);
        }
    }

    public static class ReadOnlySpanExtensions
    {
        public static int ToCamelCase(this ReadOnlySpan<char> source, Span<char> destination)
        {
            return source.Transform(destination, StringTransformations.CamelCase);
        }

        public static int ToKebabCase(this ReadOnlySpan<char> source, Span<char> destination)
        {
            return source.Transform(destination, StringTransformations.KebabCase);
        }

        public static int ToSnakeCase(this ReadOnlySpan<char> source, Span<char> destination)
        {
            return source.Transform(destination, StringTransformations.SnakeCase);
        }

        public static int ToPascalCase(this ReadOnlySpan<char> source, Span<char> destination)
        {
            return source.Transform(destination, StringTransformations.PascalCase);
        }

        public static int ToTitleCase(this ReadOnlySpan<char> source, Span<char> destination)
        {
            return source.Transform(destination, StringTransformations.TitleCase);
        }

        public static int Transform(this ReadOnlySpan<char> source, Span<char> destination, StringTransformation transformation)
        {
            if (source.IsEmpty)
            {
                return 0;
            }

            if (destination.Length < source.Length)
            {
                return -1;
            }

            return transformation(source, destination);
        }
    }
}