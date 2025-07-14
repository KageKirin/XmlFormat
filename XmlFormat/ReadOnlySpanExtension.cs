using System;
using System.Runtime.CompilerServices;

namespace XmlFormat;

public static class ReadOnlySpanCharExtensions
{
    /// <summary>
    /// Removes all leading and trailing characters that match a specified predicate.
    /// </summary>
    /// <param name="span">The source span from which the characters are removed.</param>
    /// <param name="match">A predicate to evaluate for each character.</param>
    /// <returns>A sub-span that excludes all leading and trailing characters matching the predicate.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<char> Trim(this ReadOnlySpan<char> span, Func<char, bool> match)
    {
        // Assume that in most cases input doesn't need trimming
        if (span.Length == 0 || (!match(span[0]) && !match(span[span.Length - 1])))
        {
            return span;
        }
        return TrimFallback(span, match);

        [MethodImpl(MethodImplOptions.NoInlining)]
        static ReadOnlySpan<char> TrimFallback(ReadOnlySpan<char> span, Func<char, bool> match)
        {
            int start = 0;
            for (; start < span.Length; start++)
            {
                if (!match(span[start]))
                {
                    break;
                }
            }

            int end = span.Length - 1;
            for (; end > start; end--)
            {
                if (!match(span[end]))
                {
                    break;
                }
            }
            return span.Slice(start, end - start + 1);
        }
    }

    /// <summary>
    /// Removes all leading characters that match a specified predicate.
    /// </summary>
    /// <param name="span">The source span from which the characters are removed.</param>
    /// <param name="match">A predicate to evaluate for each character.</param>
    /// <returns>A sub-span that excludes all leading characters matching the predicate.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<char> TrimStart(this ReadOnlySpan<char> span, Func<char, bool> match)
    {
        int start = 0;
        for (; start < span.Length; start++)
        {
            if (!match(span[start]))
            {
                break;
            }
        }

        return span.Slice(start);
    }

    /// <summary>
    /// Removes all trailing characters that match a specified predicate.
    /// </summary>
    /// <param name="span">The source span from which the characters are removed.</param>
    /// <param name="match">A predicate to evaluate for each character.</param>
    /// <returns>A sub-span that excludes all trailing characters matching the predicate.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<char> TrimEnd(this ReadOnlySpan<char> span, Func<char, bool> match)
    {
        int end = span.Length - 1;
        for (; end >= 0; end--)
        {
            if (!match(span[end]))
            {
                break;
            }
        }

        return span.Slice(0, end + 1);
    }

    /// <summary>
    /// Counts the number of characters in the span that match a specified predicate.
    /// </summary>
    /// <param name="span">The span to search.</param>
    /// <param name="match">A predicate to evaluate for each character.</param>
    /// <returns>The number of characters in the span that match the predicate.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Count(this ReadOnlySpan<char> span, Func<char, bool> match)
    {
        int count = 0;
        for (int i = 0; i < span.Length; i++)
        {
            if (match(span[i]))
            {
                count++;
            }
        }
        return count;
    }

    /// <summary>
    /// Counts the number of consecutive matching characters from the start of the span.
    /// </summary>
    /// <param name="span">The span to search.</param>
    /// <param name="match">A predicate to evaluate for each character.</param>
    /// <returns>The number of consecutive leading characters that match the predicate.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int CountStart(this ReadOnlySpan<char> span, Func<char, bool> match)
    {
        int start = 0;
        for (; start < span.Length; start++)
        {
            if (!match(span[start]))
            {
                break;
            }
        }
        return start;
    }

    /// <summary>
    /// Counts the number of consecutive matching characters from the end of the span.
    /// </summary>
    /// <param name="span">The span to search.</param>
    /// <param name="match">A predicate to evaluate for each character.</param>
    /// <returns>The number of consecutive trailing characters that match the predicate.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int CountEnd(this ReadOnlySpan<char> span, Func<char, bool> match)
    {
        int end = span.Length - 1;
        for (; end >= 0; end--)
        {
            if (!match(span[end]))
            {
                break;
            }
        }
        return span.Length - 1 - end;
    }

    /// <summary>
    /// Determines whether any character in the span satisfies a condition.
    /// </summary>
    /// <param name="span">The span to search.</param>
    /// <param name="match">A predicate to evaluate for each character.</param>
    /// <returns><c>true</c> if any character matches the predicate; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Any(this ReadOnlySpan<char> span, Func<char, bool> match)
    {
        for (int i = 0; i < span.Length; i++)
        {
            if (match(span[i]))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Determines whether no character in the span satisfies a condition.
    /// </summary>
    /// <param name="span">The span to search.</param>
    /// <param name="match">A predicate to evaluate for each character.</param>
    /// <returns><c>true</c> if no character matches the predicate; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool None(this ReadOnlySpan<char> span, Func<char, bool> match) => !span.Any(match);

    /// <summary>
    /// Determines whether all characters in the span satisfy a condition.
    /// </summary>
    /// <param name="span">The span to search.</param>
    /// <param name="match">A predicate to evaluate for each character.</param>
    /// <returns><c>true</c> if all characters match the predicate; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool All(this ReadOnlySpan<char> span, Func<char, bool> match)
    {
        for (int i = 0; i < span.Length; i++)
        {
            if (!match(span[i]))
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Finds the zero-based index of the first character in the span that matches the conditions defined by the specified predicate.
    /// </summary>
    /// <param name="span">The read-only character span to search.</param>
    /// <param name="match">The predicate that defines the conditions of the character to search for.</param>
    /// <returns>
    /// The zero-based index of the first occurrence of a character that matches the conditions defined by <paramref name="match"/> within the entire <see cref="ReadOnlySpan{T}"/>, if found; otherwise, –1.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int IndexOf(this ReadOnlySpan<char> span, Func<char, bool> match)
    {
        for (int i = 0; i < span.Length; i++)
        {
            if (match(span[i]))
            {
                return i;
            }
        }
        return -1;
    }

    /// <summary>
    /// Finds the zero-based index of the first character in the span that matches the conditions defined by the specified predicate, starting the search at a specified index.
    /// </summary>
    /// <param name="span">The read-only character span to search.</param>
    /// <param name="match">The predicate that defines the conditions of the character to search for.</param>
    /// <param name="startIndex">The zero-based starting index of the search.</param>
    /// <returns>
    /// The zero-based index of the first occurrence of a character that matches the conditions defined by <paramref name="match"/> within the span that starts at <paramref name="startIndex"/>, if found; otherwise, –1.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int IndexOf(this ReadOnlySpan<char> span, Func<char, bool> match, int startIndex)
    {
        for (int i = startIndex; i < span.Length; i++)
        {
            if (match(span[i]))
            {
                return i;
            }
        }
        return -1;
    }

    /// <summary>
    /// Finds the zero-based index of the first character in a sub-section of the span that matches the conditions defined by the specified predicate.
    /// </summary>
    /// <param name="span">The read-only character span to search.</param>
    /// <param name="match">The predicate that defines the conditions of the character to search for.</param>
    /// <param name="startIndex">The zero-based starting index of the search.</param>
    /// <param name="count">The number of characters in the section to search.</param>
    /// <returns>
    /// The zero-based index of the first occurrence of a character that matches the conditions defined by <paramref name="match"/> within the section of the span that starts at <paramref name="startIndex"/> and contains up to <paramref name="count"/> number of characters, if found; otherwise, –1.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int IndexOf(this ReadOnlySpan<char> span, Func<char, bool> match, int startIndex, int count)
    {
        for (int i = startIndex; i < span.Length; i++)
        {
            if (match(span[i]))
            {
                return i;
            }

            count--;
            if (count == 0)
                break;
        }
        return -1;
    }

    /// <summary>
    /// Creates a tokenizer that enumerates the segments of a span, separated by characters that match the specified predicate.
    /// </summary>
    /// <param name="span">The source span to tokenize.</param>
    /// <param name="separator">The predicate that defines the separator character(s).</param>
    /// <returns>A <see cref="PredicatedReadOnlySpanTokenizer"/> for the specified span.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static PredicatedReadOnlySpanTokenizer Tokenize(this ReadOnlySpan<char> span, Func<char, bool> separator)
    {
        return new(span, separator);
    }

    /// <summary>
    /// Splits a span into segments based on a separator defined by a predicate.
    /// </summary>
    /// <param name="span">The source span to split.</param>
    /// <param name="separator">The predicate that defines the separator character(s).</param>
    /// <returns>A <see cref="PredicatedReadOnlySpanTokenizer"/> that can be used to iterate over the segments.</returns>
    /// <remarks>This method is an alias for the <see cref="Tokenize"/> method.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static PredicatedReadOnlySpanTokenizer Split(this ReadOnlySpan<char> span, Func<char, bool> separator) =>
        span.Tokenize(separator);
}
