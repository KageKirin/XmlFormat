using System;
using System.Runtime.CompilerServices;

namespace XmlFormat;

public static class ReadOnlySpanCharExtensions
{
    /// <summary>
    /// Removes all leading and trailing characters matching the `match` delegate from the span.
    /// </summary>
    /// <param name="span">The source span from which the characters are removed.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<char> Trim(this ReadOnlySpan<char> span, Func<char, bool> match)
    {
        // Assume that in most cases input doesn't need trimming
        if (span.Length == 0 || (!match(span[0]) && !match(span[^1])))
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
    /// Removes all leading characters matching the `match` delegate from the span.
    /// </summary>
    /// <param name="span">The source span from which the characters are removed.</param>
    /// <param name="match">Delegate match function.</param>
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
    /// Removes all trailing characters matching the `match` delegate from the span.
    /// </summary>
    /// <param name="span">The source span from which the characters are removed.</param>
    /// <param name="match">Delegate match function.</param>
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

    /// <summary>Counts the number of times the specified delegate <paramref name="match"/> returns true in the <paramref name="span"/>.
    /// </summary>
    /// <param name="span">The span to search.</param>
    /// <param name="match">The delegate for which to search.</param>
    /// <returns>The number of times <paramref name="match"/> returned true in the <paramref name="span"/>.</returns>
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

    /// <summary>Counts the number of times the specified delegate <paramref name="match"/> returns true before returning false
    /// starting at the beginning of the <paramref name="span"/>.
    /// </summary>
    /// <param name="span">The span to search.</param>
    /// <param name="match">The delegate for which to search.</param>
    /// <returns>The number of times <paramref name="match"/> returned true in the <paramref name="span"/>.</returns>
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

    /// <summary>Counts the number of times the specified delegate <paramref name="match"/> returns true before returning false
    /// starting at the end of the <paramref name="span"/>.
    /// </summary>
    /// <param name="span">The span to search.</param>
    /// <param name="match">The delegate for which to search.</param>
    /// <returns>The number of times <paramref name="match"/> returned true in the <paramref name="span"/>.</returns>
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
}
